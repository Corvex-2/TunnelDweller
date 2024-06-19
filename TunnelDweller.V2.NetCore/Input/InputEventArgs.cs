using System;

namespace TunnelDweller.NetCore.Input
{
    public class InputEventArgs : EventArgs
    {
        public readonly int vkCode;
        public readonly int skCode;
        public readonly bool State;
        public bool Suppress;

        public InputEventArgs()
        {
        }
        public InputEventArgs(int vkCode, int skCode, bool state)
        {
            this.vkCode = vkCode;
            this.skCode = skCode;
            State = state;
            Suppress = false;
        }
    }
}
