using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.Moduling
{
    internal class ModuleBase
    {
        public ModuleBase(string FilePath, string FIleName, byte[] FileData, bool Networked = false)
        {
            this.FilePath = FilePath;
            this.FileData = FileData;
            this.FileName = FIleName;
            this.Networked = Networked;
        }

        public byte[] FileData { get; private set; }
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public bool Networked { get; private set; } 
    }
}
