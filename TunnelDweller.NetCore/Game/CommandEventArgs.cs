using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.Game
{
    public class CommandEventArgs : EventArgs
    {
        public readonly string Command;
        public bool Suppress;

        public CommandEventArgs() { }
        public CommandEventArgs(string command, bool suppress)
        {
            Command = command;
            Suppress = suppress;
        }
    }
}
