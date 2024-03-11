using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct BlockB032
    {
        [FieldOffset(0)]
        public BlockB016 A;
        [FieldOffset(16)]
        public BlockB016 B;
    }
}
