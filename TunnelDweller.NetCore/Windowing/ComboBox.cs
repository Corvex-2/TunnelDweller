using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Windowing
{
    public class ComboBox : Control
    {
        public string Text { get; set; }
        public string[] Values { get; set; }
        public Font Font { get; set; }
        public int SelectedIndex
        {
            get
            {
                return _selectedindex;
            }
            set
            {
                _selectedindex = value;
            }
        }
        public bool Visible { get; set; } = true;

        public ComboBox(string Text, string[] Values)
        {
            this.Text = Text;
            this.Values = Values;
        }
        public ComboBox(string Text, string[] Values, Font Font)
        {
            this.Text= Text;
            this.Values = Values;
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

            ImGui.ComboBox((Text.Contains("###") ? Text : Text + StackID), FormatListBoxString(Values), ref _selectedindex);

            if(Font != null)
                Font.PopFont();
        }

        public override bool Visibility()
        {
            return Visible;
        }

        private int _selectedindex;
        private static string FormatListBoxString(string[] values)
        {
            StringBuilder strb = new StringBuilder();
            foreach (var value in values)
            {
                strb.Append(value + '\0');
            }
            return strb.ToString();
        }
    }
}
