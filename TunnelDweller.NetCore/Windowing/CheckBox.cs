using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Windowing
{
    public class CheckBox : Control
    {
        public string Text { get; set; }
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
            }
        }
        public Font Font { get; set; }
        public bool Visible { get; set; } = true;

        public CheckBox(string Text, bool State)
        {
            this.Text = Text;
            this._checked = State;
        }


        public override void Paint()
        {
            if (!Visible)
                return;

            if (Font != null)
                Font.PushFont();

            if (Sameline)
                ImGui.SameLine(0, -1);

            ImGui.CheckBox(Text, ref _checked);

            if(Font != null)
                Font.PopFont();
        }

        public override bool Visibility()
        {
            return Visible;
        }

        private bool _checked;
    }
}
