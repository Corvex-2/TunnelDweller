namespace TunnelDweller.Memory
{
    public class Signature
    {
        internal string Mask { get; set; }
        internal int Offset { get; set; }
        internal bool Scan { get; set; } = true;

        public Signature() { }
        public Signature(string Mask, int Offset)
        {
            this.Mask = Mask;
            this.Offset = Offset;
        }
    }
}
