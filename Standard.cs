using System.Runtime.InteropServices;

namespace FFF
{
    [Flags] public enum Options : byte
    {
        Default = 0,
        FixedNameLength = 0b00000001,
        FullWidthTiny = 0b00000010,
        FullWidthSmall = 0b00000100,
        FullWidthMedium = 0b00000110,
        FullWidthLarge = 0b00001000,
        FullWidthBig = 0b00001010,
        FullWidthGreat = 0b00001110,
    }

    public enum Type : byte
    {
        Object = 0,
        Bool = 1,
        Byte = 2,
        Short = 3,
        Tribyte = 4,
        Int = 5,
        Pentabyte = 6,
        Hexabyte = 7,
        Heptabyte = 8,
        Long = 9,
        Large = 10,
        Big = 11,
        Great = 12,
        Huge = 13,
        Giant = 14
    }

    public enum Size : byte
    {
        Auto = 0,
        Tiny = 1,
        Small = 2,
        Short = 3,
        Medium = 4,
        Large = 5,
        Big = 6,
        Great = 7,
        Huge = 8
    }

    public static class Standard
    {
        public static bool IsSystemLittleEndian() => fffIsLittleEndian();
        public static bool IsDefault(ISerializable obj) => obj == null || obj.IsDefault;

        [DllImport("libFFF.so")] static private extern bool fffIsLittleEndian();

    }
}