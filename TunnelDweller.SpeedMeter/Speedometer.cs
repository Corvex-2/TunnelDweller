using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Extensions;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Rendering;
using TunnelDweller.NetCore.Threading;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.SpeedMeter
{
    internal static class Speedometer
    {
        #region Menu Components

        public const ImGuiWindowFlags MOVABLE =     /*ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize | */
                                                    ImGuiWindowFlags.ImGuiWindowFlags_NoDecoration;

        public const ImGuiWindowFlags UNMOVABLE =   /*ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize |*/
                                                    ImGuiWindowFlags.ImGuiWindowFlags_NoDecoration |
                                                    ImGuiWindowFlags.ImGuiWindowFlags_NoBackground |
                                                    ImGuiWindowFlags.ImGuiWindowFlags_NoInputs;

        public static Window SpeedometerWindow = new Window("SpeedoWindow", true)
        {
            Flags = UNMOVABLE,
            Show = true,
        };

        public static ShadedPlot SpeedoPlot = new ShadedPlot("SpeedoPlot", null) 
        { 
            MinX = 0,
            MinY = 0,
            MaxX = 1000,
            MaxY = 10,
            AxisFlags = ImPlotAxisFlags.ImPlotAxisFlags_NoDecorations,
            PlotFlags = ImPlotFlags.ImPlotFlags_CanvasOnly | 
                        ImPlotFlags.ImPlotFlags_NoFrame | 
                        ImPlotFlags.ImPlotFlags_NoInputs,
            UseColours = true,
            ForegroundColor = new col32_t(.4f, 1f, .5f, 0.5f),
            BackgroundColor = new col32_t(0f, 0f, 0f, 0f),
            BorderColor = new col32_t(0f, 0f, 0f, 0f)
        };
        public static Label SpeedoPlotText = new Label("0 u") { Font = Font.Default(), };

        public static TabItem SpeedoTab = new TabItem("Speedmeter");

        public static CheckBox cbSpeedoEnabled = new CheckBox("Enable Speedmeter", true);
        public static ColorPicker cpSpeedoColor = new ColorPicker("Speedo Color", 102, 25, 123, 123);
        public static ColorPicker cpSpeedoTextColor = new ColorPicker("Speedo Text Color", 152, 55, 183, 200);

        #endregion

        public static List<float> DataPoints = new List<float>();

        public static void Initialize()
        {
            SpeedoTab.Controls.Add(cbSpeedoEnabled);
            SpeedoTab.Controls.Add(new Seperator());
            SpeedoTab.Controls.Add(new Label("Speed o Meter Settings"));
            SpeedoTab.Controls.Add(cpSpeedoColor);
            SpeedoTab.Controls.Add(cpSpeedoTextColor);

            SpeedometerWindow.MaxSize = new vec2_t(100, 100);
            SpeedometerWindow.MinSize = new vec2_t(100, 100);
            SpeedometerWindow.Size = new vec2_t(100, 100);
            SpeedoPlot.Size = new vec2_t(198, 98);
            SpeedoPlot.Position = new vec2_t(1, 1);
            SpeedoPlotText.Position = new vec2_t(75 - Font.Default().Size / 2, 50 - Font.Default().Size / 2);

            SpeedometerWindow.Controls.Add(SpeedoPlot);
            SpeedometerWindow.Controls.Add(SpeedoPlotText);

            DataPoints = Enumerable.Repeat<float>(0f, 1000).ToList();

            Window.MainTabs.Items.Add(SpeedoTab);

            Update.OnUpdate += Update_OnUpdate;
            Renderer.OnRender += Renderer_OnRender;
        }

        private static void Renderer_OnRender(object sender, EventArgs e)
        {
            lock(DataPoints) //threadlock the data points in order to prevent crashes due to race conditions. 
            {
                SpeedoPlot.Data = DataPoints.ToArray(); //ToArray creates a duplicate of the data points, therefore the following code can be executed without threadlocking the data points.
            }

            if(SpeedoPlot.Data != null && SpeedoPlot.Data.Length > 0 && cbSpeedoEnabled.Checked) //ensure we have data and speedo is enabled.
            {
                SpeedometerWindow.Paint(); //Draw the dummy Window.
            }
        }

        private static void Update_OnUpdate(object sender, EventArgs e)
        {
            //threadlock the data points in order to prevent crashes due to race conditions. 
            lock(DataPoints) {

                while (DataPoints.Count > 1000)
                {
                    DataPoints.RemoveAt(0);
                }

                DataPoints.Add(NormalizeVelocity(Variables.Velocity));

                SpeedoPlotText.Text = $"{DataPoints.Last().ToString("0.00")} u";
                SpeedoPlot.ForegroundColor = cpSpeedoColor.Color;
                SpeedoPlotText.Color = cpSpeedoTextColor.Color;
                
                if (Window.MainWindow.Show)
                {
                    SpeedometerWindow.Flags = MOVABLE;
                    SpeedoPlot.BorderColor = new col32_t(1f, 1f, 1f, 1f);
                }
                else
                {
                    SpeedometerWindow.Flags = UNMOVABLE;
                    SpeedoPlot.BorderColor = new col32_t(0f, 0f, 0f, 0f);
                }
            }
        }

        //Because the Velocity is based on vx and vy we need to normalize the data to achive equal distribution. the vz is ignored, as it doesn't actually contribute to our rt velocity!
        public static float NormalizeVelocity(vec3_t veloc)
        {
            // Convert negative values to positive using Math.Abs
            float v_x = Math.Abs(veloc.x);
            float v_y = Math.Abs(veloc.y);

            // If both velocities are 0, return 0
            if (v_x == 0 && v_y == 0)
                return 0;

            // If one velocity is 0, return the magnitude of the non-zero velocity
            if (v_x == 0)
                return v_y;
            if (v_y == 0)
                return v_x;

            // Calculate combined velocity magnitude
            float v_combined = (float)Math.Sqrt(Math.Pow(v_x, 2) + Math.Pow(v_y, 2));

            return v_combined;
        }
    }
}
