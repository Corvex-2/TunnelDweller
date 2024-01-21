using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.Moduling;
using TunnelDweller.NetCore.Rendering;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.SpeedMeter
{
    public class Startup : ModuleEntryPoint
    {
        public List<float> Velocities { get; set; } = new List<float>();

        public Window TestWindow = new Window("Graph", true);
        public ShadedPlot Plot = new ShadedPlot("Velocities", null);

        public override void Initialize()
        {
            Velocities = Enumerable.Repeat(0f, 1000).ToList();
            Renderer.OnRender += Renderer_OnRender;

            Plot.MinX = 0f;
            Plot.MaxX = 1000f;
            Plot.MinY = 0f;
            Plot.MaxY = 20f;
            Plot.AxisFlags = ImPlotAxisFlags.ImPlotAxisFlags_NoDecorations;
            Plot.PlotFlags = ImPlotFlags.ImPlotFlags_CanvasOnly | ImPlotFlags.ImPlotFlags_NoFrame | ImPlotFlags.ImPlotFlags_NoInputs;
            Plot.ForegroundColor = new col32_t(.4f, 1f, .5f, 0.5f);
            Plot.BackgroundColor = new col32_t(0f, 0f, 0f, 0f);

            TestWindow.Show = true;
            TestWindow.Controls.Add(Plot);
            TestWindow.Flags = ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize | ImGuiWindowFlags.ImGuiWindowFlags_NoDecoration | ImGuiWindowFlags.ImGuiWindowFlags_NoBackground | ImGuiWindowFlags.ImGuiWindowFlags_NoInputs;
            TestWindow.Size = new vec2_t(300, 500);
            TestWindow.Position = new vec2_t(12, 48);
        }

        private void Renderer_OnRender(object sender, EventArgs e)
        {
            var veloc = Variables.Velocity;

            while(Velocities.Count > 1000)
                Velocities.RemoveAt(0);

            Velocities.Add(Math.Abs(veloc.x) + Math.Abs(veloc.y)); //velocities can be negative, enforce absolute value (if positive -> flip signbit)

            Plot.Data = Velocities.ToArray();

            ImGui.PushPlotStyleColor(ImPlotCol.ImPlotCol_PlotBorder, new col32_t(1f, 1f, 1f, 0f));

            TestWindow.Paint();

            ImGui.PopPlotStyleColor();
        }
    }
}