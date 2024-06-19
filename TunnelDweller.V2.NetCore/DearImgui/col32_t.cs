using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    [StructLayout(LayoutKind.Sequential)]
    public struct col32_t
    {
        private float r;
        private float g;
        private float b;
        private float a;

        public col32_t(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1;
        }
        public col32_t(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public col32_t(byte r, byte g, byte b)
        {
            //r
            if (r > 255)
                this.r = 1f;
            else if (r < 0)
                this.r = 0;
            else
                this.r = 1f / 255 * g;

            //g
            if (g > 255)
                this.g = 1f;
            else if (g < 0)
                this.g = 0;
            else
                this.g = 1f / 255 * g;

            //b
            if (b > 255)
                this.b = 1f;
            else if (b < 0)
                this.b = 0;
            else
                this.b = 1f / 255 * b;

            //a
            a = 1f;
        }

        public col32_t(byte r, byte g, byte b, byte a)
        {
            //r
            if (r > 255)
                this.r = 1f;
            else if (r < 0)
                this.r = 0;
            else
                this.r = 1f / 255 * r;

            //g
            if (g > 255)
                this.g = 1f;
            else if (g < 0)
                this.g = 0;
            else
                this.g = 1f / 255 * g;

            //b
            if (b > 255)
                this.b = 1f;
            else if (b < 0)
                this.b = 0;
            else
                this.b = 1f / 255 * b;

            //a
            if (a > 255)
                this.a = 1f;
            else if (a < 0)
                this.a = 0;
            else
                this.a = 1f / 255 * a;
        }

        public float R
        {
            get
            {
                return r;
            }
            set
            {
                if (value < 0)
                    r = 0;
                else if (value > 1)
                    r = 1;

                r = value;
            }
        }


        public float G
        {
            get
            {
                return g;
            }
            set
            {
                if (value < 0)
                    g = 0;
                else if (value > 1)
                    g = 1;

                g = value;
            }
        }

        public float B
        {
            get
            {
                return b;
            }
            set
            {
                if (value < 0)
                    b = 0;
                else if (value > 1)
                    b = 1;

                b = value;
            }
        }

        public float A
        {
            get
            {
                return a;
            }
            set
            {
                if (value < 0)
                    a = 0;
                else if (value > 1)
                    a = 1;

                a = value;
            }
        }

        public byte RByte
        {
            get
            {
                return (byte)(255 * R);
            }
            set
            {
                if (value > 255)
                    R = 1f;
                else if (value < 0)
                    R = 0;
                else
                    R = 1f / 255 * value;
            }
        }
        public byte GByte
        {
            get
            {
                return (byte)(255 * G);
            }
            set
            {
                if (value > 255)
                    G = 1f;
                else if(value < 0)
                    G = 0;
                else
                    G = 1f / 255 * value;
            }
        }
        public byte BByte
        {
            get
            {
                return (byte)(255 * B);
            }
            set
            {
                if (value > 255)
                    B = 1f;
                else if (value < 0)
                    B = 0;
                else
                    B = 1f / 255 * value;
            }
        }
        public byte AByte
        {
            get
            {
                return (byte)(255 * A);
            }
            set
            {
                if (value > 255)
                    A = 1f;
                else if (value < 0)
                    A = 0;
                else
                    A = 1f / 255 * value;
            }
        }
    }
}
