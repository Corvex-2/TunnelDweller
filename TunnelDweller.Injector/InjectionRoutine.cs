using Lunar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TunnelDweller.Injector
{
    internal static class InjectionRoutine
    {
        private const int WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpbaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        private static IntPtr Open(Process p)
        {
            return OpenProcess(WM_READ, false, p.Id);
        }

        private static bool Close(IntPtr p)
        {
            return CloseHandle(p);
        }

        public static bool IsInjected(Process p) //yeah lets prevent accidentally injecting twice!
        {
            var Handle = Open(p);

            if(Handle != IntPtr.Zero)
            {
                var Addr = p.MainModule.BaseAddress + 0x3b0;

                var data = new byte[64];
                int numread = 0;

                if(ReadProcessMemory(Handle, Addr, data, data.Length, ref numread))
                {
                    var str = Encoding.Unicode.GetString(data);

                    if (str.StartsWith("TunnelDweller"))
                    {
                        Close(Handle);
                        return true;
                    }
                    else
                        CloseHandle(Handle);
                }
            }
            return false;
        }

        public static bool EmbedReleaseStream(Process p, string releaseStream)
        {
            var Handle = Open(p);

            if (Handle != IntPtr.Zero)
            {
                var Addr = p.MainModule.BaseAddress + 0x3b0 + 0x20;

                int numread = 0;
                var data = Encoding.Unicode.GetBytes(releaseStream);

                if (WriteProcessMemory(Handle, Addr, data, data.Length, ref numread))
                {
                    byte[] buf = new byte[32];

                    ReadProcessMemory(Handle, Addr, buf, buf.Length, ref numread);

                    var str = Encoding.Unicode.GetString(buf);

                    if (str.StartsWith(releaseStream))
                    {
                        Close(Handle);
                        return true;
                    }
                    else
                        CloseHandle(Handle);
                }
            }
            return false;
        }

        public static void InjectCore(byte[] data)
        {
            var Mapper = new LibraryMapper(Process.GetProcessesByName("metro").First(), new Memory<byte>(data), MappingFlags.None);
            Mapper.MapLibrary();
        }

        public static void InjectNetCore(byte[] data)
        {
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream(".", "TUNNEL.DWELLER", PipeDirection.InOut))
            {
                pipeStream.Connect();

                if (!pipeStream.IsConnected)
                {
                    MessageBox.Show("An Error occured while injecting! TUNNEL.DWELLER Pipe not connected!");
                    return;
                }

                char[] buffer = new char[256];
                string strbuffer = "";

                using (StreamReader reader = new StreamReader(pipeStream))
                {
                    while (true)
                    {
                        if (reader.Peek() > 0)
                        {
                            reader.Read(buffer, 0, buffer.Length);
                            strbuffer = new string(buffer, 0, buffer.Length);
                            if (strbuffer.StartsWith("TUNNEL.DWELLER"))
                            {
                                pipeStream.Flush();
                                var writeData = new byte[data.Length + 4];
                                var lengthBytes = BitConverter.GetBytes(data.Length);
                                Array.Copy(lengthBytes, 0, writeData, 0, 4);
                                Array.Copy(data, 0, writeData, 4, data.Length);
                                pipeStream.Write(writeData, 0, writeData.Length);
                                pipeStream.Flush();
                                pipeStream.Close();
                                Environment.Exit(0);
                            }
                        }
                    }
                }
            }
        }
    }
}
