using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 128)]
    public struct BlockB128
    {
        [FieldOffset(0)]
        public BlockB064 A;
        [FieldOffset(64)]
        public BlockB064 B;
    }
}
