using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct BlockB064
    {
        [FieldOffset(0)]
        public BlockB032 A;
        [FieldOffset(32)]
        public BlockB032 B;
    }
}
