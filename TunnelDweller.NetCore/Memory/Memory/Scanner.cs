using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TunnelDweller.Memory
{
    internal class Scanner
    {
        private List<MEMORY_BASIC_INFORMATION> READABLE_PAGES = new List<MEMORY_BASIC_INFORMATION>();

        public Process Process
        {
            get
            {
                return Process.GetCurrentProcess();
            }
        }
        public IntPtr Handle
        {
            get
            {
                return Process.Handle;
            }
        }

        public void PreparePages()
        {
            IntPtr ADDRESS = IntPtr.Zero;
            while (true)
            {
                MEMORY_BASIC_INFORMATION MemInfo = new MEMORY_BASIC_INFORMATION();
                int MemDump = Natives.VirtualQuery(ADDRESS, ref MemInfo, Marshal.SizeOf(MemInfo));
                if (MemDump == 0) break;
                if (ADDRESS.ToInt32() == 0x7FFF0000)
                {
                    MemInfo.RegionSize -= 1;
                    break;
                }
                if ((MemInfo.State & 0x1000) != 0 && (MemInfo.Protect & 0x100) == 0)
                    READABLE_PAGES.Add(MemInfo);
                ADDRESS = new IntPtr(MemInfo.BaseAddress.ToInt32() + MemInfo.RegionSize);
            }
        }

        public IntPtr Scan(Signature Signature)
        {
            if (READABLE_PAGES.Count == 0)
                PreparePages();

            IntPtr ptr = IntPtr.Zero;
            for (int i = 0; i < READABLE_PAGES.Count; i++)
            {
                var Page = READABLE_PAGES[i];
                ptr = Scan(Page.BaseAddress, (int)Page.RegionSize, Signature);

                if (ptr != IntPtr.Zero)
                    return ptr;
            }
            return ptr;
        }

        public IntPtr Scan(IntPtr Module, int Size, Signature Signature)
        {
            //var buffer = MemoryManager.Read(Module, Size);
            var buffer = MemoryManager.ReadStealth(Module, Size); //MemoryManager.Read(Module, Size); //Causes Crashing.
            var ptr = FindPattern(buffer, Signature.Mask, Module);
            if (ptr != IntPtr.Zero)
                return FindPattern(buffer, Signature.Mask, Module) + Signature.Offset;
            return IntPtr.Zero;
        }

        public IntPtr FindPattern(byte[] Buffer, string Pattern, IntPtr BaseAddress)
        {
            string[] pa = Pattern.Split(' ');
            byte fb = byte.Parse(pa[0], System.Globalization.NumberStyles.HexNumber);

            for (int i = 0; i <= (Buffer.Length - pa.Length); i++)
            {
                if (Buffer[i] != fb)
                    continue;
                var result = InverseMatchPattern(Buffer, pa, i);//MatchPattern(Buffer, pa, i);
                if (result)
                {
                    return (BaseAddress + i);
                }
            }

            return IntPtr.Zero;
        }
        public bool InverseMatchPattern(byte[] buffer, string[] pattern, int beginAt)
        {
            for(int i = pattern.Length - 1; i > 0; i--)
            {
                if (pattern[i] == "?")
                    continue;
                var a = byte.Parse(pattern[i], System.Globalization.NumberStyles.HexNumber);
                if (buffer[beginAt + i] != a)
                    return false;
            }
            return true;
        }
        public bool MatchPattern(byte[] buffer, string[] Pattern, int beginAt)
        {
            int patternIndex = 0;
            for (int i = beginAt; i < buffer.Length; i++)
            {
                if (patternIndex == Pattern.Length)
                    break;
                if (Pattern[patternIndex] == "?")
                {
                    patternIndex++;
                    continue;
                }
                var a = byte.Parse(Pattern[patternIndex], System.Globalization.NumberStyles.HexNumber);
                if (buffer[i] != a)
                    return false;
                patternIndex++;
            }
            return true;
        }
    }
}
