using System.Runtime.InteropServices;
using System.Collections;

namespace FFF
{
    public class Writer : IDisposable
    {
        private IntPtr nativeWriter;
        private bool nameLengthSet;

        public Writer(string path) : this(path, Options.Default) { }

        public Writer(string path, Options options, byte maxDepth = 10)
        {
            nativeWriter = fffWriterCreate(path, maxDepth);
            fffWriterInitFile(nativeWriter, (byte)options);

            if(!HasNameLength) nameLengthSet = true;
        }

        public void Dispose()
        {
            fffWriterFree(nativeWriter);
        }

        public bool HasNameLength => fffWriterHasNameLength(nativeWriter);
        public bool HasFullWidth => fffWriterHasFullWidth(nativeWriter);
        public byte FullWidth => fffWriterGetFullWidth(nativeWriter);

        public void SetNameLength(byte nameLength)
        {
            fffWriterSetNameLengthOption(nativeWriter, nameLength);
            nameLengthSet = true;
        }

        public void Flush() => fffWriterFlush(nativeWriter);

        public void Write(string fieldName, ISerializable obj)
        {
            if(obj == null || obj.IsDefault)
                return;

            fffWriterObjectBegin(nativeWriter, fieldName);
            obj.Write(this);
            fffWriterObjectEnd(nativeWriter);
        }

        public void Write(string fieldName, bool data) => fffWriterWriteBool(nativeWriter, fieldName, data);

        public void Write(string fieldName, byte data) => fffWriterWrite8(nativeWriter, fieldName, data);
        public void Write(string fieldName, sbyte data) => fffWriterWrite8(nativeWriter, fieldName, (byte)data);

        public void Write(string fieldName, ushort data) => fffWriterWrite16(nativeWriter, fieldName, data);
        public void Write(string fieldName, short data) => fffWriterWrite16(nativeWriter, fieldName, (ushort)data);

        public void Write(string fieldName, Tribyte data) => fffWriterWrite24(nativeWriter, fieldName, data.GetBytes());

        public void Write(string fieldName, uint data) => fffWriterWrite32(nativeWriter, fieldName, data);
        public void Write(string fieldName, int data) => fffWriterWrite32(nativeWriter, fieldName, (uint)data);
        public void Write(string fieldName, float data) => fffWriterWriteFloat(nativeWriter, fieldName, data);

        public void Write(string fieldName, Pentabyte data) => fffWriterWrite40(nativeWriter, fieldName, data.GetBytes());

        public void Write(string fieldName, Hexabyte data) => fffWriterWrite48(nativeWriter, fieldName, data.GetBytes());

        public void Write(string fieldName, Heptabyte data) => fffWriterWrite56(nativeWriter, fieldName, data.GetBytes());

        public void Write(string fieldName, ulong data) => fffWriterWrite64(nativeWriter, fieldName, data);
        public void Write(string fieldName, long data) => fffWriterWrite64(nativeWriter, fieldName, (ulong)data);
        public void Write(string fieldName, double data) => fffWriterWriteDouble(nativeWriter, fieldName, data);

        public void Write(string fieldName, LargeInt data) => fffWriterWrite96(nativeWriter, fieldName, data.GetBytes());

        public void Write(string fieldName, BigInt data) => fffWriterWrite128(nativeWriter, fieldName, data.GetBytes());

        public void Write(string fieldName, GreatInt data) => fffWriterWrite192(nativeWriter, fieldName, data.GetBytes());

        public void Write(string fieldName, HugeInt data) => fffWriterWrite256(nativeWriter, fieldName, data.GetBytes());

        public void Write(string fieldName, GiantInt data) => fffWriterWrite512(nativeWriter, fieldName, data.GetBytes());

