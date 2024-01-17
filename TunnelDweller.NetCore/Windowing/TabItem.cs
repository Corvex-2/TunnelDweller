using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Randomness;

namespace TunnelDweller.NetCore.Windowing
{
    public class TabItem
    {
        public string Name { get; set; }
        public ImGuiTabItemFlags Flags { get; set; }
        public List<Control> Controls { get; private set; } = new List<Control>();

        public TabItem(string Name)
        {
            this.StackID = "###" + SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
            this.Name = Name;
            this.Flags = ImGuiTabItemFlags.ImGuiTabItemFlags_None;
        }
        public TabItem(string Name, ImGuiTabItemFlags Flags)
        {
            this.StackID = "###" + SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
            this.Name = Name;
            this.Flags = Flags;
        }

        public void Paint()
        {
            if (ImGui.BeginTabItem((Name.Contains("###") ? Name : Name + StackID), Flags))
            {
                for (int i = 0; i < Controls.Count; i++)
                {
                    Controls[i].Paint();
                }
                ImGui.EndTabItem();
            }
        }

        private string StackID { get; set; }
    }
}
