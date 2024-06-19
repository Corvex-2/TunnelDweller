using System;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.Warp
{
    internal class WarpPointView : Control
    {
        public WarpPoint WarpPoint;

        private Label warpInfo;
        private Button warpGotoPoint;
        private Button warpRemovePoint;
        private Seperator warpSeperator = new Seperator();

        private Popup removePointPopup;
        private Label removePointPopupLabel;
        private Button removePointPopupConfirm;
        private Button removePointPopupCancel;

        public WarpPointView(WarpPoint point)
        {
            warpInfo = new Label($"{point.Name}\r\n Position: {point.Position}");
            warpGotoPoint = new Button($"Jump to Point###gotoButton{point.Name}", gotoPoint_ButtonClick);
            warpRemovePoint = new Button($"Remove Point###removeButton{point.Name}", removePoint_ButtonClick) { Sameline = true };

            removePointPopup = new Popup($"Delete Warp Point?###removePopupWindowFor{point.Name}");
            removePointPopupLabel = new Label($"The following Warp Point will be permanently deleted from your collection!\r\nThere is no going back! Are you sure you wan't to delete it?");
            removePointPopupConfirm = new Button($"CONFIRM###confirmDeletionButton{point.Name}", removePointPopupConfirm_Click);
            removePointPopupCancel = new Button($"CANCEL###cancelDeletionButton{point.Name}", removePointPopupCancel_Click) { Sameline = true };
            removePointPopup.Controls.Add(removePointPopupLabel);
            removePointPopup.Controls.Add(removePointPopupConfirm);
            removePointPopup.Controls.Add(removePointPopupCancel);

            WarpPoint = point;
        }

        public override void Paint()
        {
            if (WarpPoint == null)
                return;

            warpInfo.Paint();
            warpGotoPoint.Paint();
            warpRemovePoint.Paint();
            warpSeperator.Paint();
        }

        private void gotoPoint_ButtonClick()
        {
            WarpPoint.Goto();
        }

        private void removePoint_ButtonClick()
        {
            removePointPopup.Active = true;
        }

        private void removePointPopupConfirm_Click()
        {
            ImGui.CloseCurrentPopup();
            removePointPopup.Active = false;
            WarpManager.Delete(this);
        }

        private void removePointPopupCancel_Click()
        {
            ImGui.CloseCurrentPopup();
            removePointPopup.Active = false;
        }

        public override bool Visibility()
        {
            return true;
        }


    }
}
