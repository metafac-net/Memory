using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 1024 * 008)]
    public struct BlockK008
    {
        [FieldOffset(0)]
        public BlockK004 A;
        [FieldOffset(1024 * 004)]
        public BlockK004 B;
    }
}
