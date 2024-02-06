using System;
using System.Diagnostics;
using System.Text;
using TunnelDweller.NetCore.API;
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
            
            Console.WriteLine($"Writing CCAVE Data");

            Variables.MemoryManager.WriteString(Offsets.CCAVEPTR, "TunnelDweller", Encoding.ASCII);

            if (Variables.MemoryManager.ReadString(Offsets.CCAVEPTR, Encoding.ASCII, 15) == "TunnelDweller")
                Console.WriteLine("CCAVE Data success!");
            else
                Console.WriteLine("CCAVE Data failed!");

            TechnicalMetroApi.RELEASESTREAM = Variables.MemoryManager.ReadString(Variables.MemoryManager.Base + 0x3b0 + 0x20, Encoding.ASCII, 32);

            Console.Title = $"TunnelDweller - Build: {TechnicalMetroApi.RELEASESTREAM}";

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

            Popup info = new Popup("Tunnel Dweller###notice");
            info.Controls.Add(new Label($"Welcome to Tunnel Dweller - a Speedrunning Toolkit and much more for the metro games.\r\n\r\nCurrently Supported are the following games: \r\n\r\n"));
            info.Controls.Add(new Label($"Metro 2033 Redux") { Color = new col32_t(.3f, .3f, .9f, 1f) });
            info.Controls.Add(new Label($"Metro Last Light Reudx") { Color = new col32_t(.3f, .3f, .9f, 1f) });
            info.Controls.Add(new Label($"You can open the menu by pressing "));
            info.Controls.Add(new Label("DEL or DELETE") { Color = new col32_t(0f, 1f, 0.33f), Sameline = true });
            info.Controls.Add(new Label(" on your Keyboard.") { Sameline = true });
            info.Controls.Add(new Seperator());
            info.Controls.Add(new Label("Please keep in mind that Tunnel Dweller is still in BETA. \r\nCrashes and Performance issues are to be expected. If you encounter any bugs, please report them to Corvex via GitHub or Discord.") { Color = new col32_t(1f, 0.8f, 0.8f) });
            info.Controls.Add(new Label("Discord: corvex5"));
            info.Controls.Add(new Label("GitHub: /Corvex-2"));
            info.Controls.Add(new Seperator());
            info.Controls.Add(new Button("Open on GitHub", new Action(() => { Process.Start("https://github.com/Corvex-2/TunnelDweller"); })));
            info.Controls.Add(new Button("Continue", new Action(() => { info.Active = false; ImGui.CloseCurrentPopup(); })) { Sameline = true });
            info.Active = true;


            return 1;
        }
    }
}
