using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.VarMod.Framerate
{
    internal class FPSKeybindsView
    {
        public Popup CreateKeybindPopup = new Popup("Create FPS Keybind");
        

        public List<FPSKeybindsView> Keybinds = new List<FPSKeybindsView>();


        internal FPSKeybindsView()  
        {
                
        }


    }
}
