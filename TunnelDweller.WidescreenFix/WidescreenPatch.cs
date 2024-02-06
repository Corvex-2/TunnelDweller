using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Game;

namespace TunnelDweller.WidescreenFix
{
    internal class WidescreenPatch
    {
        public bool Patched { get; private set; }
        public int Offset { get; private set; }
        public byte[] Opcodes { get; private set; }
        
        public WidescreenPatch(int offset, int length)
        {
            Offset = offset;
            this.Opcodes = Variables.MemoryManager.Read(Variables.MemoryManager.Base + offset, length);
        }

        public void Patch()
        {
            if(IsPatchedAlready())
                Patched = true;

            if (Patched) return;

            var patchData = Enumerable.Repeat<byte>(0x90, Opcodes.Length).ToArray();
            var ptr = Variables.MemoryManager.Base + Offset;
            Variables.MemoryManager.Write(ptr, patchData);
            Patched = true;
        }

        public void Unpatch()
        {
            if (!IsPatchedAlready())
                Patched = false;

            if (!Patched) return;

            var ptr = Variables.MemoryManager.Base + Offset;
            Variables.MemoryManager.Write(ptr, Opcodes);
            Patched = false;
        }

        public bool IsPatchedAlready()
        {
            var ptr = Variables.MemoryManager.Base + Offset;
            return Variables.MemoryManager.Read<byte>(ptr) == 0x90;
        }
    }
}
