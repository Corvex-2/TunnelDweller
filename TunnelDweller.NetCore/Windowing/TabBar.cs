using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Randomness;

namespace TunnelDweller.NetCore.Windowing
{
    public class TabBar : Control
    {
        private string idx;

        public ImGuiTabBarFlags Flags { get; set; }
        public List<TabItem> Items { get; private set; } = new List<TabItem>();
        public bool Visible { get; set; } = true;

        public TabBar()
        {
            idx = SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
            this.Flags = ImGuiTabBarFlags.ImGuiTabBarFlags_None;
        }
        public TabBar(ImGuiTabBarFlags Flags)
        {
            idx = SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
            this.Flags = Flags;
        }

        public override void Paint()
        {
            if (ImGui.BeginTabBar(idx, Flags))
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Paint();
                }
                ImGui.EndTabBar();
            }
        }

        public override bool Visibility()
        {
            return Visible;
        }
    }
}