        public void WriteArray<T>(string fieldName, ICollection<T> collection, Size size = Size.Auto)
        {
            if(collection == null)
                return;

            switch(size)
            {
                case Size.Auto:   fffWriterInitAutoArray  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Tiny:   fffWriterInitTinyArray  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Small:  fffWriterInitSmallArray (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Short:  fffWriterInitShortArray (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Medium: fffWriterInitMediumArray(nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Large:  fffWriterInitLargeArray (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Big:    fffWriterInitBigArray   (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Great:  fffWriterInitGreatArray (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Huge:   fffWriterInitHugeArray  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                default: throw new Exception("Invalid size for writting an array !");
            }
            
            if(typeof(T) == typeof(IDictionary))
            {

            }
            else if(typeof(T) == typeof(ICollection))
            {

            }
            else
            {

            }
        }

        public void WriteDictionary<K, V>(string fieldName, IDictionary<K, V> collection, Size size = Size.Auto)
        {
            switch(size)
            {
                case Size.Auto:   fffWriterInitAutoDict  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Tiny:   fffWriterInitTinyDict  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Small:  fffWriterInitSmallDict (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Short:  fffWriterInitShortDict (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Medium: fffWriterInitMediumDict(nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Large:  fffWriterInitLargeDict (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Big:    fffWriterInitBigDict   (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Great:  fffWriterInitGreatDict (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Huge:   fffWriterInitHugeDict  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                default: throw new Exception("Invalid size for writting a dictionary !");
            }
        }

        public void WriteBlob(string fieldName, ICollection<byte> collection, Size size = Size.Auto)
        {
            switch(size)
            {
                case Size.Auto:   fffWriterInitAutoBlob  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Tiny:   fffWriterInitTinyBlob  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Small:  fffWriterInitSmallBlob (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Short:  fffWriterInitShortBlob (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Medium: fffWriterInitMediumBlob(nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Large:  fffWriterInitLargeBlob (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Big:    fffWriterInitBigBlob   (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Great:  fffWriterInitGreatBlob (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Huge:   fffWriterInitHugeBlob  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                default: throw new Exception("Invalid size for writting a blob !");
            }
        }

        [DllImport("libFFF.so")] static private extern IntPtr fffWriterCreate(string path, byte maxDepth);
        [DllImport("libFFF.so")] static private extern void fffWriterFree(IntPtr writer);

        [DllImport("libFFF.so")] static private extern bool fffWriterHasNameLength(IntPtr writer);
        [DllImport("libFFF.so")] static private extern bool fffWriterHasFullWidth(IntPtr writer);
        [DllImport("libFFF.so")] static private extern byte fffWriterGetFullWidth(IntPtr writer);

        [DllImport("libFFF.so")] static private extern void fffWriterInitFile(IntPtr writer, byte fffOptions);
        [DllImport("libFFF.so")] static private extern void fffWriterSetNameLengthOption(IntPtr writer, byte nameLength);

        [DllImport("libFFF.so")] static private extern void fffWriterObjectBegin(IntPtr writer, string fieldName);
        [DllImport("libFFF.so")] static private extern void fffWriterObjectEnd(IntPtr writer);

        [DllImport("libFFF.so")] static private extern void fffWriterFlush(IntPtr writer);

        [DllImport("libFFF.so")] static private extern void fffWriterWriteBool(IntPtr writer, string fieldName, bool data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite8(IntPtr writer, string fieldName, byte data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite16(IntPtr writer, string fieldName, ushort data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite24(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite32(IntPtr writer, string fieldName, uint data);
        [DllImport("libFFF.so")] static private extern void fffWriterWriteFloat(IntPtr writer, string fieldName, float data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite40(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite48(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite56(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite64(IntPtr writer, string fieldName, ulong data);
        [DllImport("libFFF.so")] static private extern void fffWriterWriteDouble(IntPtr writer, string fieldName, double data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite96(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite128(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite192(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite256(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libFFF.so")] static private extern void fffWriterWrite512(IntPtr writer, string fieldName, byte[] data);

#region COLLECTIONS

        [DllImport("libFFF.so")] static private extern void fffWriterWriteCollectionElementType(IntPtr writer, byte fffType);
        [DllImport("libFFF.so")] static private extern void fffWriterWriteCollectionElementTypes(IntPtr writer, byte[] fffTypes, uint length);
        [DllImport("libFFF.so")] static private extern void fffWriterWriteCollectionCount(IntPtr writer, ulong count);

        [DllImport("libFFF.so")] static private extern void fffWriterInitAutoArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitTinyArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitSmallArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitShortArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitMediumArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitLargeArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitBigArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitGreatArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitHugeArray(IntPtr writer, string fieldName, ulong count);

        [DllImport("libFFF.so")] static private extern void fffWriterInitAutoDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitTinyDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitSmallDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitShortDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitMediumDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitLargeDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitBigDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitGreatDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitHugeDict(IntPtr writer, string fieldName, ulong count);

        [DllImport("libFFF.so")] static private extern void fffWriterInitAutoBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitTinyBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitSmallBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitShortBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitMediumBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitLargeBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitBigBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitGreatBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterInitHugeBlob(IntPtr writer, string fieldName, ulong count);
    
#endregion
    }
}