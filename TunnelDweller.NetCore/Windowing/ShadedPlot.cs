using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Windowing
{
    public class ShadedPlot : Plot
    {
        public float[] Data { get; set; } = new float[0];

        public ShadedPlot(string Name, float[] Data) : base(Name)
        {
            this.Data = Data;
        }

        public override void Paint()
        {
            if(!Visible) return;

            if(Start())
            {
                if (Data == null || Data.Length <= 0) return;

                ImGui.PlotShaded(Name + $"###{Name}shadeGraph", Data, Data.Length);

                End();
            }
        }
    }
}
