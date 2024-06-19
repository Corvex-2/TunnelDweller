using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Windowing
{
    public class Label : Control
    {
        public string Text { get; set; }
        public Font Font { get; set; }
        public bool Visible { get; set; } = true;
        public col32_t Color { get; set; } = new col32_t(255, 255, 255, 255);
        public vec2_t Position { get; set; } = vec2_t.MinusOne;

        public Label(string Text)
        {
            this.Text = Text;
        }
        public Label(string Text, Font Font)
        {
            this.Text = Text;
            this.Font = Font;
        }

        public override void Paint()
        {
            if(!Visible) return;

            if (Font != null)
                Font.PushFont();

            if (Sameline)
                ImGui.SameLine(0, -1);

            if(Position != vec2_t.MinusOne)
                ImGui.SetCursorPosition(Position.x, Position.y);

            ImGui.PushColorVar(ImGuiCol.ImGuiCol_Text, Color.RByte, Color.GByte, Color.BByte, Color.AByte);

            ImGui.Label(Text);

            ImGui.PopColorVar();

            if(Font != null)
                Font.PopFont();
        }

        public override bool Visibility()
        {
            return Visible;
        }
    }
}
