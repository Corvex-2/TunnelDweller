using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Moduling;
using TunnelDweller.NetCore.Rendering;
using TunnelDweller.NetCore.Threading;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.Overlay
{
    public class Startup : ModuleEntryPoint
    {
        internal TabItem tiOverlayTab = new TabItem("Overlay###overlayTabId", ImGuiTabItemFlags.ImGuiTabItemFlags_Leading);
        internal CheckBox cbGameTime = new CheckBox("Display Game Time###gtDisplayCb", true);
        internal ComboBox cmbGameTimeMode = new ComboBox("Display Game Time Mode###cmbGameTimeMode", new string[] { "Clock", "Tick" });
        internal CheckBox cbPosition = new CheckBox("Display Position###posDisplayCb", true);
        internal CheckBox cbRotation = new CheckBox("Display Rotation###rotDisplayCb", true);
        internal CheckBox cbResolution = new CheckBox("Display Resolution###resDisplayCb", true);
        internal CheckBox cbAspectRatio = new CheckBox("Display AspectRatio###arDisplayCb", false);
        internal CheckBox cbLevelId = new CheckBox("Display Level Id###lidDisplayCb", true);
        internal ComboBox cmbLevelIdMode = new ComboBox("Display Level Id Mode###cmbLevelIdMode", new string[] { "Name", "Number" });
        internal CheckBox cbTargetFps = new CheckBox("Display FPS Limit###fpsDisplayCb", true);
        internal CheckBox cbViewDistance = new CheckBox("Display View Distance###vdDisplayCb", true);
        internal CheckBox cbFieldofView = new CheckBox("Display Field of View###fovDisplayCb", true);
        internal CheckBox cbCrosshair = new CheckBox("Display Crosshair###overlayCrosshairCb", true);
        internal Slider slCrosshairThickness = new Slider("Crosshair Thickness###overlayCrosshairThicknessSl", 1, 0.2f, 10f, 2);
        internal ColorPicker cpCrosshairColor = new ColorPicker("Crosshair Color###overlayCrosshairColorCp");
        internal Slider slCorsshairSize = new Slider("Crosshair Size###overlayCrosshairSizeSl", 1, 0.2f, 10f, 2);
        internal Slider slOverlaySize = new Slider("Overlay Size###overlaySizeSl", 12f, 0f, 64f, 0);
        internal CheckBox cbOverlayEnabled = new CheckBox("Enable Overlay###overlayEnableCp", true);
        internal ColorPicker cpOverlayColor = new ColorPicker("Overlay Color###overlayColorCp");

        public override void Initialize()
        {
            tiOverlayTab.Controls.Add(new Label("Overlay Settings"));
            tiOverlayTab.Controls.Add(new Seperator());
            tiOverlayTab.Controls.Add(cbOverlayEnabled);
            tiOverlayTab.Controls.Add(slOverlaySize);
            tiOverlayTab.Controls.Add(cpOverlayColor);
            tiOverlayTab.Controls.Add(new Label("Game Variables"));
            tiOverlayTab.Controls.Add(new Seperator());
            tiOverlayTab.Controls.Add(cbGameTime);
            tiOverlayTab.Controls.Add(cmbGameTimeMode);
            tiOverlayTab.Controls.Add(cbPosition);
            tiOverlayTab.Controls.Add(cbRotation);
            tiOverlayTab.Controls.Add(cbResolution);
            tiOverlayTab.Controls.Add(cbAspectRatio);
            tiOverlayTab.Controls.Add(cbLevelId);
            tiOverlayTab.Controls.Add(cmbLevelIdMode);
            tiOverlayTab.Controls.Add(cbTargetFps);
            tiOverlayTab.Controls.Add(cbViewDistance);
            tiOverlayTab.Controls.Add(cbFieldofView);
            tiOverlayTab.Controls.Add(new Label("Utilities"));
            tiOverlayTab.Controls.Add(new Seperator());
            tiOverlayTab.Controls.Add(cbCrosshair);
            tiOverlayTab.Controls.Add(slCorsshairSize);
            tiOverlayTab.Controls.Add(slCrosshairThickness);
            tiOverlayTab.Controls.Add(cpCrosshairColor);
            Window.MainTabs.Items.Add(tiOverlayTab);
            Update.OnUpdate += Update_OnUpdate;
            Renderer.OnRender += Renderer_OnRender;
        }

        private void Update_OnUpdate(object sender, EventArgs e)
        {
            if (!cbGameTime.Checked)
                cmbGameTimeMode.Visible = false;
            else
                cmbGameTimeMode.Visible = true;


            if (!cbLevelId.Checked)
                cmbLevelIdMode.Visible = false;
            else
                cmbLevelIdMode.Visible = true;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Tunnel Dweller - Overlay]");

            if (cbGameTime.Checked)
            {
                switch (cmbGameTimeMode.SelectedIndex)
                {
                    default:
                    case 0:
                        sb.AppendLine($"Game Time: {TimeSpan.FromMilliseconds(Variables.GameTime).ToString("hh\\:mm\\:ss\\.fff")}");
                        break;
                    case 1:
                        sb.AppendLine($"Game Time: {Variables.GameTime}");
                        break;
                }
            }

            if (cbPosition.Checked)
                sb.AppendLine($"Player Position: {Variables.Position.ToString()}");
            if (cbRotation.Checked)
                sb.AppendLine($"Player Rotation: {Variables.Angles.ToString()}");
            if (cbResolution.Checked)
                sb.AppendLine($"Resolution: {Variables.Width}x{Variables.Height}");
            if (cbAspectRatio.Checked)
                sb.AppendLine($"Aspect Ratio: {Variables.AspectRatio}");
            if (cbLevelId.Checked)
            {
                switch (cmbLevelIdMode.SelectedIndex)
                {
                    default:
                    case 0:
                        sb.AppendLine($"Level: {Variables.GetLevelName(Variables.LevelId)}");
                        break;
                    case 1:
                        sb.AppendLine($"Level: {Variables.LevelId}");
                        break;
                }
            }

            if (cbTargetFps.Checked)
                sb.AppendLine($"Target FPS: {Variables.Target_FPS}");
            if (cbViewDistance.Checked)
                sb.AppendLine($"View Distance: {Variables.ViewDistance}");
            if (cbFieldofView.Checked)
                sb.AppendLine($"Field of View: {Variables.FieldOfView}");

            _overlayText = sb.ToString();
        }

        private void Renderer_OnRender(object sender, EventArgs e)
        {
            if (cbCrosshair.Checked)
            {
                float x1, x2, y1, y2;

                x1 = Variables.Width / 2 - slCorsshairSize.Value / 2;
                x2 = Variables.Width / 2 + slCorsshairSize.Value / 2;
                y1 = Variables.Height/ 2 - slCorsshairSize.Value / 2;
                y2 = Variables.Height/ 2 + slCorsshairSize.Value / 2;

                ImGui.ImDrawLine(x1, Variables.Height / 2, x2, Variables.Height / 2, slCrosshairThickness.Value, cpCrosshairColor.Color.RByte, cpCrosshairColor.Color.GByte, cpCrosshairColor.Color.BByte, cpCrosshairColor.Color.AByte);
                ImGui.ImDrawLine(Variables.Width / 2, y1, Variables.Width / 2, y2, slCrosshairThickness.Value, cpCrosshairColor.Color.RByte, cpCrosshairColor.Color.GByte, cpCrosshairColor.Color.BByte, cpCrosshairColor.Color.AByte);
            }

            if (!cbOverlayEnabled.Checked) return;

            Font.Default().PushFont();

            ImGui.ImDrawText(_overlayText, 12, 36, slOverlaySize.Value, cpOverlayColor.Color.RByte, cpOverlayColor.Color.GByte, cpOverlayColor.Color.BByte, cpOverlayColor.Color.AByte);

            Font.Default().PopFont();
        }

        private string _overlayText = "[Tunnel Dweller - Overlay]";
    }
}
