using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Moduling;
using TunnelDweller.NetCore.Windowing;
using TunnelDweller.VarMod.Bright;
using TunnelDweller.VarMod.Framerate;
using TunnelDweller.VarMod.Speed;

namespace TunnelDweller.VarMod
{
    public class Varm : ModuleEntryPoint
    {
        public static TabItem VarModTab { get; private set; } = new TabItem("VarMod");

        public override void Initialize()
        {
            VarModTab.Controls.Add(new Label("VarMod allows you to edit certain in game variables."));
            VarModTab.Controls.Add(new Seperator());

            SpeedHack.Initialize();
            FPS.Initialize();
            Gamma.Initialize();

            Window.MainTabs.Items.Add(VarModTab);
        }
    }
}
