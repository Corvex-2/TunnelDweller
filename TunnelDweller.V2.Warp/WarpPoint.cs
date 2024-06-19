using TunnelDweller.NetCore.DearImgui;
using TunnelDweller.NetCore.Game;

namespace TunnelDweller.Warp
{
    internal class WarpPoint
    {
        internal string Name { get; set; }
        internal vec3_t Position { get; set; }
        internal vec3_t Rotation { get; set; }

        public WarpPoint(string Name, vec3_t Position, vec3_t Rotation) 
        {
            this.Name = Name;
            this.Position = Position;
            this.Rotation = Rotation;
        }

        internal void Goto()
        {
            Variables.Angles = Rotation;
            Variables.Position = Position;
        }

        public override string ToString()
        {
            return $"{Name} - {Position}";
        }
    }
}
