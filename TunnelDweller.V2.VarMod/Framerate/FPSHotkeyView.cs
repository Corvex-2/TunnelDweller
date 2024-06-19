using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.VarMod.Framerate
{
    internal class FPSHotkeyView : Control
    {
        public Hotkey Hotkey { get; set; } = new Hotkey(0x00);
        public Slider Slider { get; set; } = new Slider("Target FPS", 1000, 1, 1000, 0);

        public FPSHotkeyView(int diKey = 0x00)
        { 
            if(diKey != 0x00)
            {
                Hotkey.Key = diKey;
            }
        }

        public override void Paint()
        { 
            if(Visibility())

            Hotkey.Paint();
            Slider.Paint();
        }

        public override bool Visibility()
        {
            return true;
        }
    }
}
