using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Input;

namespace TunnelDweller.NetCore.Windowing
{
    public class Hotkey : Control
    {
        public bool Visible { get; set; } = true;

        public string Caption { get; set; }
        public int Key { get; set; }
        public bool Capturing { get; private set; } = false;

        public Hotkey(int diKeyId)
        {
            Key = diKeyId;
            Caption = "";
            InputManager.InputChanged += InputManager_InputChanged;
        }
        public Hotkey(int diKeyId, string caption)
        {
            Key= diKeyId;
            Caption = caption;
            InputManager.InputChanged += InputManager_InputChanged;
        }

        public void InputManager_InputChanged(object sender, InputEventArgs e)
        {
            if(Capturing && e.State)
            {
                if(e.skCode == 0x01)
                {
                    Capturing = false;
                    return;
                }
                Key = e.skCode;
                Capturing = false;
            }
        }

        public override void Paint()
        {
            if(Caption != null && Caption.Length > 0)
                ImGui.Label(Caption);

            if (Capturing)
            {
                ImGui.PushColorVar(ImGuiCol.ImGuiCol_Button, 255, 0, 0, 255);
                ImGui.PushColorVar(ImGuiCol.ImGuiCol_ButtonActive, 255, 0, 0, 255);
                ImGui.PushColorVar(ImGuiCol.ImGuiCol_ButtonHovered, 255, 0, 0, 255);

                if (ImGui.Button($"...###{StackID}"))
                    Capturing = false;

                ImGui.PopColorVar();
                ImGui.PopColorVar();
                ImGui.PopColorVar();
            }
            else
            {
                ImGui.PushColorVar(ImGuiCol.ImGuiCol_Button, 144, 0, 0, 255);
                ImGui.PushColorVar(ImGuiCol.ImGuiCol_ButtonActive, 144, 0, 0, 255);
                ImGui.PushColorVar(ImGuiCol.ImGuiCol_ButtonHovered, 144, 0, 0, 255);

                if (ImGui.Button($"{Key}###{StackID}"))
                    Capturing = true;

                ImGui.PopColorVar();
                ImGui.PopColorVar();
                ImGui.PopColorVar();
            }
        }

        public override bool Visibility()
        {
            return Visible;
        }
    }
}
