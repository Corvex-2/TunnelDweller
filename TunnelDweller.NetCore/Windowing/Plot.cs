﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Windowing
{
    public abstract class Plot : Control
    {
        public ImPlotFlags PlotFlags { get; set; }
        public ImPlotAxisFlags AxisFlags { get; set; }
        public bool Visible { get; set; } = true;
        public string Name { get; set; }

        public string XAxisName { get; set; } = string.Empty;
        public string YAxisName { get; set; } = string.Empty;

        public float MinX { get; set; } = float.MinValue;
        public float MinY { get; set; } = float.MinValue;
        public float MaxX { get; set; } = float.MaxValue;
        public float MaxY { get; set; } = float.MaxValue;

        public bool UseColours { get; set; } = true;

        public col32_t BackgroundColor { get; set; }
        public col32_t ForegroundColor { get; set; }
        public col32_t BorderColor { get; set; }

        public vec2_t Position { get; set; } = vec2_t.MinusOne;
        public vec2_t Size { get; set; } = new vec2_t(-1, 0);

        public Plot(string Name)
        {
            this.Name = Name;
        }

        public override bool Visibility()
        {
            return Visible;
        }

        public virtual bool Start()
        {
            if (UseColours)
            {
                ImGui.PushPlotStyleColor(ImPlotCol.ImPlotCol_PlotBorder, BorderColor);
                ImGui.PushPlotStyleColor(ImPlotCol.ImPlotCol_PlotBg, BackgroundColor);
                ImGui.PushPlotStyleColor(ImPlotCol.ImPlotCol_Fill, ForegroundColor);
            }

            if (Position != vec2_t.MinusOne)
                ImGui.SetCursorPosition(Position.x, Position.y);

            var success = ImGui.BeginPlot(Name, (int)PlotFlags, Size);

            if (success)
            {
                ImGui.SetupAxes(XAxisName, YAxisName, (int)AxisFlags);
                ImGui.SetupAxesLimits(MinX, MaxX, MinY, MaxY);
            }

            return success;
        }

        public virtual void End()
        {
            ImGui.EndPlot();

            if (UseColours)
            {
                ImGui.PopPlotStyleColor();
                ImGui.PopPlotStyleColor();
                ImGui.PopPlotStyleColor();
            }
        }
    }
}
