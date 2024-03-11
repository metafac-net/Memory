using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 1024 * 004)]
    public struct BlockK004
    {
        [FieldOffset(0)]
        public BlockK002 A;
        [FieldOffset(1024 * 002)]
        public BlockK002 B;
    }
}
