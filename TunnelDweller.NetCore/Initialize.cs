using System;
using System.Diagnostics;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Extensions;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Input;
using TunnelDweller.NetCore.MinHook;
using TunnelDweller.NetCore.Moduling;
using TunnelDweller.NetCore.Rendering;
using TunnelDweller.NetCore.Threading;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.NetCore
{
    public class Initialize
    {
        public static int Main(string[] args)
        {
            Console.WriteLine($"TunnelDweller.NetCore loaded successfully into {Process.GetCurrentProcess().ProcessName} / AppDomain {AppDomain.CurrentDomain.FriendlyName}");

            typeof(ImGui).PopulateDefinitions(args, "[Imgui_Net]");
            typeof(Renderer).PopulateDefinitions(args, "[CallbackRenderer]");
            typeof(InputManager).PopulateDefinitions(args, "[CallbackInput]");
            typeof(MH).PopulateDefinitions(args, "[Minhook]");
            typeof(ModuleManager).PopulateDefinitions(args, "[ModuleManager]");
            Update.Initialize();
            Renderer.Initialize();
            InputManager.Initialize();
            Window.Initialize();
            ModuleManager.Initialize(); // requires polishing, but works.
            return 1;
        }
    }
}
