using System;
using System.Linq;
using System.Reflection;
using TunnelDweller.NetCore.Windowing;

namespace TunnelDweller.NetCore.Moduling
{
    internal class ModuleView : Control
    {
        private bool Loaded;
        internal ModuleBase Module;
        public Button btnLoadModule;
        public Label lblNameModule;

        public ModuleView(ModuleBase module)
        {
            this.Module = module;
            lblNameModule = new Label(module.FileName);
            btnLoadModule = new Button($"Load###{module.FileName}", Load);
            btnLoadModule.Sameline = true;

            if (module.Networked)
                lblNameModule.Color = new DearImgui.col32_t(0, 255, 136);

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("NetCore"))
                return Assembly.GetExecutingAssembly();
            else
                return null;
        }

        public override void Paint()
        {
            lblNameModule.Paint();
            btnLoadModule.Paint();
        }

        private void Load()
        {
            if (Loaded)
                return;

            try
            {
                var asm = Assembly.Load(Module.FileData);
                if (asm != null)
                {
                    Loaded = true;
                    btnLoadModule.Visible = false;
                    var types = asm.GetTypes();
                    if (types.Length > 0 )
                    {
                         for (int i = 0; i< types.Length; i++)
                        {
                            if (types != null && types[i] != null && types[i].BaseType != null && types[i].BaseType == typeof(ModuleEntryPoint))
                            {
                                var isnt = Activator.CreateInstance(types[i]);
                                ((ModuleEntryPoint)isnt).Initialize();
                            }
                        }
                    }

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override bool Visibility()
        {
            return true;
        }
    }
}
