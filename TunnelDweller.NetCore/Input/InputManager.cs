using System;
using System.Globalization;
using System.Runtime.InteropServices;
using TunnelDweller.NetCore.Extensions;

namespace TunnelDweller.NetCore.Input
{
    public static class InputManager
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool InputCallback_t(int vkCode, int skCode, bool State);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool RegisterInputCallback_t(InputCallback_t callback);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool UnregisterInputCallback_t(InputCallback_t callback);

        internal static InputCallback_t smCallback;
        internal static RegisterInputCallback_t smRegisterInputCallback;
        internal static UnregisterInputCallback_t smUnregisterInputCallback;

        internal static void Initialize()
        {
            if (smCallback != null)
                return;
            smCallback = Callback;
            if (smCallback != null && smRegisterInputCallback != null)
                smRegisterInputCallback(smCallback);
        }

        public static event EventHandler<InputEventArgs> InputChanged;

        internal static bool Callback(int vkCode, int skCode, bool State)
        {
            var args = new InputEventArgs(vkCode, skCode, State);
            InputChanged?.Invoke(null, args);
            return args.Suppress;
        }

        internal static void Uninitialize()
        {
            smUnregisterInputCallback(smCallback);
        }
    }
}
