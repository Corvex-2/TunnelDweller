using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Extensions;

namespace TunnelDweller.NetCore.MinHook
{
    internal enum MH_STATUS : int
    {
        MH_UNKNOWN = -1,
        MH_OK = 0,
        MH_ERROR_ALREADY_INITIALIZED,
        MH_ERROR_NOT_INITIALIZED,
        MH_ERROR_ALREADY_CREATED,
        MH_ERROR_NOT_CREATED,
        MH_ERROR_ENABLED,
        MH_ERROR_DISABLED,
        MH_ERROR_NOT_EXECUTABLE,
        MH_ERROR_UNSUPPORTED_FUNCTION,
        MH_ERROR_MEMORY_ALLOC,
        MH_ERROR_MEMORY_PROTECT,
        MH_ERROR_MODULE_NOT_FOUND,
        MH_ERROR_FUNCTION_NOT_FOUND
    }

    internal static class MH
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate MH_STATUS MH_EnableHook_t(IntPtr pTarget);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate MH_STATUS MH_DisableHook_t( IntPtr pTarget);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate MH_STATUS MH_CreateHook_t(IntPtr pTarget, IntPtr pDetour, out IntPtr ppOriginal);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate MH_STATUS MH_RemoveHook_t(IntPtr pTarget);

        internal static MH_EnableHook_t smMH_EnableHook;
        internal static MH_DisableHook_t smMH_DisableHook;
        internal static MH_CreateHook_t smMH_CreateHook;
        internal static MH_RemoveHook_t smMH_RemoveHook;
    }
}
