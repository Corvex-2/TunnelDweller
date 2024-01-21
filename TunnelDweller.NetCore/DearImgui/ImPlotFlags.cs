using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    public enum ImPlotFlags : int
    {
        ImPlotFlags_None = 0,       // default
        ImPlotFlags_NoTitle = 1 << 0,  // the plot title will not be displayed (titles are also hidden if preceeded by double hashes, e.g. "##MyPlot")
        ImPlotFlags_NoLegend = 1 << 1,  // the legend will not be displayed
        ImPlotFlags_NoMouseText = 1 << 2,  // the mouse position, in plot coordinates, will not be displayed inside of the plot
        ImPlotFlags_NoInputs = 1 << 3,  // the user will not be able to interact with the plot
        ImPlotFlags_NoMenus = 1 << 4,  // the user will not be able to open context menus
        ImPlotFlags_NoBoxSelect = 1 << 5,  // the user will not be able to box-select
        ImPlotFlags_NoFrame = 1 << 6,  // the ImGui frame will not be rendered
        ImPlotFlags_Equal = 1 << 7,  // x and y axes pairs will be constrained to have the same units/pixel
        ImPlotFlags_Crosshairs = 1 << 8,  // the default mouse cursor will be replaced with a crosshair when hovered
        ImPlotFlags_CanvasOnly = ImPlotFlags_NoTitle | ImPlotFlags_NoLegend | ImPlotFlags_NoMenus | ImPlotFlags_NoBoxSelect | ImPlotFlags_NoMouseText
    }
}
