using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Globalization;
using TunnelDweller.NetCore.Extensions;
using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Moduling;
using TunnelDweller.NetCore.Input;
using TunnelDweller.NetCore.Windowing;
using TunnelDweller.NetCore.Game;
using TunnelDweller.NetCore.API;
using System.Diagnostics;
using TunnelDweller.NetCore.Threading;

namespace TunnelDweller.NetCore.Rendering
{
    //metro.exe+d07748

    public static class Renderer
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void DrawCallback_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void PreDrawCallback_t();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool RegisterDrawCallback_t(DrawCallback_t callback);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool UnregisterDrawCallback_t(DrawCallback_t callback);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool RegisterPreDrawCallback_t(PreDrawCallback_t callback);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool UnregisterPreDrawCallback_t(PreDrawCallback_t callback);

        internal static DrawCallback_t smCallback;
        internal static PreDrawCallback_t smPreCallback;
        internal static RegisterDrawCallback_t smRegisterDrawCallback;
        internal static UnregisterDrawCallback_t smUnregisterDrawCallback;
        internal static RegisterPreDrawCallback_t smRegisterPreDrawCallback;
        internal static UnregisterPreDrawCallback_t smUnregisterPreDrawCallback;

        public static event EventHandler OnRender;

        internal static void Initialize()
        {
            if (smCallback != null || smPreCallback != null)
                return;

            smPreCallback = PreCallback;
            smCallback = Callback;

            if (smPreCallback != null && smRegisterPreDrawCallback != null)
                smRegisterPreDrawCallback(smPreCallback);
            if (smCallback != null && smRegisterDrawCallback != null)
                smRegisterDrawCallback(smCallback);

            InputManager.InputChanged += InputManager_InputChanged;
        }

        private static void InputManager_InputChanged(object sender, InputEventArgs e)
        {
            if (Window.Windows.Any(x => x.Show == true) || Popup.Popups.Any(x => x.Active == true))
                e.Suppress = true;
        }

        internal static void Callback()
        {
            Font.Default().PushFont();
            ImGui.ImDrawText($"[V2] TunnelDweller - Build: {TechnicalMetroApi.RELEASESTREAM}", 12, 12, 12, 255, 255, 255, 177);
            Font.Default().PopFont();

            if (Window.Windows.Any(x => x.Show == true) || Popup.Popups.Any(x => x.Active == true))
                ImGui.SetCursorVisibility(true);
            else
                ImGui.SetCursorVisibility(false);

            OnRender?.Invoke(null, null);

            Window.DrawAll();
            Popup.DrawAll();
        }

        internal static void PreCallback()
        {
            Font.UploadFonts();
        }

        internal static void Uninitialize()
        {
            smUnregisterDrawCallback(smCallback);
            smUnregisterPreDrawCallback(smPreCallback);
        }
    }
}
