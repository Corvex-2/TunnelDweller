using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    [StructLayout(LayoutKind.Sequential)]
    public struct vec2_t
    {
        public float x;
        public float y;

        public vec2_t(float x, float y)
        {
            this.x = x; this.y = y;
        }

        public static vec2_t Zero
        {
            get
            {
                return new vec2_t() { x = 0, y = 0 }; 
            }
        }
        public static vec2_t One
        {
            get
            {
                return new vec2_t() { x = 1, y = 1 };
            }
        }
        public static vec2_t MinusOne
        {
            get 
            { 
                return new vec2_t() { x = -1, y = -1 };
            }
        }
        public static vec2_t operator +(vec2_t a) => new vec2_t(Math.Abs(a.x), Math.Abs(a.y));
        public static vec2_t operator -(vec2_t a) => new vec2_t(-a.x, -a.y);
        public static vec2_t operator +(vec2_t a, vec2_t b) => new vec2_t(a.x + b.x, a.y + b.y);
        public static vec2_t operator -(vec2_t b, vec2_t a) => new vec2_t(b.x - a.x, b.y - a.y);
        public static vec2_t operator *(vec2_t a, vec2_t b) => new vec2_t(a.x * b.x, a.y * b.y);
        public static vec2_t operator /(vec2_t a, vec2_t b) => new vec2_t(a.x / b.x, a.y / b.y);
        public static bool operator ==(vec2_t a, vec2_t b)
        {
            return (a.x == b.y && a.y == b.y);
        }
        public static bool operator !=(vec2_t a, vec2_t b)
        {
            return (a.x != b.x || a.y != b.y);
        }
    }
}
