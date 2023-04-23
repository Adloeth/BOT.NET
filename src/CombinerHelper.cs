using System.Collections.Generic;

namespace BOT
{
    /// <summary>
    /// Combine severals sets of bytes to a primitive. Note that if your data has a variable size or it's size cannot be mapped with one of the primitives, use the blob instead.
    /// <br></br>Note: Using these helper classes makes your life easier but will make the (de)serialization slower ! 
    /// Common combiner/extractors are in WriterUtils/ReaderUtils and it should be better to implement your own for any custom class you are using a lot.
    /// </summary>
    /// <example>
    /// This shows how to combine 3 bytes (x, y, z) in a tribyte (t) such that:
    /// <br></br>x = 1111 1111 (256), y = 0000 1010 (10) and z = 1011 0100 (180).
    /// <br></br>t = 1111 1111   0000 1010   1011 0100 (16714420)
    /// <code>
    ///     //         Use the capacity (in bytes) if you already know how much bytes you are going to combine !
    ///     //                                                 V
    ///     PrimitiveCombiner combiner = new PrimitiveCombiner(3);
    ///     Tribyte t = combiner.Combine(256).Combine(10).Combine(180).ToTribyte();
    ///     //or, when only using bytes.
    ///     Tribyte t = combiner.Combine(256, 10, 180).ToTribyte();
    /// </code>
    /// </example>
    public class PrimitiveCombiner
    {
        private List<byte> data;

        public PrimitiveCombiner()
        {
            data = new List<byte>();
        }

        public PrimitiveCombiner(int capacity)
        {
            data = new List<byte>(capacity);
        }

        #region COMBINERS

        /// <summary>Checks for endianness before combining.</summary>
        public PrimitiveCombiner CheckedCombine(byte[] bytes)
        {
            data.AddRange(Standard.IsSystemLittleEndian() ? bytes : bytes.Reverse());
            return this;
        }

        // I wished C# had macros or something like template specialisation...

        public PrimitiveCombiner Combine(byte val)
        { data.Add(val); return this; }
        /// <summary>Warning: the array is added as is, it doesn't take endianness into account. Use CheckedCombine if you want to correct the endianness of a variable before combining it.</summary>
        public PrimitiveCombiner Combine(params byte[] val)
        { data.AddRange(val); return this; }

        public PrimitiveCombiner Combine(sbyte val)
            => Combine((byte)val);
        public PrimitiveCombiner Combine(params sbyte[] val)
            => Combine((byte[])(Array)val);


