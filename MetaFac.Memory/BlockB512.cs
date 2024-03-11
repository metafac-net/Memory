using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 512)]
    public struct BlockB512
    {
        [FieldOffset(0)]
        public BlockB256 A;
        [FieldOffset(256)]
        public BlockB256 B;
    }
}
