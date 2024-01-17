using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    [StructLayout(LayoutKind.Sequential)]
    public struct vec3_t
    {
        public float x;
        public float y; 
        public float z;

        public override string ToString()
        {
            return $"X: {x}, Y: {y}, Z {z}";
        }
    }
}
