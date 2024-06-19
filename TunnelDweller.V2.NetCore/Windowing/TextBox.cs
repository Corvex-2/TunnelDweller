using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Windowing
{
    public class TextBox : Control, IDisposable
    {
        public ImGuiInputTextFlags Flags { get; set; }

        public bool Visible { get; set; } = true;

        public string Text
        {
            get
            {
                if (p_Buffer == IntPtr.Zero)
                    return "";
                return Marshal.PtrToStringAnsi(p_Buffer);
            }
            set
            {
                if (p_Buffer == IntPtr.Zero)
                {
                    bufferSize = 260;
                    p_Buffer = Marshal.AllocHGlobal(260);
                }

                if (value.Length == 0)
                {
                    Marshal.Copy(ZERO_BUFFER, 0, p_Buffer, bufferSize);
                }

                var valdata = Encoding.ASCII.GetBytes(value);
                if (valdata.Length > 0 && valdata.Length < bufferSize)
                {
                    Marshal.Copy(ZERO_BUFFER, 0, p_Buffer, bufferSize);
                    Marshal.Copy(valdata, 0, p_Buffer, valdata.Length);
                }
            }
        }
        public string Caption;
        public Font Font;

        public float Width;

        public TextBox(string Caption, string Text)
        {
            this.Caption = Caption;
            this.Text = Text;
        }
        public TextBox(string Caption, string Text, ImGuiInputTextFlags Flags = ImGuiInputTextFlags.ImGuiInputTextFlags_None)
        {
            this.Caption = Caption;
            this.Text = Text;
            this.Flags = Flags;
        }
        public TextBox(string Caption, string Text, Font Font, ImGuiInputTextFlags Flags = ImGuiInputTextFlags.ImGuiInputTextFlags_None)
        {
            this.Caption = Caption;
            this.Text = Text;
            this.Font = Font;
        }

        public override bool Visibility()
        {
            return Visible;
        }

        public override void Paint()
        {
            if (!Visible)
                return;

            if (Font != null)
                Font.PushFont();

            if (Sameline)
                ImGui.SameLine(0, -1);
            ImGui.TextBox((Caption.Contains("###") ? Caption : Caption + StackID), p_Buffer, bufferSize, (int)Flags);

            if (Font != null)
                Font.PopFont();
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(p_Buffer);
        }

        private IntPtr p_Buffer;
        private int bufferSize;
        private static byte[] ZERO_BUFFER = new byte[260];
    }
}
