using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Threading;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.VarMod.Framerate
{
    internal static class FPS
    {
        internal static Label lblFPSInfo = new Label("Change your Target FPS.");
        internal static Slider slFPS = new Slider("Target FPS", 1000, 1, 1000, 0);
        internal static CheckBox cbEnabled = new CheckBox("Enable FPS", true);

        internal static int Original = 0;

        public static void Initialize()
        {
            //cbEnabled.Checked = Varm.Config.FrameRateEnabled;
            //slFPS.Value = Varm.Config.FrameRate;

            Varm.VarModTab.Controls.Add(lblFPSInfo);
            Varm.VarModTab.Controls.Add(cbEnabled);
            Varm.VarModTab.Controls.Add(slFPS);
            Varm.VarModTab.Controls.Add(new Seperator());

            Original = Variables.Target_FPS;

            Update.OnUpdate += Update_OnUpdate;
        }

        private static void Update_OnUpdate(object sender, EventArgs e)
        {
            //Varm.Config.SpeedEnabled = cbEnabled.Checked;
            //Varm.Config.Speed = slFPS.Value;

            if (!cbEnabled.Checked)
            {
                Variables.Target_FPS = Original;
                return;
            }

            Variables.Target_FPS = (int)slFPS.Value;
        }
    }
}
