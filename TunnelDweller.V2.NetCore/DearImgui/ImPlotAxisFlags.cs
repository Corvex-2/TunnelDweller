using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    public enum ImPlotAxisFlags : int
    {
        ImPlotAxisFlags_None = 0,       // default
        ImPlotAxisFlags_NoLabel = 1 << 0,  // the axis label will not be displayed (axis labels are also hidden if the supplied string name is nullptr)
        ImPlotAxisFlags_NoGridLines = 1 << 1,  // no grid lines will be displayed
        ImPlotAxisFlags_NoTickMarks = 1 << 2,  // no tick marks will be displayed
        ImPlotAxisFlags_NoTickLabels = 1 << 3,  // no text labels will be displayed
        ImPlotAxisFlags_NoInitialFit = 1 << 4,  // axis will not be initially fit to data extents on the first rendered frame
        ImPlotAxisFlags_NoMenus = 1 << 5,  // the user will not be able to open context menus with right-click
        ImPlotAxisFlags_NoSideSwitch = 1 << 6,  // the user will not be able to switch the axis side by dragging it
        ImPlotAxisFlags_NoHighlight = 1 << 7,  // the axis will not have its background highlighted when hovered or held
        ImPlotAxisFlags_Opposite = 1 << 8,  // axis ticks and labels will be rendered on the conventionally opposite side (i.e, right or top)
        ImPlotAxisFlags_Foreground = 1 << 9,  // grid lines will be displayed in the foreground (i.e. on top of data) instead of the background
        ImPlotAxisFlags_Invert = 1 << 10, // the axis will be inverted
        ImPlotAxisFlags_AutoFit = 1 << 11, // axis will be auto-fitting to data extents
        ImPlotAxisFlags_RangeFit = 1 << 12, // axis will only fit points if the point is in the visible range of the **orthogonal** axis
        ImPlotAxisFlags_PanStretch = 1 << 13, // panning in a locked or constrained state will cause the axis to stretch if possible
        ImPlotAxisFlags_LockMin = 1 << 14, // the axis minimum value will be locked when panning/zooming
        ImPlotAxisFlags_LockMax = 1 << 15, // the axis maximum value will be locked when panning/zooming
        ImPlotAxisFlags_Lock = ImPlotAxisFlags_LockMin | ImPlotAxisFlags_LockMax,
        ImPlotAxisFlags_NoDecorations = ImPlotAxisFlags_NoLabel | ImPlotAxisFlags_NoGridLines | ImPlotAxisFlags_NoTickMarks | ImPlotAxisFlags_NoTickLabels,
        ImPlotAxisFlags_AuxDefault = ImPlotAxisFlags_NoGridLines | ImPlotAxisFlags_Opposite
    }
}
