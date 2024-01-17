using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    [StructLayout(LayoutKind.Sequential)]
    public struct m16_t
    {
        public float m11;
        public float m12;
        public float m13;
        public float m14;
        public float m21;
        public float m22;
        public float m23;
        public float m24;
        public float m31;
        public float m32;
        public float m33;
        public float m34;
        public float m41;
        public float m42;
        public float m43;
        public float m44;


        public override string ToString()
        {
            return $"m11: {m11} m12: {m12} m13: {m13} m14: {m14}\r\nm21: {m21} m22: {m22} m23: {m23} m24: {m24}\r\nm31: {m31} m32: {m32} m33: {m33} m34: {m34}\r\nm41: {m41} m42: {m42} m43: {m43} m44: {m44}";
        }
    }
}
