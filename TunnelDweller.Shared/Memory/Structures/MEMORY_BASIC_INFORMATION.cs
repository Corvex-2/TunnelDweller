using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelDweller.Shared.Memory.Structures
{
    internal struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public uint AllocationProtect;
        public uint RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }
}
