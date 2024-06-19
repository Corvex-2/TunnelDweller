using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    public enum ImPlotCol
    {
        // item styling colors
        ImPlotCol_Line,          // plot line/outline color (defaults to next unused color in current colormap)
        ImPlotCol_Fill,          // plot fill color for bars (defaults to the current line color)
        ImPlotCol_MarkerOutline, // marker outline color (defaults to the current line color)
        ImPlotCol_MarkerFill,    // marker fill color (defaults to the current line color)
        ImPlotCol_ErrorBar,      // error bar color (defaults to ImGuiCol_Text)
                                 // plot styling colors
        ImPlotCol_FrameBg,       // plot frame background color (defaults to ImGuiCol_FrameBg)
        ImPlotCol_PlotBg,        // plot area background color (defaults to ImGuiCol_WindowBg)
        ImPlotCol_PlotBorder,    // plot area border color (defaults to ImGuiCol_Border)
        ImPlotCol_LegendBg,      // legend background color (defaults to ImGuiCol_PopupBg)
        ImPlotCol_LegendBorder,  // legend border color (defaults to ImPlotCol_PlotBorder)
        ImPlotCol_LegendText,    // legend text color (defaults to ImPlotCol_InlayText)
        ImPlotCol_TitleText,     // plot title text color (defaults to ImGuiCol_Text)
        ImPlotCol_InlayText,     // color of text appearing inside of plots (defaults to ImGuiCol_Text)
        ImPlotCol_AxisText,      // axis label and tick lables color (defaults to ImGuiCol_Text)
        ImPlotCol_AxisGrid,      // axis grid color (defaults to 25% ImPlotCol_AxisText)
        ImPlotCol_AxisTick,      // axis tick color (defaults to AxisGrid)
        ImPlotCol_AxisBg,        // background color of axis hover region (defaults to transparent)
        ImPlotCol_AxisBgHovered, // axis hover color (defaults to ImGuiCol_ButtonHovered)
        ImPlotCol_AxisBgActive,  // axis active color (defaults to ImGuiCol_ButtonActive)
        ImPlotCol_Selection,     // box-selection color (defaults to yellow)
        ImPlotCol_Crosshairs,    // crosshairs color (defaults to ImPlotCol_PlotBorder)
    }
}
