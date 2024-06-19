using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Configure;

namespace TunnelDweller.VarMod
{
    internal class VarmConfig : Config
    {
        public VarmConfig(string Name, string Description) : base(Name, Description) { }

        public bool GammaEnabled { get; set; } = true;
        public float Gamma { get; set; } = 1f;

        public bool SpeedEnabled { get; set; } = false;
        public float Speed { get; set; } = 100f;

        public bool FrameRateEnabled { get; set; } = true;
        public int FrameRate { get; set; } = 1000;
    }
}
