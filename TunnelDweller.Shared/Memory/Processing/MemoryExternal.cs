using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using TunnelDweller.Shared.Memory.Enums;

namespace TunnelDweller.Shared.Memory.Processing
{
    public class MemoryExternal : IMemory, IDisposable
    {
        public Process Process { get; set; }

        public IntPtr Base
        {
            get
            {
                return _hBase;
            }
        }

        public MemoryExternal()
        {

        }
        public MemoryExternal(Process Process)
        {
            this.Process = Process;
            Initialize();
        }

        public void Initialize()
        {
            if(Process != null && _hHandle == IntPtr.Zero)
            {
                _hBase = Process.MainModule.BaseAddress;
                _hHandle = Win32Natives.OpenProcess(ProcessAccessRights.PROCESS_ALL_ACCESS, false, Process.Id);
            }
        }

        public byte[] Read(IntPtr lpMemoryAddress, int dwReadSize)
        {
            if (_hHandle == IntPtr.Zero)
                return default;

            byte[] buffer = new byte[dwReadSize];
            int read = 0;
            Win32Natives.VirtualProtectEx(_hHandle, lpMemoryAddress, buffer.Length, Protection.PAGE_EXECUTE_READWRITE, out var prot);
            Win32Natives.ReadProcessMemory(_hHandle, lpMemoryAddress, buffer, dwReadSize, ref read);
            Win32Natives.VirtualProtectEx(_hHandle, lpMemoryAddress, buffer.Length, prot, out var _);
            return buffer;
        }

        public T Read<T>(IntPtr lpMemoryAddress) where T : struct
        {
            if (_hHandle == IntPtr.Zero)
                return default;

            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            int read = 0;
            Win32Natives.VirtualProtectEx(_hHandle, lpMemoryAddress, buffer.Length, Protection.PAGE_EXECUTE_READWRITE, out var prot);
            Win32Natives.ReadProcessMemory(_hHandle, lpMemoryAddress, buffer, buffer.Length, ref read);
            Win32Natives.VirtualProtectEx(_hHandle, lpMemoryAddress, buffer.Length, prot, out var _);

            if(read == buffer.Length)
            {
                IntPtr alloc = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, alloc, buffer.Length);
                var ret = Marshal.PtrToStructure<T>(alloc);
                Marshal.FreeHGlobal(alloc);
                return ret;
            }

            return default(T);
        }

        public string ReadString(IntPtr lpMemoryAddress, Encoding e, int Length)
        {
            if (_hHandle == IntPtr.Zero)
                return default;

            byte[] strData = Read(lpMemoryAddress, Length);
            return e.GetString(strData).TrimEnd('\0');
        }
        public void Write(IntPtr lpMemoryAddress, byte[] value)
        {
            if (_hHandle == IntPtr.Zero)
                return;

            int write = 0;
            Win32Natives.VirtualProtectEx(_hHandle, lpMemoryAddress, value.Length, Protection.PAGE_EXECUTE_READWRITE, out var prot);
            Win32Natives.WriteProcessMemory(_hHandle, lpMemoryAddress, value, value.Length, ref write);
            Win32Natives.VirtualProtectEx(_hHandle, lpMemoryAddress, value.Length, prot, out var _);
        }
        public void Write<T>(IntPtr lpMemoryAddress, T value) where T : struct
        {
            if (_hHandle == IntPtr.Zero)
                return;

            var buffer = new byte[Marshal.SizeOf(typeof(T))];

            IntPtr alloc = Marshal.AllocHGlobal(buffer.Length);
            Marshal.StructureToPtr(value, alloc, false);
            Marshal.Copy(alloc, buffer, 0, buffer.Length);
            Marshal.FreeHGlobal(alloc);
            
            Write(lpMemoryAddress, buffer);
        }

        public void WriteString(IntPtr lpMemoryAddress, string value, Encoding e)
        {
            if (_hHandle == IntPtr.Zero)
                return;
            var data = e.GetBytes(value);
            Write(lpMemoryAddress, data);
        }

        public IntPtr AllocateMemory(int dwSize)
        {
            return Win32Natives.VirtualAllocEx(_hHandle, IntPtr.Zero, dwSize, AllocationType.MEM_COMMIT, Protection.PAGE_EXECUTE_READWRITE);
        }

        public IntPtr GetModuleHandle(string lpModuleName)
        {
            return IntPtr.Zero; //i don't think its needed tbh
        }

        public IntPtr GetProcAddress(string lpModuleName, string lpProcName)
        {
            return IntPtr.Zero; //i don't think its needed tbh
        }

        public void Dispose()
        {
            if(_hHandle != IntPtr.Zero)
            {
                Win32Natives.CloseHandle(_hHandle);
                _hHandle = IntPtr.Zero;
            }
        }

        private IntPtr _hHandle;
        private IntPtr _hBase;
    }
}
