using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Threading;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.VarMod.FOV
{
    public static class FOVMan
    {
        internal static Label lbFovHackInfo = new Label("Change the in game Field of View.");
        internal static Slider slFov = new Slider("Field of View", 100, 1, 180, 0);

        public static void Initialize()
        {
            Varm.VarModTab.Controls.Add(lbFovHackInfo);
            Varm.VarModTab.Controls.Add(slFov);
            Varm.VarModTab.Controls.Add(new Seperator());

            Update.OnUpdate += Update_OnUpdate;
        }

        private static void Update_OnUpdate(object sender, EventArgs e)
        {
            Variables.FieldOfView = slFov.Value;
        }
    }
}
