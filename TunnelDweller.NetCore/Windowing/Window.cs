using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Input;
using TunnelDweller.NetCore.Randomness;

namespace TunnelDweller.NetCore.Windowing
{
    public class Window
    {
        internal static List<Window> Windows = new List<Window>();

        public static TabBar MainTabs;
        public static Window MainWindow;


        public string Title { get; set; }
        public ImGuiWindowFlags Flags { get; set; }
        public bool Show { get; set; }
        public bool ForceUnshow { get; set; }
        public List<Control> Controls { get; private set; } = new List<Control>();

        public vec2_t Size { get; set; } = new vec2_t() { x = -1, y = -1 };
        public vec2_t MinSize { get; set; } = new vec2_t() { x = -1, y = -1 };
        public vec2_t MaxSize { get; set; } = new vec2_t() { x = -1, y = -1 };
        public vec2_t Position { get; set; } = new vec2_t() { x = -1, y = -1 };

        public Font Font { get; set; }

        public Window(string Title, bool DisableAutoAdd = false)
        {
            this.StackID = "###" + SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
            this.Title = Title;
            this.Flags = ImGuiWindowFlags.ImGuiWindowFlags_None;
            if(!DisableAutoAdd)
                Windows.Add(this);
            // TODO: Input Callbacks
        }
        public Window(string Title, ImGuiWindowFlags Flags, bool DisableAutoAdd = false)
        {
            this.StackID = "###" + SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
            this.Title = Title;
            this.Flags = Flags;
            if(!DisableAutoAdd)
                Windows.Add(this);
            // TODO: Input Callbacks
        }
        public Window(string Title, ImGuiWindowFlags Flags, Font Font, bool DisableAutoAdd = false)
        {
            this.StackID = "###" + SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
            this.Title = Title;
            this.Flags = Flags;
            this.Font = Font;
            if (!DisableAutoAdd)
                Windows.Add(this);
            // TODO: Input Callbacks
        }

        internal static void Initialize()
        {
            InputManager.InputChanged += InputManager_InputChanged;

            MainTabs = new TabBar();
            MainWindow = new Window("Tunnel Dweller", ImGuiWindowFlags.ImGuiWindowFlags_NoResize | ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize | ImGuiWindowFlags.ImGuiWindowFlags_NoCollapse, Windowing.Font.Default());
            MainWindow.MinSize = new vec2_t(400, 400);
            MainWindow.MaxSize = new vec2_t(9999, Variables.Height * 0.75f);
            MainWindow.Controls.Add(MainTabs);
        }

        private static void InputManager_InputChanged(object sender, InputEventArgs e)
        {
            if (e.skCode == 0xd3 && e.State)
                MainWindow.Show = !MainWindow.Show;
        }

        public void Paint()
        {
            if(Show && !ForceUnshow)
            {
                if (Font != null)
                    Font.PushFont();

                if(MinSize != vec2_t.MinusOne && MaxSize != vec2_t.MinusOne)
                    ImGui.SetNextWindowSizeConstraints(MinSize.x, MinSize.y, MaxSize.x, MaxSize.y);
                if(Size.x > 0 && Size.y > 0)
                    ImGui.SetNextWindowSize(Size.x, Size.y);

                if (ImGui.Begin((Title.Contains("###") ? Title : Title + StackID), Flags))
                {

                    for(int i = 0; i < Controls.Count; i++)
                    {
                        if (Controls[i].Visibility())
                        {
                            Controls[i].Paint();
                        }
                    }

                    if (Position != vec2_t.MinusOne)
                        ImGui.SetWindowPos(Position.x, Position.y);

                    ImGui.End();
                }

                if(Font != null)
                    Font.PopFont();
            }
        }

        public static void DrawAll()
        {
            for(int i = 0; i < Windows.Count; i++)
            {
                Windows[i].Paint();
            }
        }

        private string StackID { get; set; }
    }
}
