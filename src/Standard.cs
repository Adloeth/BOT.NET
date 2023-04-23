using System.Runtime.InteropServices;

namespace BOT
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
        Object    = 0x00,
        Bool      = 0x01,
        Byte      = 0x02,
        Short     = 0x03,
        Tribyte   = 0x04,
        Int       = 0x05,
        Pentabyte = 0x06,
        Hexabyte  = 0x07,
        Heptabyte = 0x08,
        Long      = 0x09,
        Large     = 0x0A,
        Big       = 0x0B,
        Great     = 0x0C,
        Huge      = 0x0D,
        Giant     = 0x0E,

        Tiny_Array   = 0x0F,
	    Small_Array  = 0x10,
        Short_Array  = 0x11,
    	Medium_Array = 0x12,
    	Large_Array  = 0x13,
    	Big_Array    = 0x14,
    	Great_Array  = 0x15,
    	Huge_Array   = 0x16,

        Tiny_Dict   = 0x17,
	    Small_Dict  = 0x18,
        Short_Dict  = 0x19,
    	Medium_Dict = 0x1A,
    	Large_Dict  = 0x1B,
    	Big_Dict    = 0x1C,
    	Great_Dict  = 0x1D,
    	Huge_Dict   = 0x1E,

        Tiny_Blob   = 0x1F,
	    Small_Blob  = 0x20,
        Short_Blob  = 0x21,
    	Medium_Blob = 0x22,
    	Large_Blob  = 0x23,
    	Big_Blob    = 0x24,
    	Great_Blob  = 0x25,
    	Huge_Blob   = 0x26,

        Object_End = 0xFF
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

    public enum Collection : byte
    {
        Array = Type.Tiny_Array - 1,
        Dictionary = Type.Tiny_Dict - 1,
        Blob = Type.Tiny_Blob - 1
    }

    public static class Standard
    {
        public static bool IsSystemLittleEndian() => botIsLittleEndian();
        public static bool IsDefault(ISerializable obj) => obj == null || obj.IsDefault;

        [DllImport("libBOT.so")] static private extern bool botIsLittleEndian();

        public static Type GetCollectionType(Collection collection, Size size, ulong count)
        {
            if(size == Size.Auto)
                size = GetCollectionSizeFromCount(count);

            if(size > Size.Huge)
                throw new Exception("Invalid size for writting an array !");

            return (Type)((byte)collection + (byte)size);
        }

        public static Size GetCollectionSizeFromCount(ulong count)
        {
            if(count == 0)
                throw new Exception("Count is 0. The collection should not be serialized !");

            count--;

                 if(count < 256)                   return Size.Tiny  ;
            else if(count < 65536)                 return Size.Small ;
            else if(count < 16777216)              return Size.Short ;
            else if(count < 4294967296)            return Size.Medium;
            else if(count < 1099511627777)         return Size.Large ;
            else if(count < 281474976710656)       return Size.Big   ;
            else if(count < 72057594037927936)     return Size.Great ;
            else if(count <= 18446744073709551615) return Size.Huge  ;

            throw new Exception("Apparently, you come from a future where a ulong is no longer a 64-bits integers. Good luck !");
        }

        public static Size SizeFromCollection(Type type)
        {
            if(!IsCollection(type))
                throw new Exception(string.Concat("Invalid type, need a collection, passed a '", type, "'"));

            if(type == Type.Tiny_Array   || type == Type.Tiny_Dict   || type == Type.Tiny_Blob  ) return Size.Tiny;
            if(type == Type.Small_Array  || type == Type.Small_Dict  || type == Type.Small_Blob ) return Size.Small;
            if(type == Type.Short_Array  || type == Type.Short_Dict  || type == Type.Short_Blob ) return Size.Short;
            if(type == Type.Medium_Array || type == Type.Medium_Dict || type == Type.Medium_Blob) return Size.Medium;
            if(type == Type.Large_Array  || type == Type.Large_Dict  || type == Type.Large_Blob ) return Size.Large;
            if(type == Type.Big_Array    || type == Type.Big_Dict    || type == Type.Big_Blob   ) return Size.Big;
            if(type == Type.Great_Array  || type == Type.Great_Dict  || type == Type.Great_Blob ) return Size.Great;
            if(type == Type.Huge_Array   || type == Type.Huge_Dict   || type == Type.Huge_Blob  ) return Size.Huge;

            throw new Exception(string.Concat("Invalid size from type '", type, "'"));
        }

        public static Type CSTypeToPrimitive(System.Type type)
        {
            if(typeof(IPrimitive).IsAssignableFrom(type))       return (Type)type.GetMethod("AsPrimitive").Invoke(null, null);
            if(typeof(ISerializable).IsAssignableFrom(type))    return Type.Object;
            if(type == typeof(bool))                            return Type.Bool;
            if(type == typeof(byte) || type == typeof(sbyte))   return Type.Byte;
            if(type == typeof(ushort) || type == typeof(short)) return Type.Short;
            if(type == typeof(Tribyte))                         return Type.Tribyte;
            if(type == typeof(uint) || type == typeof(int))     return Type.Int;
            if(type == typeof(float))                           return Type.Int;
            if(type == typeof(Pentabyte))                       return Type.Pentabyte;
            if(type == typeof(Hexabyte))                        return Type.Hexabyte;
            if(type == typeof(Heptabyte))                       return Type.Heptabyte;
            if(type == typeof(ulong) || type == typeof(long))   return Type.Long;
            if(type == typeof(double))                          return Type.Long;
            if(type == typeof(LargeInt))                        return Type.Large;
            if(type == typeof(BigInt))                          return Type.Big;
            if(type == typeof(GreatInt))                        return Type.Great;
            if(type == typeof(HugeInt))                         return Type.Huge;
            if(type == typeof(GiantInt))                        return Type.Giant;

            throw new Exception(string.Concat("Cannot serialize ", type.FullName));
        }

        public static bool IsObject(Type type)
            => type == Type.Object;

        public static bool IsPrimitive(Type type)
            => type >= Type.Bool && type <= Type.Giant;

        public static bool IsCollection(Type type)
            => type >= Type.Tiny_Array && type <= Type.Huge_Blob;

        public static bool IsArray(Type type)
            => type >= Type.Tiny_Array && type <= Type.Huge_Array;

        public static bool IsDictionary(Type type)
            => type >= Type.Tiny_Dict && type <= Type.Huge_Dict;

        public static bool IsBlob(Type type)
            => type >= Type.Tiny_Blob && type <= Type.Huge_Blob;

        public static bool IsSpecial(Type type)
            => type == Type.Object_End;
    }
}