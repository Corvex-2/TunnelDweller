using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.DearImgui
{
    public enum ImGuiTabItemFlags
    {
        ImGuiTabItemFlags_None = 0,
        ImGuiTabItemFlags_UnsavedDocument = 1 << 0,   // Display a dot next to the title + tab is selected when clicking the X + closure is not assumed (will wait for user to stop submitting the tab). Otherwise closure is assumed when pressing the X, so if you keep submitting the tab may reappear at end of tab bar.
        ImGuiTabItemFlags_SetSelected = 1 << 1,   // Trigger flag to programmatically make the tab selected when calling BeginTabItem()
        ImGuiTabItemFlags_NoCloseWithMiddleMouseButton = 1 << 2,   // Disable behavior of closing tabs (that are submitted with p_open != NULL) with middle mouse button. You can still repro this behavior on user's side with if (IsItemHovered() && IsMouseClicked(2)) *p_open = false.
        ImGuiTabItemFlags_NoPushId = 1 << 3,   // Don't call PushID(tab->ID)/PopID() on BeginTabItem()/EndTabItem()
        ImGuiTabItemFlags_NoTooltip = 1 << 4,   // Disable tooltip for the given tab
        ImGuiTabItemFlags_NoReorder = 1 << 5,   // Disable reordering this tab or having another tab cross over this tab
        ImGuiTabItemFlags_Leading = 1 << 6,   // Enforce the tab position to the left of the tab bar (after the tab list popup button)
        ImGuiTabItemFlags_Trailing = 1 << 7    // Enforce the tab position to the right of the tab bar (before the scrolling buttons)
    }
}
