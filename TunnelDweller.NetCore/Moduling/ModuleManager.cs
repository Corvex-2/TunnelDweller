using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Input;
using TunnelDweller.NetCore.Randomness;
using TunnelDweller.NetCore.Rendering;
using TunnelDweller.NetCore.Windowing;
using TunnelDweller.NetCore.Game;
using System.Reflection;
using TunnelDweller.NetCore.Threading;
using System.Net;
using TunnelDweller.NetCore.API;

namespace TunnelDweller.NetCore.Moduling
{
    internal static class ModuleManager
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void ReloadDomInternal_t([MarshalAs(UnmanagedType.LPWStr)]string lpName);
        internal static ReloadDomInternal_t smReloadDomInternal;

        internal static TabItem ModuleManagerTab;
        internal static List<ModuleView> ModuleViews;

        internal static bool networked;

        internal static string ModulePath
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.FileName.Replace("metro.exe", "modules\\");
            }
        }

        internal static void Initialize()
        {
            if (!Directory.Exists(ModulePath))
                Directory.CreateDirectory(ModulePath);

            ModuleManagerTab = new TabItem("Modules");
            ModuleViews = new List<ModuleView>();

            Window.MainTabs.Items.Add(ModuleManagerTab);

            Update.OnUpdate += Update_OnUpdate;
            InputManager.InputChanged += InputManager_InputChanged;
        }

        private static void LoadNetworked()
        {
            if (networked)
                return;

            foreach(var mod in TechnicalMetroApi.GetModules())
            {
                var data = mod.moduleData;
                var dot = mod.moduleName.LastIndexOf('.');
                var name = mod.moduleName;
                if(dot != -1)
                    name = mod.moduleName.Substring(0, dot);

                var module = new ModuleBase($"http://api.technicaldifficulties.de\\{name}", name, data, true);
                var view = new ModuleView(module);


                ModuleViews.Add(view);
                ModuleManagerTab.Controls.Add(view);
            }
            networked = true;
        }

        private static void Update_OnUpdate(object sender, EventArgs e)
        {
            if (ModuleViews == null || ModuleManagerTab == null)
                return;

            LoadNetworked();

            var files = Directory.GetFiles(ModulePath);

            for (int i = 0; i < files.Length; i++)
            {
                if (ModuleViews.Any(x => x.Module.FileName == Path.GetFileNameWithoutExtension(files[i])))
                    continue;


                try
                {
                    AssemblyName.GetAssemblyName(files[i]);
                }
                catch
                {
                    continue;
                }

                var module = new ModuleBase(files[i], Path.GetFileNameWithoutExtension(files[i]), File.ReadAllBytes(files[i]));
                var view = new ModuleView(module);

                ModuleViews.Add(view);
                ModuleManagerTab.Controls.Add(view);
            }
        }

        private static void InputManager_InputChanged(object sender, InputEventArgs e)
        {
            if (e.skCode == 0x58 && e.State)
                ForceReload();
        }

        internal static void ForceReload()
        {
            if(smReloadDomInternal == null)
            {
                return;
            }

            new Task(() => { // The actual reload has to be done from a seperate thread, as executing it from the draw thread for example would cause issues with unregistering the renderer callbacks.
                             // We don't care about the Task Return as the dom gets unloaded and the thread destroyed by the runtime anyway.

                Console.WriteLine("Commencing Reload Procedure.");

                Renderer.Uninitialize();
                InputManager.Uninitialize();

                var domName = $"TunnelDweller::ModuleDom::Mod{SecureRandom.NextString(12, SecureRandom.DEFAULT_CHAR_SET)}";

                smReloadDomInternal(domName); // The AppDomain should be created by the C++/unmanaged core, otherwise the current AppDomain takes ownership of the new one and would get unloaded with the current one.

                AppDomain.Unload(AppDomain.CurrentDomain);

            }).Start();
        }
    }
}
