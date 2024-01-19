using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Moduling;

namespace TunnelDweller.WidescreenFix
{
    public class Startup : ModuleEntryPoint
    {
        public override void Initialize()
        {
            Widescreen.Initialize();
        }
    }
}
