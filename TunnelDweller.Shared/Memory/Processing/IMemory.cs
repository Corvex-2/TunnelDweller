using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TunnelDweller.Shared.Memory.Processing
{
    internal interface IMemory
    {
        Process Process { get; set; }

        IntPtr Base { get; }

        void Initialize();

        byte[] Read(IntPtr lpMemoryAddress, int dwReadSize);
        T Read<T>(IntPtr lpMemoryAddress) where T : struct;
        void Write(IntPtr lpMemoryAddress, byte[] value);
        void Write<T>(IntPtr lpMemoryAddress, T value) where T : struct;
        string ReadString(IntPtr lpMemoryAddress, Encoding e, int dwSize);
        void WriteString(IntPtr lpMemoryAddress, string value, Encoding e);
        IntPtr AllocateMemory(int dwSize);
        IntPtr GetModuleHandle(string name);
        IntPtr GetProcAddress(string module, string name);
    }
}
