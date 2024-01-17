using System;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TunnelDweller.NetCore.Extensions
{
    internal static class TypeEx
    {
        internal static bool HasFieldDefinition(this Type t, string FieldName)
        {
            return t.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name == FieldName).Count() > 0;
        }
        internal static bool HasDelegateDefinition(this Type t, string DelegateName)
        {
            return t.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public).Where(f => f.Name == DelegateName).Count() > 0;
        }
        internal static FieldInfo GetFieldDefinition(this Type t, string FieldDefinition)
        {
            return t.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name == FieldDefinition).First();
        }
        internal static Type GetDelegateDefinition(this Type t, string def)
        {
            return t.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public).Where(f => f.Name == def).First();
        }
        internal static void PopulateDefinitions(this Type populationTarget, string[] arguments, params string[] columns)
        {
            string currentColumn = "";
            foreach (var cppdef in arguments)
            {
                if (cppdef.StartsWith("["))
                    currentColumn = cppdef;

                if (string.IsNullOrWhiteSpace(cppdef) || !cppdef.Contains(":") || !columns.Contains(currentColumn))
                    continue;

                var split = cppdef.Split(':');
                var definition = split[0];
                var pointer_as_string = split[1];

                if (long.TryParse(pointer_as_string, NumberStyles.Number, CultureInfo.InvariantCulture, out var pointer_as_long))
                {
                    //Console.WriteLine($"Parsing {definition} on Renderer as {pointer_as_long.ToString("X")}.");

                    if (populationTarget.HasFieldDefinition("sm" + definition) && populationTarget.HasDelegateDefinition(definition + "_t"))
                    {
                        var fieldDefinition = populationTarget.GetFieldDefinition("sm" + definition);
                        var delegateDefinition = populationTarget.GetDelegateDefinition(definition + "_t");

                        try
                        {
                            if (fieldDefinition == null || delegateDefinition == null)
                            {
                                Console.WriteLine($"Parser was unable to locate definitions for {definition}!\r\nField State: {fieldDefinition}\r\nDelegate State: {delegateDefinition}");
                                continue;
                            }

                            //Console.WriteLine($"Attempting to create and set {fieldDefinition.Name} to {delegateDefinition.Name} using {pointer_as_long.ToString("X")}");

                            var constructedDelegate = Marshal.GetDelegateForFunctionPointer((IntPtr)pointer_as_long, delegateDefinition);

                            if (constructedDelegate != null && fieldDefinition != null)
                                fieldDefinition.SetValue(null, constructedDelegate);
                            else
                                Console.WriteLine("Unable to create delegate type or set field definition!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                        Console.WriteLine($"Parser was unable to locate definitions for {definition}!\r\nHas Static: {populationTarget.HasFieldDefinition("sm" + definition)}\r\nHas Delegate: {populationTarget.HasDelegateDefinition(definition + "_t")}");
                }
            }
        }
    }
}
