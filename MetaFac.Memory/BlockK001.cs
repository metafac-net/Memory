using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 1024 * 1)]
    public struct BlockK001
    {
        [FieldOffset(0)]
        public BlockB512 A;
        [FieldOffset(512)]
        public BlockB512 B;
    }
}
