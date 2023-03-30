using System.Runtime.CompilerServices;

namespace FFF
{
    public static class WriterUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static byte[] Combine(byte[][] data, int dimensions, byte varSize)
        {
            byte[] total = new byte[varSize * dimensions];
            int k = 0;
            for(int i = 0; i < dimensions; i++)
                if(Standard.IsSystemLittleEndian())
                {
                    for(int j = 0; j < varSize; j++)
                        total[k++] = data[i][j];
                }
                else
                {
                    for(int j = varSize - 1; j >= 0; j--)
                        total[k++] = data[i][j];
                }

            return total;
        }


        /// <summary>Write an RGB color as a 24-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, byte r, byte g, byte b)
            => writer.Write(fieldName, new Tribyte(r, g, b));

        /// <summary>Write an RGBA color as a 32-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, byte r, byte g, byte b, byte a)
            => writer.Write(fieldName, BitConverter.ToUInt32(new byte[4] { r, g, b, a }));

        /// <summary>Write a deep RGB color as a 48-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, ushort r, ushort g, ushort b)
        {
            byte[][] bytes = new byte[3][] { BitConverter.GetBytes(r), BitConverter.GetBytes(g), BitConverter.GetBytes(b) };
            writer.Write(fieldName, new Hexabyte(Combine(bytes, 3, 2)));
        }

        /// <summary>Write a deep RGBA color as a 64-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, ushort r, ushort g, ushort b, ushort a)
        {
            byte[][] bytes = new byte[4][] { BitConverter.GetBytes(r), BitConverter.GetBytes(g), BitConverter.GetBytes(b), BitConverter.GetBytes(a) };
            writer.Write(fieldName, BitConverter.ToUInt64(Combine(bytes, 4, 2)));
        }

        /// <summary>Write a GUID (or UUID) as a 128-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, Guid guid)
            => writer.Write(fieldName, new BigInt(guid.ToByteArray()));

        /// <summary>Write a half as a 16-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, Half data)
            => writer.Write(fieldName, BitConverter.HalfToUInt16Bits(data));

        /// <summary>Write a 2 components 32 bits floating-point vector as a 64-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, float x, float y)
            => writer.Write(fieldName, BitConverter.SingleToInt32Bits(x), BitConverter.SingleToInt32Bits(y));

        /// <summary>Write a 2 components 32 bits integer vector as a 64-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, int x, int y)
        {
            byte[][] bytes = new byte[2][] { BitConverter.GetBytes(x), BitConverter.GetBytes(y) };
            writer.Write(fieldName, BitConverter.ToUInt64(Combine(bytes, 2, 4)));
        }

        /// <summary>Write a 3 components 32 bits floating-point vector as a 96-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, float x, float y, float z)
            => writer.Write(fieldName, BitConverter.SingleToInt32Bits(x), BitConverter.SingleToInt32Bits(y), BitConverter.SingleToInt32Bits(z));

        /// <summary>Write a 3 components 32 bits integer vector as a 96-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, int x, int y, int z)
        {
            byte[][] bytes = new byte[3][] { BitConverter.GetBytes(x), BitConverter.GetBytes(y), BitConverter.GetBytes(z) };
            writer.Write(fieldName, new LargeInt(Combine(bytes, 3, 4)));
        }

        /// <summary>Write a 4 components 32 bits floating-point vector as a 128-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, float x, float y, float z, float w)
            => writer.Write(fieldName, BitConverter.SingleToInt32Bits(x), BitConverter.SingleToInt32Bits(y), BitConverter.SingleToInt32Bits(z), BitConverter.SingleToInt32Bits(w));

        /// <summary>Write a 4 components 32 bits integer vector as a 128-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, int x, int y, int z, int w)
        {
            byte[][] bytes = new byte[4][] { BitConverter.GetBytes(x), BitConverter.GetBytes(y), BitConverter.GetBytes(z), BitConverter.GetBytes(w) };
            writer.Write(fieldName, new BigInt(Combine(bytes, 4, 4)));
        }

        /// <summary>Write a 2 components 64 bits floating-point vector as a 128-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, double x, double y)
            => writer.Write(fieldName, BitConverter.DoubleToInt64Bits(x), BitConverter.DoubleToInt64Bits(y));

        /// <summary>Write a 2 components 64 bits integer vector as a 128-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, long x, long y)
        {
            byte[][] bytes = new byte[2][] { BitConverter.GetBytes(x), BitConverter.GetBytes(y) };
            writer.Write(fieldName, new BigInt(Combine(bytes, 2, 8)));
        }

        /// <summary>Write a 3 components 64 bits floating-point vector as a 192-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, double x, double y, double z)
            => writer.Write(fieldName, BitConverter.DoubleToInt64Bits(x), BitConverter.DoubleToInt64Bits(y), BitConverter.DoubleToInt64Bits(z));

        /// <summary>Write a 3 components 64 bits integer vector as a 192-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, long x, long y, long z)
        {
            byte[][] bytes = new byte[3][] { BitConverter.GetBytes(x), BitConverter.GetBytes(y), BitConverter.GetBytes(z) };
            writer.Write(fieldName, new GreatInt(Combine(bytes, 3, 8)));
        }

        /// <summary>Write a 4 components 64 bits floating-point vector as a 256-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, double x, double y, double z, double w)
            => writer.Write(fieldName, BitConverter.DoubleToInt64Bits(x), BitConverter.DoubleToInt64Bits(y), BitConverter.DoubleToInt64Bits(z), BitConverter.DoubleToInt64Bits(w));

        /// <summary>Write a 4 components 64 bits integer vector as a 256-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, long x, long y, long z, long w)
        {
            byte[][] bytes = new byte[4][] { BitConverter.GetBytes(x), BitConverter.GetBytes(y), BitConverter.GetBytes(z), BitConverter.GetBytes(w) };
            writer.Write(fieldName, new HugeInt(Combine(bytes, 4, 8)));
        }

        /// <summary>Write a 32 bits floating-point 4x4 matrix as a 512-bits field.</summary>
        public static void Write(this Writer writer, string fieldName, 
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            byte[][] bytes = new byte[16][] 
            { 
                BitConverter.GetBytes(m00), BitConverter.GetBytes(m01), BitConverter.GetBytes(m02), BitConverter.GetBytes(m03),
                BitConverter.GetBytes(m10), BitConverter.GetBytes(m11), BitConverter.GetBytes(m12), BitConverter.GetBytes(m13),
                BitConverter.GetBytes(m20), BitConverter.GetBytes(m21), BitConverter.GetBytes(m22), BitConverter.GetBytes(m23),
                BitConverter.GetBytes(m30), BitConverter.GetBytes(m31), BitConverter.GetBytes(m32), BitConverter.GetBytes(m33)
            };
            writer.Write(fieldName, new GiantInt(Combine(bytes, 16, 4)));
        }
    }
}