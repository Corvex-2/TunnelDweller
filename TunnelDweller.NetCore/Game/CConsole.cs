using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TunnelDweller.NetCore.Input;

namespace TunnelDweller.NetCore.Game
{
    public static class CConsole
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal delegate bool CommandHandler_t([MarshalAs(UnmanagedType.LPStr)] string command);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool RegisterCommandHandler_t(CommandHandler_t callback);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool UnregisterCommandHandler_t(CommandHandler_t callback);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Show_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void Execute_t([MarshalAs(UnmanagedType.LPStr)] string command);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void ExecuteDeferred_t([MarshalAs(UnmanagedType.LPStr)] string command);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        internal delegate string GetTextBuffer_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GetInstance_t();

        internal static GetInstance_t smGetInstance;
        internal static GetTextBuffer_t smGetTextBuffer;
        internal static ExecuteDeferred_t smExecuteDeferred;
        internal static Execute_t smExecute;
        internal static Show_t smShow;
        internal static CommandHandler_t smCallback;
        internal static RegisterCommandHandler_t smRegisterCommandHandler;
        internal static UnregisterCommandHandler_t smUnregisterCommandHandler;

        public static event EventHandler<CommandEventArgs> OnCommand;

        public static int ToggleKey { get; set; } = 41;

        internal static void Initialize()
        {
            if (smCallback != null)
                return;
            smCallback = Callback;
            if (smCallback != null && smRegisterCommandHandler != null)
                smRegisterCommandHandler(smCallback);

            InputManager.InputChanged += InputManager_InputChanged;
        }

        private static void InputManager_InputChanged(object sender, InputEventArgs e)
        {
            if(e.State == false && e.skCode == ToggleKey)
            {
                Show();
            }
        }

        internal static bool Callback(string command)
        {
            var args = new CommandEventArgs(command, false);
            Console.WriteLine(command);
            OnCommand?.Invoke(null, args);
            return args.Suppress;
        }

        internal static void Uninitialize()
        {
            smUnregisterCommandHandler(smCallback);
        }


        public static void Show()
        {
            if (smShow != null)
                smShow();
        }

        public static void Execute(string command)
        {
            if(smExecute != null)
                smExecute(command);
        }

        public static void ExecuteDeferred(string command)
        {
            if(smExecuteDeferred != null)
                smExecuteDeferred(command);
        }

        public static string GetTextBuffer()
        {
            if (smGetTextBuffer != null)
                return smGetTextBuffer();
            return string.Empty;
        }

        public static IntPtr GetInstance()
        {
            if(smGetInstance != null)
                return smGetInstance();
            return IntPtr.Zero;
        }
    }
}
