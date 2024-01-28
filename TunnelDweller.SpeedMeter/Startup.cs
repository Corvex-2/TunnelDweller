using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Moduling;
using TunnelDweller.NetCore.Rendering;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.SpeedMeter
{
    public class Startup : ModuleEntryPoint
    {
        public override void Initialize()
        {
            Speedometer.Initialize();
        }
    }
}