using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Threading;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.WidescreenFix
{
    internal static class Widescreen
    {
        internal static TabItem tiWidescreenTab = new TabItem("Widescreen Fix");
        internal static Label lblStatus = new Label("Fixes Widescreen duh");
        internal static ComboBox cmbMode = new ComboBox("Aspect Ratio Mode", new string[] { "Auto", "Preset", "Custom" });
        internal static TextBox txtWidth = new TextBox("Width", "") { Visible = false };
        internal static TextBox txtHeight = new TextBox("Height", "") { Visible = false };
        internal static ComboBox cmbCommonAspectRatio = new ComboBox("Aspect Ratio", new string[] { "3:2", "4:3", "5:4", "15:9", "16:9", "16:10", "21:9", }) { Visible = false };

        internal static bool Patched { get; set; } = false;

        internal static List<WidescreenPatch> Patches { get; set; } = new List<WidescreenPatch>() 
        { 
            new WidescreenPatch(0x698B2C, 5),
            new WidescreenPatch(0x66DDB1, 10),
            new WidescreenPatch(0x66E048, 9),
            new WidescreenPatch(0x5753C3, 10),
            new WidescreenPatch(0x575A84, 8),
            new WidescreenPatch(0x66844A, 10),
            new WidescreenPatch(0x668B58, 8),
            new WidescreenPatch(0x83CE2B, 8),
            new WidescreenPatch(0x698ADD, 3),
            new WidescreenPatch(0x698B36, 3)
        };

        internal static void Initialize()
        {
            if (Patched) return;

            tiWidescreenTab.Controls.Add(lblStatus);
            tiWidescreenTab.Controls.Add(new Seperator());
            tiWidescreenTab.Controls.Add(cmbMode);
            tiWidescreenTab.Controls.Add(new Seperator());
            tiWidescreenTab.Controls.Add(cmbCommonAspectRatio);
            tiWidescreenTab.Controls.Add(txtWidth);
            tiWidescreenTab.Controls.Add(txtHeight);

            Window.MainTabs.Items.Add(tiWidescreenTab);

            for(int i = 0; i < Patches.Count; i++)
            {
                Patches[i].Patch();
            }

            Patched = Patches.All(x => x.Patched);

            Update.OnUpdate += Update_OnUpdate;
        }

        private static void Update_OnUpdate(object sender, EventArgs e)
        {
            if (!Patched || !Patches.All(x => x.Patched))
                return;

            txtWidth.Visible = cmbMode.SelectedIndex == 2;
            txtHeight.Visible = cmbMode.SelectedIndex == 2;
            cmbCommonAspectRatio.Visible = cmbMode.SelectedIndex == 1;

            switch (cmbMode.SelectedIndex)
            {
                case 0:
                    AutoAspectRatio();
                    break;
                case 1:
                    CustomDropdownRatio();
                    break;
                case 2:
                    CustomAspectRatio();
                    break;
            }
        }

        internal static void CustomDropdownRatio()
        {
            if (Patched && Patches.All(x => x.Patched))
            {
                switch (cmbCommonAspectRatio.SelectedIndex)
                {
                    default:
                        AutoAspectRatio();
                        break;
                    case 0:
                        Variables.AspectRatio = 3f / 2f;
                        break;
                    case 1:
                        Variables.AspectRatio = 4f / 3f;
                        break;
                    case 2:
                        Variables.AspectRatio = 5f / 4f;
                        break;
                    case 3:
                        Variables.AspectRatio = 15f / 9f;
                        break;
                    case 4:
                        Variables.AspectRatio = 16f / 9f;
                        break;
                    case 5:
                        Variables.AspectRatio = 16f / 10f;
                        break;
                    case 6:
                        Variables.AspectRatio = 21f / 9f;
                        break;
                }
            }
        }

        internal static void CustomAspectRatio()
        {
            if (Patched && Patches.All(x => x.Patched))
            {
                if (float.TryParse(txtWidth.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float width) && float.TryParse(txtHeight.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float height))
                {
                    Variables.AspectRatio = width / height;
                }
            }
        }

        internal static void AutoAspectRatio()
        {
            if (Patched && Patches.All(x => x.Patched))
            {
                Variables.AspectRatio = (((float)Variables.Width) / ((float)Variables.Height));
            }
        }
    }
}
