using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace TunnelDweller.Memory
{
    public static class MemoryManager
    {
        static MemoryManager()
        {
            PrepareZwReadVirtualMemory();
        }

        internal static Process Process
        {
            get
            {
                return Process.GetCurrentProcess();
            }
        }
        internal static IntPtr Handle
        {
            get
            {
                return Process.Handle;
            }
        }

        #region Reading & Writing Memory

        #region SPECIAL: ntdll.NtReadVirtualMemory
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint NtReadVirtualMemory32(IntPtr hProcess, IntPtr lpBaseAddress, ref byte[] buffer, uint lpNumberOfBytesToRead, out uint lpNumberOfBytesRead);
        public static NtReadVirtualMemory32 NtReadVirtualMemory = null;
        
        private static IntPtr NtReadVirtualMemory_Pointer;
        private static byte[] NtReadVirtualMemory_Shellcode =
        {
            0xB8, 0x3F, 0x00, 0x00, 0x00,   //mov eax, 0x3F
            0xBA, 0x00, 0x00, 0x00, 0x00,   //mov edx, ntdll.RtlInterlockedCompareExchange+170
            0xFF, 0xD2,                     //call edx
            0xC2, 0x14, 0x00,               //ret 0x0014
            0x90,                           //nop
        };

        public static void PrepareZwReadVirtualMemory()
        {
            if (NtReadVirtualMemory != null)
                return;

            var RtlInterlockedCompareExchange64 = GetFunctionPointer("RtlInterlockedCompareExchange64", "ntdll") + 0x170;
            Array.Copy(BitConverter.GetBytes(RtlInterlockedCompareExchange64.ToInt32()), 0, NtReadVirtualMemory_Shellcode, 7, 4);

            NtReadVirtualMemory_Pointer = Marshal.AllocHGlobal(NtReadVirtualMemory_Shellcode.Length);

            Write(NtReadVirtualMemory_Pointer, NtReadVirtualMemory_Shellcode);

            NtReadVirtualMemory = Marshal.GetDelegateForFunctionPointer<NtReadVirtualMemory32>(NtReadVirtualMemory_Pointer);
        }

        public static byte[] ReadStealth(IntPtr Pointer, int Size)
        {
            byte[] _buffer = new byte[Size];
            Natives.ReadProcessMemory(Process.Handle, Pointer, _buffer, (uint)Size, out var _);
            //try
            //{
            //    if (!IsBadReadPtr(Pointer, out var mbi))
            //        var s = NtReadVirtualMemory(Natives.GetCurrentProcess(), Pointer, ref _buffer, (uint)Size, out var _);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error on Read: {Pointer.ToString("X")}");
            //}
            return _buffer;
        }
        #endregion

        public static byte[] Read(IntPtr Pointer, int Size)
        {
            byte[] _buffer = new byte[Size];
            Marshal.Copy(Pointer, _buffer, 0, Size);
            return _buffer;
        }
        public static bool Write(IntPtr Pointer, byte[] Buffer)
        {
            if (!Natives.VirtualProtect(Pointer, (uint)Buffer.Length, PROTECTION.PAGE_EXECUTE_READWRITE, out var prot))
#if DEBUG
                throw new InvalidOperationException("Unable to change protection of memory page.");
#else
                return false;
#endif

            Marshal.Copy(Buffer, 0, Pointer, Buffer.Length);

            if (!Natives.VirtualProtect(Pointer, (uint)Buffer.Length, prot, out _))
#if DEBUG
                throw new InvalidOperationException("Unable to change protection of memory page.");
#else
                return false;
#endif

            return true;
        }

        internal static unsafe byte[] URead(IntPtr Pointer, int Size)
        {
            unsafe
            {
                byte* src = (byte*)Pointer;
                var buffer = new byte[Size];
                for (int i = 0; i < Size; i++)
                {
                    buffer[i] = src[i];
                }
                return buffer;
            }
        }
        internal static string ReadString(IntPtr Pointer)
        {
            return Marshal.PtrToStringUni(Pointer);
        }
        internal static unsafe T Read<T>(IntPtr Pointer) where T : struct
        {
            if(Pointer.ToInt64() < 0xFFFF) //Hack to pervent crashes with invalid pointers. reading values below 0xFFFF shouldnt happen, if we ever do come across that, we just return default duh.
                return default(T);

            return Marshal.PtrToStructure<T>(Pointer);
        }
        internal static T URead<T>(IntPtr Pointer) where T : struct
        {
            return Marshal.PtrToStructure<T>(Pointer);
        }
        internal static bool UWrite(IntPtr Pointer, byte[] Buffer)
        {
            if (!Natives.VirtualProtect(Pointer, (uint)Buffer.Length, PROTECTION.PAGE_EXECUTE_READWRITE, out var prot))
#if DEBUG
                throw new InvalidOperationException("Unable to change protection of memory page.");
#else
                return false;
#endif

            unsafe
            {
                byte* src = (byte*)Pointer;
                for (int i = 0; i < Buffer.Length; i++)
                {
                    src[i] = Buffer[i];
                }
            }

            if (!Natives.VirtualProtect(Pointer, (uint)Buffer.Length, prot, out _))
#if DEBUG
                throw new InvalidOperationException("Unable to change protection of memory page.");
#else
                return false;
#endif

            return true;
        }
        public static bool UWrite<T>(IntPtr Pointer, T Value) where T : struct
        {
            int _size = Marshal.SizeOf<T>();
            byte[] _buffer = new byte[_size];
            IntPtr _nativePointer = Marshal.AllocHGlobal(_size);
            Marshal.StructureToPtr<T>(Value, _nativePointer, true);
            Marshal.Copy(_nativePointer, _buffer, 0, _size);
            Marshal.FreeHGlobal(_nativePointer);

            return UWrite(Pointer, _buffer);
        }
        public static bool Write<T>(IntPtr Pointer, T Value) where T : struct
        {
            int _size = Marshal.SizeOf<T>();
            byte[] _buffer = new byte[_size];
            IntPtr _nativePointer = Marshal.AllocHGlobal(_size);
            Marshal.StructureToPtr<T>(Value, _nativePointer, true);
            Marshal.Copy(_nativePointer, _buffer, 0, _size);
            Marshal.FreeHGlobal(_nativePointer);

            return Write(Pointer, _buffer);
        }
        public static bool WriteString(IntPtr Pointer, string Value)
        {
            var data = Encoding.Unicode.GetBytes(Value);
            return Write(Pointer, data);
        }
        #endregion

        #region Function Retrieval
        public static IntPtr GetFunctionPointer(string Function, string Module)
        {
            IntPtr mHandle = Natives.GetModuleHandle(Module);

            if (mHandle == IntPtr.Zero)
                return IntPtr.Zero;

            IntPtr fPointer = Natives.GetProcAddress(mHandle, Function);
            return fPointer;
        }
        public static IntPtr GetFunctionPointer(string Function)
        {
            IntPtr fPointer = IntPtr.Zero;
            foreach (ProcessModule nModule in Process.Modules)
            {
                if ((fPointer = GetFunctionPointer(Function, nModule.ModuleName)) != IntPtr.Zero)
                    break;
            }
            return fPointer;
        }
        public static MethodInfo GetMethodByName(string Method)
        {
            BindingFlags mFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;
            var x = AppDomain.CurrentDomain.GetAssemblies();
            foreach(Assembly asm in x)
            {
                foreach (Type xx in asm.GetTypes())
                {
                    foreach (var m in xx.GetMethods(mFlags))
                    {
                        if (m.Name == Method)
                        {
                            return m;
                        }
                    }
                }
            }
            return default;
        }
        public static List<MethodInfo> GetAllMethodsByName(string Method)
        {
            var ret = new List<MethodInfo>();
            BindingFlags mFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;
            var x = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in x)
            {
                foreach (Type xx in asm.GetTypes())
                {
                    foreach (var m in xx.GetMethods(mFlags))
                    {
                        if (m.Name == Method)
                        {
                            ret.Add(m);
                        }
                    }
                }
            }
            return ret;
        }
        public static List<MethodInfo> GetAllMethodsByName(string Method, int assemblyIndex)
        {
            var ret = new List<MethodInfo>();
            BindingFlags mFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;
            var x = AppDomain.CurrentDomain.GetAssemblies()[assemblyIndex];
            //foreach (Assembly asm in x)
            //{
                foreach (Type xx in x.GetTypes())
                {
                    foreach (var m in xx.GetMethods(mFlags))
                    {
                        if (m.Name == Method)
                        {
                            ret.Add(m);
                        }
                    }
                }
            //}
            return ret;
        }
        public static MethodInfo GetMethodByPointer(IntPtr Pointer, int AssemblyIndex)
        {
            var plookup = PreparedMethods.Where(x => x.Item2 == Pointer).FirstOrDefault().Item1;
            if (plookup != null && plookup != default(MethodInfo))
                return plookup;


            var assembly = AppDomain.CurrentDomain.GetAssemblies()[AssemblyIndex];
            var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;
            foreach(var t in assembly.GetTypes())
            {
                if (t == null || t == default(Type))
                    continue;

                foreach(var m in t.GetMethods(flags))
                {
                    if (m.IsAbstract || m.IsGenericMethod || m.IsGenericMethodDefinition)
                        continue;
                    if (m == null || m == default(MethodInfo))
                        continue;

                    if (m.MethodHandle == null || m.MethodHandle.Value == IntPtr.Zero)
                        RuntimeHelpers.PrepareMethod(m.MethodHandle);

                    if (Pointer == m.MethodHandle.GetFunctionPointer())
                        return m;

                }
            }
            return default;
        }
        public static MethodInfo GetMethodByPointer(IntPtr Pointer)
        {
            var plookup = PreparedMethods.Where(io => io.Item2 == Pointer).FirstOrDefault().Item1;
            if (plookup != null && plookup != default(MethodInfo))
                return plookup;


            BindingFlags mFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;
            var x = AppDomain.CurrentDomain.GetAssemblies();
            foreach(Assembly asm in x)
            {
                foreach (Type xx in asm.GetTypes())
                {
                    if (xx.IsGenericType || xx.IsAbstract)
                        continue;

                    foreach (var m in xx.GetMethods(mFlags))
                    {
                        if (m.IsAbstract || m.IsGenericMethod || m.IsGenericMethodDefinition)
                            continue;
                        try
                        {
                            //RuntimeHelpers.PrepareMethod(m.MethodHandle);
                            IntPtr mPtr = m.MethodHandle.GetFunctionPointer();
                            if (mPtr.ToInt32() == (int)Pointer)
                            {
                                return m;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception Orrcured.");
                        }
                    }
                }
            }
            return default;
        }
        public static MethodInfo GetMethodByToken(int Token, int AssemblyIndex)
        {
            return (MethodInfo)AppDomain.CurrentDomain.GetAssemblies()[AssemblyIndex].ManifestModule.ResolveMethod(Token);
        }
        public static MethodInfo GetMethodByToken(int Token, Assembly Target)
        {
            return (MethodInfo)Target.ManifestModule.ResolveMethod(Token);
        }
        public static unsafe TDest ReinterpretCast<TDest>(object source)
        {
            var sourceRef = __makeref(source);
            var dest = default(TDest);
            var destRef = __makeref(dest);
            *(IntPtr*)&destRef = *(IntPtr*)&sourceRef;
            return __refvalue(destRef, TDest);
        }
        private static List<(MethodInfo, IntPtr)> PreparedMethods = new List<(MethodInfo, IntPtr)>();
        private static List<(ConstructorInfo, IntPtr)> PreparedConstructors = new List<(ConstructorInfo, IntPtr)>();
        public static void PrepareMethods(int AssemblyIndex)
        {
            var Assembly = AppDomain.CurrentDomain.GetAssemblies()[AssemblyIndex];
            foreach (Type T in Assembly.GetTypes())
            {
                foreach(var M in T.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    try
                    {
                        RuntimeHelpers.PrepareMethod(M.MethodHandle);

                        PreparedMethods.Add((M, M.MethodHandle.GetFunctionPointer()));

                        //Console.WriteLine("Preparing Method: " + M.Name + " success. " + (M.MethodHandle.GetFunctionPointer().ToString("X")));
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("Preparing Method: " + M.Name + " failed.");
                    }
                }
            }
        }
        public static void PrepareConstructors(int AssemblyIndex)
        {
            var Assembly = AppDomain.CurrentDomain.GetAssemblies()[AssemblyIndex];
            foreach (Type T in Assembly.GetTypes())
            {
                foreach (var M in T.GetConstructors(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    try
                    {
                        RuntimeHelpers.PrepareMethod(M.MethodHandle);

                        PreparedConstructors.Add((M, M.MethodHandle.GetFunctionPointer()));

                        //Console.WriteLine("Preparing Method: " + M.Name + " success. " + (M.MethodHandle.GetFunctionPointer().ToString("X")));
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("Preparing Method: " + M.Name + " failed.");
                    }
                }
            }
        }
        public static IntPtr PrepareMethod(MethodInfo Method)
        {
            RuntimeHelpers.PrepareMethod(Method.MethodHandle);
            return Method.MethodHandle.GetFunctionPointer();
        }
        #endregion

    }
}
