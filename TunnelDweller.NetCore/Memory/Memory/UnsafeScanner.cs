using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.Memory
{
    public static unsafe class UnsafeScanner
    {
        private static List<MEMORY_BASIC_INFORMATION> MEMORY_PAGES = new List<MEMORY_BASIC_INFORMATION>();

        internal static Process Process
        {
            get
            {
                return Process.GetCurrentProcess();
            }
        }
        internal static IntPtr Handle
        {
            get
            {
                return Process.Handle;
            }
        }


        static UnsafeScanner()
        {
            IntPtr ADDRESS = IntPtr.Zero;
            //Logging.Log($"UnsafeScanner: Creating MBI Table.");
            while (true)
            {
                //Logging.Log($"UnsafeScanner: MBI Address is {ADDRESS.ToString("X")}.");
                MEMORY_BASIC_INFORMATION MemInfo = new MEMORY_BASIC_INFORMATION();
                int MemDump = Natives.VirtualQueryEx(Handle, ADDRESS, ref MemInfo, Marshal.SizeOf(MemInfo));
                if (MemDump == 0) break;
                if (ADDRESS.ToInt32() == 0x7FFF0000)
                {
                    MemInfo.RegionSize -= 1;
                    break;
                }
                if ((MemInfo.State & 0x1000) != 0 && (MemInfo.Protect & 0x100) == 0)
                    MEMORY_PAGES.Add(MemInfo);
                ADDRESS = new IntPtr(MemInfo.BaseAddress.ToInt32() + MemInfo.RegionSize);
            }
        }

        internal static IntPtr Scan(Signature Signature)
        {
            IntPtr ptr = IntPtr.Zero;
            for (int i = 0; i < MEMORY_PAGES.Count; i++)
            {
                var MEMORY_PAGE = MEMORY_PAGES[i];
                //Logging.Log($"Scanning for {Signature.Mask} on {MEMORY_PAGE.BaseAddress.ToString("X")} (REGIONSIZE: {MEMORY_PAGE.RegionSize.ToString("X")} - PROTECTION: {((PROTECTION)MEMORY_PAGE.Protect).ToString()})");
                ptr = FindPattern(MEMORY_PAGE, Signature);
            }
            return ptr;
        }

        internal static unsafe IntPtr FindPattern(MEMORY_BASIC_INFORMATION MEMORY_PAGE, Signature Signature)
        {
            byte* BYTE_BASE_PTR = (byte*)MEMORY_PAGE.BaseAddress.ToPointer(); //no memory reading, this is better.
            string[] PATTERN_AS_ARRAY = Signature.Mask.Split(' ');
            byte FIRST_BYTE = byte.Parse(PATTERN_AS_ARRAY[0], NumberStyles.HexNumber);

            var xxxxx = new MEMORY_BASIC_INFORMATION();
            Natives.VirtualQueryEx(Process.Handle, MEMORY_PAGE.BaseAddress, ref xxxxx, Marshal.SizeOf(xxxxx));
            if (xxxxx.Protect == 0x00)
                return IntPtr.Zero;

            for (uint REGION_INDEX = 0; REGION_INDEX < MEMORY_PAGE.RegionSize; REGION_INDEX++)
            {
                if (BYTE_BASE_PTR == null || BYTE_BASE_PTR[REGION_INDEX] != FIRST_BYTE) //skip to next byte, first byte mismatch;
                    continue;
                if (MatchPattern((BYTE_BASE_PTR + REGION_INDEX), PATTERN_AS_ARRAY))
                {
                    return new IntPtr((BYTE_BASE_PTR + REGION_INDEX));
                }
            }
            return IntPtr.Zero;
        }

        internal static unsafe bool MatchPattern(byte* BYTE_BASE_PTR, string[] PATTERN_AS_ARRAY)
        {
            for(int i = PATTERN_AS_ARRAY.Length - 1; i > 0; i--) //Reverse Lookup, better to check if the last byte matches first as we already know that the first byte matches.
            {
                if (PATTERN_AS_ARRAY[i] == "?") //if our pattern character is a wildcard skip.
                    continue;
                var BYTE_AS_HEX_STR = BYTE_BASE_PTR[i].ToString("X").ToUpper(); //Convert our byte to a base16 string variant and cast it to upper characters.
                if (BYTE_AS_HEX_STR != PATTERN_AS_ARRAY[i]) //Compare the two values, if they don't match, its not our signature.
                    return false;
            }
            return true;
        }
    }
}
