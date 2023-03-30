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
            fffWriterFlush(nativeWriter);
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

        private void CheckValid()
        {
            if(!nameLengthSet) throw new Exception("Name length option was enabled but you didn't call SetNameLength() to set it before writting.");
        }

        public void Write(string fieldName, bool      data) { CheckValid(); fffWriterWriteBool(nativeWriter, fieldName, data); }

        public void Write(string fieldName, byte      data) { CheckValid(); fffWriterWrite8(nativeWriter, fieldName, data); }
        public void Write(string fieldName, sbyte     data) { CheckValid(); fffWriterWrite8(nativeWriter, fieldName, (byte)data); }

        public void Write(string fieldName, ushort    data) { CheckValid(); fffWriterWrite16(nativeWriter, fieldName, data); }
        public void Write(string fieldName, short     data) { CheckValid(); fffWriterWrite16(nativeWriter, fieldName, (ushort)data); }

        public void Write(string fieldName, Tribyte   data) { CheckValid(); fffWriterWrite24(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, uint      data) { CheckValid(); fffWriterWrite32(nativeWriter, fieldName, data); }
        public void Write(string fieldName, int       data) { CheckValid(); fffWriterWrite32(nativeWriter, fieldName, (uint)data); }
        public void Write(string fieldName, float     data) { CheckValid(); fffWriterWriteFloat(nativeWriter, fieldName, data); }

        public void Write(string fieldName, Pentabyte data) { CheckValid(); fffWriterWrite40(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, Hexabyte  data) { CheckValid(); fffWriterWrite48(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, Heptabyte data) { CheckValid(); fffWriterWrite56(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, ulong     data) { CheckValid(); fffWriterWrite64(nativeWriter, fieldName, data); }
        public void Write(string fieldName, long      data) { CheckValid(); fffWriterWrite64(nativeWriter, fieldName, (ulong)data); }
        public void Write(string fieldName, double    data) { CheckValid(); fffWriterWriteDouble(nativeWriter, fieldName, data); }
 
        public void Write(string fieldName, LargeInt  data) { CheckValid(); fffWriterWrite96(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, BigInt    data) { CheckValid(); fffWriterWrite128(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, GreatInt  data) { CheckValid(); fffWriterWrite192(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, HugeInt   data) { CheckValid(); fffWriterWrite256(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, GiantInt  data) { CheckValid(); fffWriterWrite512(nativeWriter, fieldName, data.GetBytes()); }

        private ulong GetCollectionOfCollectionMaxSize(ICollection collection)
        {
            int maxSize = 0;

            foreach(ICollection? item in collection)
                if(item != null && maxSize < item.Count)
                    maxSize = item.Count;
                
            return (ulong)maxSize;
        }

        private void GetArrayElements(System.Type? elementType, ICollection collection, List<Type> elementTypes)
        {
            if(elementType == null)
                throw new Exception("Somehow the element type of the collection is 'null'.");

            //Dictionaries implements ICollection so IDictionary need to be tested first
            if(typeof(IDictionary).IsAssignableFrom(elementType))
            {
                
            }
            //Blobs are collections of bytes
            else if(typeof(ICollection<byte>).IsAssignableFrom(elementType) || typeof(ICollection<sbyte>).IsAssignableFrom(elementType))
            {
                elementTypes.Add(Standard.GetCollectionType(Collection.Blob, Size.Auto, GetCollectionOfCollectionMaxSize((ICollection)collection)));
            }
            //Arrays
            else if(typeof(ICollection).IsAssignableFrom(elementType))
            {
                elementTypes.Add(Standard.GetCollectionType(Collection.Array, Size.Auto, GetCollectionOfCollectionMaxSize((ICollection)collection)));
                IEnumerator enumerator = collection.GetEnumerator();
                enumerator.MoveNext();
                GetArrayElements(elementType.GetElementType(), (ICollection)enumerator.Current, elementTypes);
            }
            //Primitive type or object
            else
            {
                elementTypes.Add(Standard.CSTypeToPrimitive(elementType));
            }
        }

        public void WriteArray<T>(string fieldName, ICollection<T> collection, Size size = Size.Auto)
        {
            if(collection == null)
                return;

            List<Type> elementTypes = new List<Type>();
            GetArrayElements(typeof(T), (ICollection)collection, elementTypes);

            Console.WriteLine(elementTypes.Count);

            if(elementTypes.Count == 0)
                return;

            for(int i = 0; i < elementTypes.Count; i++)
                Console.Write(string.Concat(i > 0 ? ", " : "", elementTypes[i]));
            Console.WriteLine("");

            if(Standard.IsCollection(elementTypes[elementTypes.Count - 1]))
                return;

            Console.WriteLine("All good !");

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

            fffWriterWriteCollectionElementTypes(nativeWriter, (byte[])(Array)elementTypes.ToArray(), (uint)elementTypes.Count);
            fffWriterWriteCollectionCount(nativeWriter, (ulong)collection.LongCount());
            RecursiveArrayPush((ICollection)collection, elementTypes, 0);
        }

        private void PushArrayElements(Type type, Array array)
        {
            switch(type)
            {
                case Type.Bool     : fffWriterPushArrayElementsBool(nativeWriter,                        (      bool[])array , (uint)array.Length); break;
                case Type.Byte     : fffWriterPushArrayElements8   (nativeWriter,                        (      byte[])array , (uint)array.Length); break;
                case Type.Short    : fffWriterPushArrayElements16  (nativeWriter,                        (    ushort[])array , (uint)array.Length); break;
                case Type.Tribyte  : fffWriterPushArrayElements24  (nativeWriter,   Tribyte.GetBytesArray((  Tribyte[])array), (uint)array.Length); break;
                case Type.Pentabyte: fffWriterPushArrayElements40  (nativeWriter, Pentabyte.GetBytesArray((Pentabyte[])array), (uint)array.Length); break;
                case Type.Hexabyte : fffWriterPushArrayElements48  (nativeWriter,  Hexabyte.GetBytesArray(( Hexabyte[])array), (uint)array.Length); break;
                case Type.Heptabyte: fffWriterPushArrayElements56  (nativeWriter, Heptabyte.GetBytesArray((Heptabyte[])array), (uint)array.Length); break;
                case Type.Large    : fffWriterPushArrayElements96  (nativeWriter,  LargeInt.GetBytesArray(( LargeInt[])array), (uint)array.Length); break;
                case Type.Big      : fffWriterPushArrayElements128 (nativeWriter,    BigInt.GetBytesArray(( BigInt  [])array), (uint)array.Length); break;
                case Type.Great    : fffWriterPushArrayElements192 (nativeWriter,  GreatInt.GetBytesArray(( GreatInt[])array), (uint)array.Length); break;
                case Type.Huge     : fffWriterPushArrayElements256 (nativeWriter,   HugeInt.GetBytesArray(( HugeInt [])array), (uint)array.Length); break;
                case Type.Giant    : fffWriterPushArrayElements512 (nativeWriter,  GiantInt.GetBytesArray(( GiantInt[])array), (uint)array.Length); break;

                case Type.Int      : 
                    if(array.GetType() == typeof(float[]))
                        fffWriterPushArrayElementsFloat (nativeWriter, (float[])array, (uint)array.Length);
                    else 
                        fffWriterPushArrayElements32    (nativeWriter, ( uint[])array, (uint)array.Length); 
                    break;
                case Type.Long     : 
                    if(array.GetType() == typeof(double[]))
                        fffWriterPushArrayElementsDouble(nativeWriter, (double[])array, (uint)array.Length);
                    else 
                        fffWriterPushArrayElements64    (nativeWriter, ( ulong[])array, (uint)array.Length); break;

                default: throw new Exception("Cannot push array elements, '" + type + "' cannot be translated to a primitive type !");
            }
        }

        private void RecursiveArrayPush(ICollection collection, List<Type> elementTypes, int depth)
        {
            Type currentType = elementTypes[depth];

            Console.WriteLine(currentType);

            if(Standard.IsArray(currentType))
            {
                if(depth > 0)
                    fffWriterPushCollectionDimension(nativeWriter, (ulong)collection.Count, (byte)Standard.SizeFromCollection(currentType));
                
                foreach(object? obj in collection)
                    RecursiveArrayPush((ICollection)obj, elementTypes, depth + 1);
            }
            else if(Standard.IsDictionary(currentType))
            {

            }
            else if(Standard.IsBlob(currentType))
            {

            }
            else
            {
                if(depth > 0)
                    fffWriterPushCollectionDimension(nativeWriter, (ulong)collection.Count, (byte)Standard.SizeFromCollection(elementTypes[depth - 1]));
                
                PushArrayElements(currentType, (Array)collection);
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
        [DllImport("libFFF.so")] static private extern void fffWriterPushCollectionDimension(IntPtr writer, ulong count, byte size);
        

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


        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElementsBool(IntPtr writer, bool[] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements8(IntPtr writer, byte[] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements16(IntPtr writer, ushort[] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements24(IntPtr writer, byte[][] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements32(IntPtr writer, uint[] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElementsFloat(IntPtr writer, float[] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements40(IntPtr writer, byte[][] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements48(IntPtr writer, byte[][] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements56(IntPtr writer, byte[][] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements64(IntPtr writer, ulong[] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElementsDouble(IntPtr writer, double[] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements96(IntPtr writer, byte[][] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements128(IntPtr writer,byte[][] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements192(IntPtr writer,byte[][] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements256(IntPtr writer,byte[][] data, ulong count);
        [DllImport("libFFF.so")] static private extern void fffWriterPushArrayElements512(IntPtr writer,byte[][] data, ulong count);
    
#endregion
    }
}