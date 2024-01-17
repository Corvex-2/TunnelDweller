using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Randomness;
using static System.Net.Mime.MediaTypeNames;

namespace TunnelDweller.NetCore.Windowing
{
    public class Popup
    {
        internal static List<Popup> Popups = new List<Popup>();

        public string Title { get; set; }
        public ImGuiWindowFlags Flags { get; set; }
        public bool Active { get; set; }
        public Font Font { get; set; }
        public List<Control> Controls { get; private set; } = new List<Control>();

        public Popup(string Title, bool DisableAutoAdd = false) 
        {
            this.StackID = "###" + SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
            this.Title = Title;
            this.Flags = ImGuiWindowFlags.ImGuiWindowFlags_NoMove | ImGuiWindowFlags.ImGuiWindowFlags_NoResize | ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize;
            if (!DisableAutoAdd)
                Popups.Add(this);
            // TODO : Input Callbacks
        }
        public Popup(string Title, ImGuiWindowFlags Flags, bool DisableAutoAdd = false)
        {
            this.StackID = "###" + SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
            this.Title = Title;
            this.Flags = ImGuiWindowFlags.ImGuiWindowFlags_NoMove | ImGuiWindowFlags.ImGuiWindowFlags_NoResize | ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize;
            if (!DisableAutoAdd)
                Popups.Add(this);
            // TODO : Input Callbacks
        }
        public Popup(string Title, ImGuiWindowFlags Flags, Font Font, bool DisableAutoAdd = false)
        {
            this.StackID = "###" + SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
            this.Title = Title;
            this.Flags = Flags;
            this.Font = Font;
            if (!DisableAutoAdd)
                Popups.Add(this);
            // TODO : Input Callbacks
        }

        public void Paint()
        {
            if(Active)
            {

                if (Font != null)
                    Font.PushFont();
                ImGui.OpenPopup(Title);

                if(ImGui.BeginPopupModal((Title.Contains("###") ? Title : Title + StackID), Flags))
                {
                    for (int i = 0; i < Controls.Count; i++)
                    {
                        if (Controls[i].Visibility())
                            Controls[i].Paint();
                    }

                    //var size = ImGui.GetWindowSize();

                    //ImGui.SetWindowPos(Variables.Width / 2 - size.x / 2, Variables.Height / 2 - size.y / 2);

                    ImGui.EndPopupModal();
                }

                if(Font != null)
                    Font.PopFont();
            }
        }

        public static void DrawAll()
        {
            for(int i = 0; i < Popups.Count; i++)
            {
                Popups[i].Paint();
            }
        }

        private string StackID;
    }
}