        public PrimitiveCombiner Combine(ushort val)
            => CheckedCombine(BitConverter.GetBytes(val));
        public PrimitiveCombiner Combine(params ushort[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }

        public PrimitiveCombiner Combine(short val)
            => Combine((ushort)val);
        public PrimitiveCombiner Combine(params short[] val)
            => Combine((ushort[])(Array)val);


        public PrimitiveCombiner Combine(Tribyte val)
            => CheckedCombine(val.GetBytes());
        public PrimitiveCombiner Combine(params Tribyte[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }


        public PrimitiveCombiner Combine(uint val)
            => CheckedCombine(BitConverter.GetBytes(val));
        public PrimitiveCombiner Combine(params uint[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }

        public PrimitiveCombiner Combine(int val)
            => Combine((uint)val);
        public PrimitiveCombiner Combine(params int[] val)
            => Combine((uint[])(Array)val);


        public PrimitiveCombiner Combine(Pentabyte val)
            => CheckedCombine(val.GetBytes());
        public PrimitiveCombiner Combine(params Pentabyte[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }


        public PrimitiveCombiner Combine(Hexabyte val)
            => CheckedCombine(val.GetBytes());
        public PrimitiveCombiner Combine(params Hexabyte[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }


        public PrimitiveCombiner Combine(Heptabyte val)
            => CheckedCombine(val.GetBytes());
        public PrimitiveCombiner Combine(params Heptabyte[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }


        public PrimitiveCombiner Combine(ulong val)
            => CheckedCombine(BitConverter.GetBytes(val));
        public PrimitiveCombiner Combine(params ulong[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }

        public PrimitiveCombiner Combine(long val)
            => Combine((ulong)val);
        public PrimitiveCombiner Combine(params long[] val)
            => Combine((ulong[])(Array)val);


        public PrimitiveCombiner Combine(LargeInt val)
            => CheckedCombine(val.GetBytes());
        public PrimitiveCombiner Combine(params LargeInt[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }


        public PrimitiveCombiner Combine(BigInt val)
            => CheckedCombine(val.GetBytes());
        public PrimitiveCombiner Combine(params BigInt[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }


        public PrimitiveCombiner Combine(GreatInt val)
            => CheckedCombine(val.GetBytes());
        public PrimitiveCombiner Combine(params GreatInt[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }


        public PrimitiveCombiner Combine(HugeInt val)
            => CheckedCombine(val.GetBytes());
        public PrimitiveCombiner Combine(params HugeInt[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }


        public PrimitiveCombiner Combine(GiantInt val)
            => CheckedCombine(val.GetBytes());
        public PrimitiveCombiner Combine(params GiantInt[] val)
        { for(int i = 0; i < val.Length; i++) Combine(val[i]); return this; }

        #endregion

        #region PRIMITIVE CONVERTERS

        public byte ToByte() 
        {
            CheckSize(1, "byte");
            if(data.Count <= 0)
                return 0;
            return data[0];
        }

        public ushort ToShort()
        {
            CheckSize(2, "short");
            return BitConverter.ToUInt16(InternalConvertTo(2));
        }

        public Tribyte ToTribyte()
        {
            CheckSize(3, "tribyte");
            return new Tribyte(InternalConvertTo(3));
        }

        public uint ToInt()
        {
            CheckSize(4, "int");
            return BitConverter.ToUInt32(InternalConvertTo(4));
        }

        public Pentabyte ToPentabyte()
        {
            CheckSize(5, "pentabyte");
            return new Pentabyte(InternalConvertTo(5));
        }

        public Hexabyte ToHexabyte()
        {
            CheckSize(6, "hexabyte");
            return new Hexabyte(InternalConvertTo(6));
        }

        public Heptabyte ToHeptabyte()
        {
            CheckSize(7, "heptabyte");
            return new Heptabyte(InternalConvertTo(7));
        }

        public ulong ToLong()
        {
            CheckSize(8, "long");
            return BitConverter.ToUInt64(InternalConvertTo(8));
        }

        public LargeInt ToLargeInt()
        {
            CheckSize(12, "large int");
            return new LargeInt(InternalConvertTo(12));
        }

        public BigInt ToBigInt()
        {
            CheckSize(16, "big int");
            return new BigInt(InternalConvertTo(16));
        }

        public GreatInt ToGreatInt()
        {
            CheckSize(24, "great int");
            return new GreatInt(InternalConvertTo(24));
        }

        public HugeInt ToHugeInt()
        {
            CheckSize(32, "huge int");
            return new HugeInt(InternalConvertTo(32));
        }

        public GiantInt ToGiantInt()
        {
            CheckSize(64, "giant int");
            return new GiantInt(InternalConvertTo(64));
        }

        #endregion
    
        private void CheckSize(int size, string varName)
        {
            if(data.Count < size)
                throw new Exception(string.Concat("Cannot convert combiner to ", varName, " (", size ,"), the current combiner size is ", data.Count, " bytes."));
        }

        private byte[] InternalConvertTo(int size)
        {
            byte[] result = new byte[size];
            byte[] dataArray = Standard.IsSystemLittleEndian() ? data.ToArray() : data.Reverse<byte>().ToArray();
            Array.Copy(dataArray, 0, result, 0, size);
            return result;
        }
    
    }

    /// <summary>
    /// Recover severals sets of bytes from a primitive. If you used the PrimitiveCombiner, be careful to extract data in the same order you combined them.
    /// <br></br>Note: Using these helper classes makes your life easier but will make the (de)serialization slower ! 
    /// Common combiner/extractors are in WriterUtils/ReaderUtils and it should be better to implement your own for any custom class you are using a lot.
    /// </summary>
    /// <example>
    /// This shows how to extract 3 bytes (x, y, z) from a tribyte (t) such that:
    /// <br></br>t = 1111 1111   0000 1010   1011 0100 (16714420)
    /// <br></br>x = 1111 1111 (256), y = 0000 1010 (10) and z = 1011 0100 (180).
    /// <code>
    ///     PrimitiveExtractor extractor = new PrimitiveExtractor(new Tribyte(16714420));
    ///     extractor.Extract(out byte x).Extract(out byte y).Extract(out byte z);
    /// </code>
    /// </example>
    public class PrimitiveExtractor
    {
        private byte[] data;
        private int current;

        #region CONSTRUCTORS

        public PrimitiveExtractor(byte[] data) 
        {
            if(Standard.IsSystemLittleEndian())
                this.data = data;
            else 
                this.data = data.Reverse().ToArray(); 
        }

        public PrimitiveExtractor(byte data) : this(new byte[1] { data }) { }
        public PrimitiveExtractor(sbyte data) : this(new byte[1] { (byte)data }) { }

        public PrimitiveExtractor(ushort data) : this(BitConverter.GetBytes(data)) { }
        public PrimitiveExtractor(short data) : this(BitConverter.GetBytes(data)) { }

        public PrimitiveExtractor(Tribyte data) : this(data.GetBytes()) { }

        public PrimitiveExtractor(uint data) : this(BitConverter.GetBytes(data)) { }
        public PrimitiveExtractor(int data) : this(BitConverter.GetBytes(data)) { }

        public PrimitiveExtractor(Pentabyte data) : this(data.GetBytes()) { }

        public PrimitiveExtractor(Hexabyte data) : this(data.GetBytes()) { }

        public PrimitiveExtractor(Heptabyte data) : this(data.GetBytes()) { }

        public PrimitiveExtractor(ulong data) : this(BitConverter.GetBytes(data)) { }
        public PrimitiveExtractor(long data) : this(BitConverter.GetBytes(data)) { }

        public PrimitiveExtractor(LargeInt data) : this(data.GetBytes()) { }

        public PrimitiveExtractor(BigInt data) : this(data.GetBytes()) { }

        public PrimitiveExtractor(GreatInt data) : this(data.GetBytes()) { }

        public PrimitiveExtractor(HugeInt data) : this(data.GetBytes()) { }

        public PrimitiveExtractor(GiantInt data) : this(data.GetBytes()) { }

        #endregion

        #region EXTRACTORS

        public byte[] CheckedExtract(int size)
        {
            byte[] bytes = new byte[size];
            Array.Copy(data, current, bytes, 0, size);
            if(!Standard.IsSystemLittleEndian())
                bytes = bytes.Reverse().ToArray();
            current += size;
            return bytes;
        }

        public PrimitiveExtractor CheckedExtract(out byte[] bytes, int size)
        {
            bytes = CheckedExtract(size);
            return this;
        }

        public PrimitiveExtractor Extract(out byte[] val, int size)
        {
            val = new byte[size];
            Array.Copy(data, current, val, 0, size);
            current += size;
            return this;
        }

        public PrimitiveExtractor Extract(out byte val)
        { val = data[current++]; return this; }

        public PrimitiveExtractor Extract(out sbyte val)
        { Extract(out byte tmp); val = (sbyte)tmp; return this; }

        public PrimitiveExtractor Extract(out ushort val)
        { val = BitConverter.ToUInt16(CheckedExtract(2)); return this; }

        public PrimitiveExtractor Extract(out short val)
        { Extract(out ushort tmp); val = (short)tmp; return this; }

        public PrimitiveExtractor Extract(out Tribyte val)
        { val = new Tribyte(CheckedExtract(3)); return this; }

        public PrimitiveExtractor Extract(out uint val)
        { val = BitConverter.ToUInt32(CheckedExtract(4)); return this; }

        public PrimitiveExtractor Extract(out int val)
        { Extract(out uint tmp); val = (int)tmp; return this; }

        public PrimitiveExtractor Extract(out Pentabyte val)
        { val = new Pentabyte(CheckedExtract(5)); return this; }

        public PrimitiveExtractor Extract(out Hexabyte val)
        { val = new Hexabyte(CheckedExtract(6)); return this; }

        public PrimitiveExtractor Extract(out Heptabyte val)
        { val = new Heptabyte(CheckedExtract(7)); return this; }

        public PrimitiveExtractor Extract(out ulong val)
        { val = BitConverter.ToUInt64(CheckedExtract(8)); return this; }

        public PrimitiveExtractor Extract(out long val)
        { Extract(out ulong tmp); val = (long)tmp; return this; }

        public PrimitiveExtractor Extract(out LargeInt val)
        { val = new LargeInt(CheckedExtract(12)); return this; }

        public PrimitiveExtractor Extract(out BigInt val)
        { val = new BigInt(CheckedExtract(16)); return this; }

        public PrimitiveExtractor Extract(out GreatInt val)
        { val = new GreatInt(CheckedExtract(24)); return this; }

        public PrimitiveExtractor Extract(out HugeInt val)
        { val = new HugeInt(CheckedExtract(32)); return this; }

        public PrimitiveExtractor Extract(out GiantInt val)
        { val = new GiantInt(CheckedExtract(64)); return this; }

        #endregion
    }
}