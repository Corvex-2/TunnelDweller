using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    public enum ImGuiTabBarFlags
    {
        ImGuiTabBarFlags_None = 0,
        ImGuiTabBarFlags_Reorderable = 1 << 0,   // Allow manually dragging tabs to re-order them + New tabs are appended at the end of list
        ImGuiTabBarFlags_AutoSelectNewTabs = 1 << 1,   // Automatically select new tabs when they appear
        ImGuiTabBarFlags_TabListPopupButton = 1 << 2,   // Disable buttons to open the tab list popup
        ImGuiTabBarFlags_NoCloseWithMiddleMouseButton = 1 << 3,   // Disable behavior of closing tabs (that are submitted with p_open != NULL) with middle mouse button. You can still repro this behavior on user's side with if (IsItemHovered() && IsMouseClicked(2)) *p_open = false.
        ImGuiTabBarFlags_NoTabListScrollingButtons = 1 << 4,   // Disable scrolling buttons (apply when fitting policy is ImGuiTabBarFlags_FittingPolicyScroll)
        ImGuiTabBarFlags_NoTooltip = 1 << 5,   // Disable tooltips when hovering a tab
        ImGuiTabBarFlags_FittingPolicyResizeDown = 1 << 6,   // Resize tabs when they don't fit
        ImGuiTabBarFlags_FittingPolicyScroll = 1 << 7,   // Add scroll buttons when tabs don't fit
        ImGuiTabBarFlags_FittingPolicyMask_ = ImGuiTabBarFlags_FittingPolicyResizeDown | ImGuiTabBarFlags_FittingPolicyScroll,
        ImGuiTabBarFlags_FittingPolicyDefault_ = ImGuiTabBarFlags_FittingPolicyResizeDown
    }
}
