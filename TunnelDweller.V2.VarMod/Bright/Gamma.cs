using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Threading;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.VarMod.Bright
{
    internal static class Gamma
    {
        public static Label lblGammaInfo = new Label("Change the in game Gamma Value to values outside of the scope of the game. Game Captures are unaffected!");
        public static Slider slGamma = new Slider("Gamma", 1, 0.1f, 2f);
        public static CheckBox cbEnabled = new CheckBox("Enable Gamma", false);


        public static float Original = 0f;


        public static void Initialize()
        {
            //cbEnabled.Checked = Varm.Config.GammaEnabled;
            //slGamma.Value = Varm.Config.Gamma;

            Varm.VarModTab.Controls.Add(lblGammaInfo);
            Varm.VarModTab.Controls.Add(cbEnabled);
            Varm.VarModTab.Controls.Add(slGamma);
            Varm.VarModTab.Controls.Add(new Seperator());

            Original = Variables.Gamma;

            Update.OnUpdate += Update_OnUpdate;
        }

        private static void Update_OnUpdate(object sender, EventArgs e)
        {
            //Varm.Config.GammaEnabled = cbEnabled.Checked;
            //Varm.Config.Gamma = slGamma.Value;

            if(!cbEnabled.Checked)
            {
                Variables.Gamma = Original;
                return;
            }


            Variables.Gamma = slGamma.Value;
        }
    }
}
