using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct NetFontInfo_t
    {
        private char* name;
        internal float fontsize;
        internal int datasize;
        internal IntPtr nativeptr;
        internal IntPtr imguiptr;

        public string GetName()
        {
            return new string(name);
        }
    }
}
