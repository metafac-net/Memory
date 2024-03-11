using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct BlockB016
    {
        [FieldOffset(0)]
        public BlockB008 A;
        [FieldOffset(8)]
        public BlockB008 B;
    }
}
