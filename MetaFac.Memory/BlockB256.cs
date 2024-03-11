using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 256)]
    public struct BlockB256
    {
        [FieldOffset(0)]
        public BlockB128 A;
        [FieldOffset(128)]
        public BlockB128 B;
    }
}
