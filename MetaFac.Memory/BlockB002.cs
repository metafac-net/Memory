using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct BlockB002
    {
        [FieldOffset(0)]
        public BlockB001 A;
        [FieldOffset(1)]
        public BlockB001 B;
    }
}
