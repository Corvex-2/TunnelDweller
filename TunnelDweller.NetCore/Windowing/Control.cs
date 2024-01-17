using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Randomness;

namespace TunnelDweller.NetCore.Windowing
{
    public abstract class Control
    {
        internal string StackID { get; set; }
        public bool Sameline { get; set; }
        public string Tooltip { get; set; }

        public Control()
        {
            this.StackID = "###" + SecureRandom.NextString(16, SecureRandom.DEFAULT_CHAR_SET);
        }

        public abstract bool Visibility();
        public abstract void Paint();
    }
}
