using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct BlockB004
    {
        [FieldOffset(0)]
        public BlockB002 A;
        [FieldOffset(2)]
        public BlockB002 B;
    }
}
