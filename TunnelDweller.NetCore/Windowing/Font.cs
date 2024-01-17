using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.Memory;
using TunnelDweller.NetCore.DearImgui;

namespace TunnelDweller.NetCore.Windowing
{
    public class Font : IDisposable
    {
        public static Dictionary<string, Font> FontCollection = new Dictionary<string, Font>();

        private static Font _default;

        public string Name { get; private set; }
        public float Size { get; private set; }

        private byte[] FontData;
        private IntPtr FontPtr;
        private IntPtr MemPtr;
        private bool Pushed;
        private bool Loaded;
        private NetFontInfo_t FontInfo;

        public Font(byte[] FontData, string Name, float Size = 16f)
        {
            if (!FontCollection.ContainsKey(Name))
                FontCollection.Add(Name, this);

            this.FontData = FontData;
            this.Name = Name;
            this.Size = Size;
        }

        // Fonts are awkward to use with ImGui
        private void Upload()
        {
            if(Loaded) return;

            if (!ImGui.ContainsFont(Name, Size))
            {
                MemPtr = Natives.VirtualAlloc(IntPtr.Zero, FontData.Length, 0x00001000, (int)PROTECTION.PAGE_EXECUTE_READWRITE); //Marshal.AllocHGlobal(FontData.Length);

                Marshal.Copy(FontData, 0, MemPtr, FontData.Length);

                FontInfo = ImGui.AddMemoryFont(Name, MemPtr, FontData.Length, Size);

                FontPtr = FontInfo.imguiptr;

                Console.WriteLine($"Uploading Font {Name}\r\nNative Ptr: {MemPtr.ToString("X")}\r\nImgui Ptr: {FontPtr.ToString("X")}");
            }
            else
            {
                FontInfo = ImGui.GetFont(Name, Size);
                FontPtr = FontInfo.imguiptr;                
                MemPtr = FontInfo.nativeptr;
                Console.WriteLine($"Getting Font from Backend {Name}\r\nNative Ptr: {MemPtr.ToString("X")}\r\nImgui Ptr: {FontPtr.ToString("X")}");
            }


            Loaded = true;
        }

        public void PushFont()
        {
            if(Loaded && !Pushed)
            {
                ImGui.PushFont(FontPtr);
                Pushed = true;
            }
        }

        public void PopFont()
        {
            if (Loaded && Pushed)
            {
                ImGui.PopFont();
                Pushed = false;
            }
        }

        static Font()
        {
            if (_default == null)
            {
                var asm = Assembly.GetExecutingAssembly();
                var resName = asm.GetName().Name + ".Resources.Raleway-Regular.ttf";
                using (var n = asm.GetManifestResourceStream(resName))
                {
                    if (n != null)
                    {
                        var buffer = new byte[n.Length];
                        n.Read(buffer, 0, buffer.Length);

                        _default = new Font(buffer, "TunnelDweller.Default");
                    }
                    else
                        Console.WriteLine("Error Creating Default Font");
                }
            }
        }

        public static void UploadFonts()
        {
            foreach(var e in FontCollection)
            {
                if(!e.Value.Loaded)
                {
                    e.Value.Upload();
                }
            }
        }

        public static Font GetFromResources(string resourceName, string FontName, float FontSize = 16f)
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var n = asm.GetManifestResourceStream(resourceName))
            {
                if (n != null)
                {
                    var buffer = new byte[n.Length];
                    n.Read(buffer, 0, buffer.Length);

                    return new Font(buffer, FontName, FontSize);
                }
                else
                    Console.WriteLine("Error Loading Font " + resourceName + " / " + FontName);
            }
            return Default();
        }

        public static Font Default()
        {
            return _default;
        }

        public void Dispose()
        {
            if (Pushed)
                ImGui.PopFont();
        }
    }
}
