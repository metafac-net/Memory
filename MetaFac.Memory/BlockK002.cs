using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 1024 * 002)]
    public struct BlockK002
    {
        [FieldOffset(0)]
        public BlockK001 A;
        [FieldOffset(1024 * 001)]
        public BlockK001 B;
    }
}
