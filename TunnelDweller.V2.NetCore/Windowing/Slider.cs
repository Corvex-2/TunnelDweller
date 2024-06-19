using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Windowing
{
    public class Slider : Control
    {
        public string Text { get; set; }
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value < MinValue)
                    _value = MinValue;
                else if (value > MaxValue)
                    _value = MaxValue;
                else
                    _value = value;
            }
        }
        public float MaxValue { get; set; }
        public float MinValue { get; set; }
        public int Decimals { get; set; }
        public ImGuiSliderFlags Flags { get; set; }
        public Font Font { get; set; }
        public bool Visible { get; set; } = true;

        public Slider(string Text, float Value, float MinValue, float MaxValue, int Decimal = 2, ImGuiSliderFlags Flags = ImGuiSliderFlags.AlwaysClamp | ImGuiSliderFlags.NoInput)
        {
            this.Text = Text;
            this.MaxValue = MaxValue;
            this.MinValue = MinValue;
            this.Value = Value;
            this.Decimals = Decimal;
            this.Flags = Flags;
        }
        public Slider(string Text, float Value, float MinValue, float MaxValue, Font Font, int Decimal = 2, ImGuiSliderFlags Flags = ImGuiSliderFlags.AlwaysClamp | ImGuiSliderFlags.NoInput)
        {
            this.Text = Text;
            this.MaxValue = MaxValue;
            this.MinValue = MinValue;
            this.Value = Value;
            this.Font = Font;
            this.Decimals = Decimal;
            this.Flags = Flags;
        }

        public override void Paint()
        {
            if (!Visible)
                return;

            if (Font != null)
                Font.PushFont();

            if (Sameline)
                ImGui.SameLine(0, -1);
            ImGui.SliderFloat(ref _value, MinValue, MaxValue, (Text.Contains("###") ? Text : Text + StackID), (int)Flags, $"%.{Decimals}f");
            Value = _value;

            if (Font != null)
                Font.PopFont();
        }

        public override bool Visibility()
        {
            return Visible;
        }

        private float _value;
    }
}
