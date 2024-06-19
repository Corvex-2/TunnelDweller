using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.Injector
{
    public class TechnicalMetroApiFile
    {
        public TechnicalMetroApiFile(string fileName, byte[] fileData)
        {
            FileName = fileName;
            FileData = fileData;
        }   

        public string FileName { get; set; }
        public byte[] FileData { get; set; }

        public void Create(string DirectoryPath)
        {
            if (FileName.StartsWith("NoLocal"))
                return;

            if(Directory.Exists(DirectoryPath))
            {
                File.Create(Path.Combine(DirectoryPath, FileName)).Close();
                File.WriteAllBytes(Path.Combine(DirectoryPath, FileName), FileData);
            }
        }
    }
}
