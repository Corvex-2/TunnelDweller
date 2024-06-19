using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Randomness;
using TunnelDweller.NetCore.Rendering;
using TunnelDweller.NetCore.Threading;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.Warp
{
    internal static class WarpManager
    {
        internal static bool Initialized { get; private set; }

        internal static void Initialize()
        {
            Update.OnUpdate += Update_OnUpdate;

            tiWarpTab.Controls.Add(lbWarpTabInfo);
            tiWarpTab.Controls.Add(btnAddWarpPoint);
            tiWarpTab.Controls.Add(new Seperator());

            puCreatePoint.Controls.Add(lbCreatePointInfo);
            puCreatePoint.Controls.Add(tbCreatePointName);
            puCreatePoint.Controls.Add(btnCreatePointConfirm);
            puCreatePoint.Controls.Add(btnCreatePointCancel);

            Window.MainTabs.Items.Add(tiWarpTab);
        }

        private static void Update_OnUpdate(object sender, EventArgs e)
        {
            Cleanup();
            DeletePending();
            lastLevel = Variables.LevelId;
        }

        private static void Cleanup()
        {
            if (lastLevel != Variables.LevelId) //Cleanup!
            {
                for (int i = 0; i < registeredWarpPoints.Count; i++)
                {
                    tiWarpTab.Controls.Remove(registeredWarpPoints[i]);
                }
                registeredWarpPoints.Clear();
                markedForDeletion.Clear();
            }
        }

        private static void DeletePending()
        {
            if (registeredWarpPoints.Count > 0 && markedForDeletion.Count > 0)
            {
                for (int i = 0; i < markedForDeletion.Count; i++)
                {
                    tiWarpTab.Controls.Remove(markedForDeletion[i]);
                    registeredWarpPoints.Remove(markedForDeletion[i]);
                }
                markedForDeletion.Clear();
            }
        }

        public static bool Delete(WarpPointView View)
        {
            markedForDeletion.Add(View);
            return markedForDeletion.Contains(View);
        }
        public static bool Add(WarpPointView View)
        {
            registeredWarpPoints.Add(View);
            tiWarpTab.Controls.Add(View);
            return tiWarpTab.Controls.Contains(View);
        }

        public static void AddWarpPoint_Click()
        {
            if (!Variables.GetIsLevelValidLevel(Variables.LevelId))
                return;

            tbCreatePointName.Text = $"warpPoint{registeredWarpPoints.Count}";
            puCreatePoint.Active = true;
        }

        private static void CreatePointConfirm_Click()
        {
            if (registeredWarpPoints.Any(x => x.WarpPoint.Name == tbCreatePointName.Text) && registeredWarpPoints.Count > 0 || tbCreatePointName.Text.Length < 3)
            {
                lbCreatePointInfo.Color = new col32_t(255, 0, 0, 255);
                return;
            }

            puCreatePoint.Active = false;
            ImGui.CloseCurrentPopup();
            var warpPoint = new WarpPoint(tbCreatePointName.Text, Variables.Position, Variables.Angles);
            var view = new WarpPointView(warpPoint);
            Add(view);
            lbCreatePointInfo.Color = new col32_t(255, 255, 255, 255);
        }
        private static void CreatePointCancel_Click()
        {
            puCreatePoint.Active = false;
            ImGui.CloseCurrentPopup();
        }


        private static int lastLevel = -1;

        private static List<WarpPointView> registeredWarpPoints = new List<WarpPointView>();
        private static List<WarpPointView> markedForDeletion = new List<WarpPointView>();

        private static TabItem tiWarpTab = new TabItem("Warp###warpManagerTab", ImGuiTabItemFlags.ImGuiTabItemFlags_Leading);
        private static Button btnAddWarpPoint = new Button("Create New Point###warpManagerAddBtn", AddWarpPoint_Click);
        private static Label lbWarpTabInfo = new Label("Manage Warp Points");


        private static Popup puCreatePoint = new Popup("New Warp Point###warpManagerCreatePopup");
        private static Label lbCreatePointInfo = new Label("Please enter a Name for your Warp Point.\r\nThe name must be at least 3 characters long and can't be a duplicate!");
        private static TextBox tbCreatePointName = new TextBox("Point Name###warpManagerCreatePointName", "warpPoint");
        private static Button btnCreatePointConfirm = new Button("CONFIRM###warpManagerCreateConfirm", CreatePointConfirm_Click);
        private static Button btnCreatePointCancel = new Button("CANCEL###warpManagerCreateCancel", CreatePointCancel_Click) { Sameline = true, };
    }
}
