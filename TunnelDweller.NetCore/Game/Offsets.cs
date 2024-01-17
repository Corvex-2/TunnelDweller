using TunnelDweller.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.Game
{
    public static class Offsets
    {
        public static IntPtr TARGET_FPS
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0xCF2708;
            }
        }
        public static IntPtr LEVEL_ID
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0xD23810;
            }
        }
        public static IntPtr LEVEL_INSTANCE
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0xD236E0;
            }
        }
        public static IntPtr IS_LOADING
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0xCFAC30;
            }
        }
        public static IntPtr PLAYER_Z
        {
            get
            {
                return GetMultilevelPointer(Process.GetCurrentProcess().MainModule.BaseAddress + 0xd01ea8, 0x30, 0x4f8, 0x08, 0xc8, 0xf0);
            }
        }
        public static IntPtr PLAYER_Y
        {
            get
            {
                return GetMultilevelPointer(Process.GetCurrentProcess().MainModule.BaseAddress + 0xd01ea8, 0x30, 0x4f8, 0x08, 0xc8, 0xec);
            }
        }
        public static IntPtr PLAYER_X
        {
            get
            {
                return GetMultilevelPointer(Process.GetCurrentProcess().MainModule.BaseAddress + 0xd01ea8, 0x30, 0x4f8, 0x08, 0xc8, 0xe8);
            }
        }
        public static IntPtr PLAYER_RX
        {
            get
            {
                return GetMultilevelPointer(Process.GetCurrentProcess().MainModule.BaseAddress + 0xD01ea8, 0x38, 0x650);
            }
        }
        public static IntPtr PLAYER_RY
        {
            get
            {
                return GetMultilevelPointer(Process.GetCurrentProcess().MainModule.BaseAddress + 0xD01ea8, 0x38, 0x654);
            }
        }
        public static IntPtr PLAYER_RZ
        {
            get
            {
                return GetMultilevelPointer(Process.GetCurrentProcess().MainModule.BaseAddress + 0xD01ea8, 0x38, 0x658);
            }
        }
        public static IntPtr VIEWDISTANCE
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0xce711c;
            }
        }
        public static IntPtr FIELDOFVIEW
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0xCE710C;
            }
        }
        public static IntPtr RESOLUTION_WIDTH
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0xD359D0;
            }
        }
        public static IntPtr RESOLUTION_HEIGHT
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0xD359D4;
            }
        }
        public static IntPtr RESULTION_ASPECTRATIO
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0xD22EBC;
            }
        }
        public static IntPtr WINDOW_TIME
        {
            get
            {
                return GetMultilevelPointer(Process.GetCurrentProcess().MainModule.BaseAddress + 0xD236E0 + 0x140, 0);
            }
        }
        public static IntPtr WINDOW_TIME_FOCUS
        {
            get
            {
                return GetMultilevelPointer(Process.GetCurrentProcess().MainModule.BaseAddress + 0xD236E0 + 140, 0x10, 0x4c);
            }
        }
        public static IntPtr GAME_TIME
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0xD236E0 + 0x328;
            }
        }
        public static IntPtr FNDRAW_GWORLD
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 677710;
            }
        }
        public static IntPtr FNLOAD_LEVEL
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0x80FBB0;
            }
        }
        public static IntPtr FNAUTO_SAVE
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0x42E0;
            }
        }
        public static IntPtr FNFIRE_PROJECTILE_WEAPON
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.BaseAddress + 0x2bf0c0;
            }
        }

        private static IntPtr GetMultilevelPointer(IntPtr Base, params int[] Levels)
        {
            var read = MemoryManager.Read<IntPtr>(Base);

            for(int i = 0; i < Levels.Length - 1; i++) 
            {
                read = MemoryManager.Read<IntPtr>(read + Levels[i]);
            }
            return read + Levels.Last();
        }
    }
}
