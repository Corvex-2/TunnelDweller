using Newtonsoft.Json;
using SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.Configure
{
    public class Config
    {
        public Config() : this(null) { }
        public Config(string Name) : this(Name, "") 
        { 

        }
        public Config(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
            Load();
        }

        protected bool ExistsOnDisk()
        {
            return File.Exists(ConfigFilePath);
        }

        public void Save()
        {
            if ((DateTime.UtcNow - LastSaved).TotalSeconds < 20)
                return;

            if (!ExistsOnDisk())
                File.Create(ConfigFilePath).Close();

            var fileContent = JsonConvert.SerializeObject(this);
            File.WriteAllText(ConfigFilePath, fileContent );
            LastSaved = DateTime.UtcNow;
        }

        public void Load()
        {
            if(!ExistsOnDisk()) return;
            var fileContent = File.ReadAllText(ConfigFilePath);
            JsonConvert.PopulateObject(fileContent, this);
        }

        private string ConfigFilePath
        {
            get {

                if (string.IsNullOrWhiteSpace(Name))
                    throw new Exception("The Name of the config cannot be NULL or Empty!");
                
                return $"{GlobalPath}\\{Name}.json"; }
        }

        private static string GlobalPath
        {
            get
            {
                var path = Process.GetCurrentProcess().MainModule.FileName.Replace("metro.exe", "config\\");

                if(!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        private DateTime LastSaved = DateTime.MinValue;
    }
}
