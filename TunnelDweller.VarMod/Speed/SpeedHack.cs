using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Threading;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.VarMod.Speed
{
    internal static class SpeedHack
    {
        internal static SpeedHackPatch Patch = new SpeedHackPatch(0x658F9F, 10);

        internal static Label lbSpeedHackInfo = new Label("Change the in game Timescale. Sounds are affected for the most part but not all!");
        internal static Slider slSpeed = new Slider("Timescale", 100, 1, 100f, 0);
        internal static CheckBox cbEnabled = new CheckBox("Enable SpeedHack", true);

        public static void Initialize()
        {
            if (!Patch.IsPatchedAlready() && !Patch.Patched)
                Patch.Patch();

            Varm.VarModTab.Controls.Add(lbSpeedHackInfo);
            Varm.VarModTab.Controls.Add(cbEnabled);
            Varm.VarModTab.Controls.Add(slSpeed);
            Varm.VarModTab.Controls.Add(new Seperator());

            Update.OnUpdate += Update_OnUpdate;
        }

        private static void Update_OnUpdate(object sender, EventArgs e)
        {
            if (!Patch.IsPatchedAlready())
                return;

            if (cbEnabled.Checked)
                Variables.Timescale = slSpeed.Value / 100f;
            else
                Variables.Timescale = 1f;
        }
    }
}
