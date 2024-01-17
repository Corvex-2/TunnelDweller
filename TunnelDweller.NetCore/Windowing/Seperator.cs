using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Windowing
{
    public class Seperator : Control
    {
        public bool Visible { get; set; } = true;

        public override void Paint()
        {
            if (!Visible)
                return;

            ImGui.Seperator();
        }

        public override bool Visibility()
        {
            return Visible;
        }
    }
}
