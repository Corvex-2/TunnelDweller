using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using TunnelDweller.Shared.Memory.Enums;

namespace TunnelDweller.Shared.Memory.Processing
{
    public class InternalMemory : IMemory, IDisposable
    {
        public Process Process { get; set; }

        public InternalMemory()
        {
            Process = Process.GetCurrentProcess();
            Initialize();
        }

        public IntPtr Base
        {
            get
            {
                return Process.MainModule.BaseAddress;
            }
        }

        public IntPtr AllocateHGlobal(int dwSize)
        {
            return Marshal.AllocHGlobal(dwSize);
        }
        public IntPtr AllocateMemory(int dwSize)
        {
            return Win32Natives.VirtualAlloc(IntPtr.Zero, dwSize, AllocationType.MEM_COMMIT, Protection.PAGE_EXECUTE_READWRITE);
        }

        public void Dispose()
        {
            
        }

        public IntPtr GetModuleHandle(string name)
        {
            return Win32Natives.GetModuleHandle(name);
        }

        public IntPtr GetProcAddress(string module, string name)
        {
            return Win32Natives.GetProcAddress(GetModuleHandle(module), name);
        }

        public void Initialize()
        {

        }

        public byte[] Read(IntPtr lpMemoryAddress, int dwReadSize)
        {
            if (lpMemoryAddress.ToInt64() < 0xFFFF)
                return new byte[] { };

            var buffer = new byte[dwReadSize];
            Win32Natives.VirtualProtect(lpMemoryAddress, dwReadSize, Protection.PAGE_EXECUTE_READWRITE, out var prot);
            Marshal.Copy(lpMemoryAddress, buffer, 0, dwReadSize);
            Win32Natives.VirtualProtect(lpMemoryAddress, dwReadSize, prot, out var _);
            return buffer;
        }

        public T Read<T>(IntPtr lpMemoryAddress) where T : struct
        {
            if (lpMemoryAddress.ToInt64() < 0xFFFF)
                return default(T);

            return Marshal.PtrToStructure<T>(lpMemoryAddress);
        }

        public string ReadString(IntPtr lpMemoryAddress, Encoding e, int dwSize)
        {
            if (lpMemoryAddress.ToInt64() < 0xFFFF)
                return "";

            var buffer = Read(lpMemoryAddress, dwSize);
            return e.GetString(buffer).Trim('\0');
        }

        public void Write(IntPtr lpMemoryAddress, byte[] value)
        {
            if (lpMemoryAddress.ToInt64() < 0xFFFF)
                return;

            Win32Natives.VirtualProtect(lpMemoryAddress, value.Length, Protection.PAGE_EXECUTE_READWRITE, out var prot);
            Marshal.Copy(value, 0, lpMemoryAddress, value.Length);
            Win32Natives.VirtualProtect(lpMemoryAddress, value.Length, prot, out var _);
        }

        public void Write<T>(IntPtr lpMemoryAddress, T value) where T : struct
        {
            if (lpMemoryAddress.ToInt64() < 0xFFFF)
                return;

            int _size = Marshal.SizeOf<T>();
            byte[] _buffer = new byte[_size];
            IntPtr _nativePointer = Marshal.AllocHGlobal(_size);
            Marshal.StructureToPtr<T>(value, _nativePointer, true);
            Marshal.Copy(_nativePointer, _buffer, 0, _size);
            Marshal.FreeHGlobal(_nativePointer);

            Write(lpMemoryAddress, _buffer);
        }

        public void WriteString(IntPtr lpMemoryAddress, string value, Encoding e)
        {
            if (lpMemoryAddress.ToInt64() < 0xFFFF)
                return;

            var buffer = e.GetBytes(value);
            Write(lpMemoryAddress, buffer);
        }
    }
}
