using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using static System.Net.Mime.MediaTypeNames;

namespace TunnelDweller.NetCore.Windowing
{
    public class ColorPicker : Control
    {
        public string Name { get; set; }
        public Font Font { get; set; }
        public col32_t Color
        {
            get
            {
                return refcol;
            }
            set
            {
                refcol = value;
            }
        }



        public bool Visible { get; set; } = true;

        public ColorPicker(string Name)
        {
            this.Name = Name;
            this.refcol = new col32_t(255, 255, 255, 255);
        }
        public ColorPicker(string Name, byte R, byte G, byte B, byte A)
        {
            this.Name = Name;
            this.refcol = new col32_t(R,G,B,A);
        }
        public ColorPicker(string Name, byte R, byte G, byte B, byte A, Font Font)
        {
            this.Name = Name;
            this.refcol = new col32_t(R, G, B, A);
            this.Font = Font;
        }

        public override void Paint()
        {
            if (!Visible)
                return;
            if (Sameline)
                ImGui.SameLine(0, -1);

            if (Font != null)
                Font.PushFont();

            ImGui.ColorPicker((Name.Contains("###") ? Name : Name + StackID), ref refcol);

            if(Font != null)
                Font.PopFont();
        }

        public override bool Visibility()
        {
            return Visible;
        }

        private col32_t refcol;
    }
}
