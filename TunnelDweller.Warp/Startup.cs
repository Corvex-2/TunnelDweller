using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Moduling;

namespace TunnelDweller.Warp
{
    public class Startup : ModuleEntryPoint
    {
        public override void Initialize()
        {
            WarpManager.Initialize();
        }
    }
}
