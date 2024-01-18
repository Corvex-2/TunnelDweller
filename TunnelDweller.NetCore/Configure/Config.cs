using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.Configure
{
    public class Config
    {
        public Config(string Name) : this(Name, "") { }
        public Config(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
        }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
