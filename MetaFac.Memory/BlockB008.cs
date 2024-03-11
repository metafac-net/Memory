using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct BlockB008
    {
        [FieldOffset(0)]
        public BlockB004 A;
        [FieldOffset(4)]
        public BlockB004 B;
    }
}
