using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Windowing
{
    public class Button : Control
    {
        public string Text { get; set; }
        public Action Callback { get; set; }
        public Font Font { get; set; }
        public bool Visible { get; set; } = true;

        public Button(string Text, Action Callback)
        {
            this.Text = Text;
            this.Callback = Callback;
        }
        public Button(string Text, Action Callback, Font Font)
        {
            this.Text= Text;
            this.Callback = Callback;
            this.Font = Font;
        }

        public override void Paint()
        {
            if (!Visible)
                return;

            if (Font != null)
                Font.PushFont();

            if (Sameline)
                ImGui.SameLine(0, -1);

            if (ImGui.Button((Text.Contains("###") ? Text : Text + StackID)) && Callback != null)
                Callback();

            if(Font != null)
                Font.PopFont();
        }

        public override bool Visibility()
        {
            return Visible; 
        }
    }
}
