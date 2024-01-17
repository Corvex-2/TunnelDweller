using System.Collections.Generic;

namespace TunnelDweller.Memory
{ 
    public static class UnmanagedCLR
    {
        public static void Pin(object Object)
        {
            if (!_pinnedObjects.Contains(Object))
                _pinnedObjects.Add(Object);
        }
        public static void Unpin(object Object)
        {
            if (_pinnedObjects.Contains(Object))
                _pinnedObjects.Remove(Object);
        }

        private static List<object> _pinnedObjects { get; set; } = new List<object>();
    }
}
