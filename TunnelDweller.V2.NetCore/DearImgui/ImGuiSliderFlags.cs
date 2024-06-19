using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    public enum ImGuiSliderFlags : int
    {
        None = 0,
        AlwaysClamp = 1 << 4,
        Logarithmic = 1 << 5,
        NoRoundToFomrat = 1 << 6,
        NoInput = 1 << 7,
    }
}
