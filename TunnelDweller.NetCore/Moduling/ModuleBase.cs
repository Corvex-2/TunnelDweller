using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.Moduling
{
    internal class ModuleBase
    {
        public ModuleBase(string FilePath, string FIleName, byte[] FileData)
        {
            this.FilePath = FilePath;
            this.FileData = FileData;
            this.FileName = FIleName;
        }

        public byte[] FileData { get; private set; }
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
    }
}
