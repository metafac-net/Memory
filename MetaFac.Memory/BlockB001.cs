using System.Runtime.InteropServices;

namespace MetaFac.Memory
{
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public struct BlockB001
    {
        [FieldOffset(0)]
        public sbyte Value;
        [FieldOffset(0)]
        public byte UValue;
    }
}
