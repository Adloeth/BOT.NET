using System.Runtime.InteropServices;
using System.Collections;

namespace BOT
{
    public class Writer : IDisposable
    {
        private IntPtr nativeWriter;
        private bool nameLengthSet;

        public Writer(string path) : this(path, Options.Default) { }

        public Writer(string path, Options options, byte maxDepth = 10)
        {
            nativeWriter = botWriterCreate(path, maxDepth);
            botWriterInitFile(nativeWriter, (byte)options);

            if(!HasNameLength) nameLengthSet = true;
        }

        public void Dispose()
        {
            botWriterFlush(nativeWriter);
            botWriterFree(nativeWriter);
        }

        public bool HasNameLength => botWriterHasNameLength(nativeWriter);
        public bool HasFullWidth => botWriterHasFullWidth(nativeWriter);
        public byte FullWidth => botWriterGetFullWidth(nativeWriter);

        public void SetNameLength(byte nameLength)
        {
            botWriterSetNameLengthOption(nativeWriter, nameLength);
            nameLengthSet = true;
        }

        public void Flush() => botWriterFlush(nativeWriter);


        public void Write(string fieldName, IPrimitive obj)
        {
            if(obj == null)
                return;

            Write(fieldName, obj.AsPrimitive());
        }

        public void Write(string fieldName, ISerializable obj)
        {
            if(obj == null || obj.IsDefault)
                return;

            botWriterObjectBegin(nativeWriter, fieldName);
            obj.Write(this);
            botWriterObjectEnd(nativeWriter);
        }

        private void CheckValid()
        {
            if(!nameLengthSet) throw new Exception("Name length option was enabled but you didn't call SetNameLength() to set it before writting.");
        }

        public void Write(string fieldName, Primitive variant)
        {
            if(variant.Is<bool>()) Write(fieldName, variant.As<bool>());
            /*#macro decl WRITER_WRITE_VARIANT(type)
            else if(variant.Is<type>()) Write(fieldName, variant.As<type>());
            #macro end*/
            //#macro use WRITER_WRITE_VARIANT(     byte)
            //#macro use WRITER_WRITE_VARIANT(    sbyte)

            //#macro use WRITER_WRITE_VARIANT(   ushort)
            //#macro use WRITER_WRITE_VARIANT(    short)
            //#macro use WRITER_WRITE_VARIANT(     Half)

            //#macro use WRITER_WRITE_VARIANT(  Tribyte)

            //#macro use WRITER_WRITE_VARIANT(     uint)
            //#macro use WRITER_WRITE_VARIANT(      int)
            //#macro use WRITER_WRITE_VARIANT(    float)

            //#macro use WRITER_WRITE_VARIANT(Pentabyte)

            //#macro use WRITER_WRITE_VARIANT( Hexabyte)

            //#macro use WRITER_WRITE_VARIANT(Heptabyte)

            //#macro use WRITER_WRITE_VARIANT(    ulong)
            //#macro use WRITER_WRITE_VARIANT(     long)
            //#macro use WRITER_WRITE_VARIANT(   double)

            //#macro use WRITER_WRITE_VARIANT( LargeInt)

            //#macro use WRITER_WRITE_VARIANT(   BigInt)

            //#macro use WRITER_WRITE_VARIANT( GreatInt)

            //#macro use WRITER_WRITE_VARIANT(  HugeInt)

            //#macro use WRITER_WRITE_VARIANT( GiantInt)
        }

        public void Write(string fieldName, bool      data) { CheckValid(); botWriterWriteBool(nativeWriter, fieldName, data); }

        public void Write(string fieldName, byte      data) { CheckValid(); botWriterWrite8(nativeWriter, fieldName, data); }
        public void Write(string fieldName, sbyte     data) { CheckValid(); botWriterWrite8(nativeWriter, fieldName, (byte)data); }

        public void Write(string fieldName, ushort    data) { CheckValid(); botWriterWrite16(nativeWriter, fieldName, data); }
        public void Write(string fieldName, short     data) { CheckValid(); botWriterWrite16(nativeWriter, fieldName, (ushort)data); }
        public void Write(string fieldName, Half      data) { CheckValid(); botWriterWrite16(nativeWriter, fieldName, BitConverter.ToUInt16(BitConverter.GetBytes(data))); }

        public void Write(string fieldName, Tribyte   data) { CheckValid(); botWriterWrite24(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, uint      data) { CheckValid(); botWriterWrite32(nativeWriter, fieldName, data); }
        public void Write(string fieldName, int       data) { CheckValid(); botWriterWrite32(nativeWriter, fieldName, (uint)data); }
        public void Write(string fieldName, float     data) { CheckValid(); botWriterWriteFloat(nativeWriter, fieldName, data); }

        public void Write(string fieldName, Pentabyte data) { CheckValid(); botWriterWrite40(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, Hexabyte  data) { CheckValid(); botWriterWrite48(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, Heptabyte data) { CheckValid(); botWriterWrite56(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, ulong     data) { CheckValid(); botWriterWrite64(nativeWriter, fieldName, data); }
        public void Write(string fieldName, long      data) { CheckValid(); botWriterWrite64(nativeWriter, fieldName, (ulong)data); }
        public void Write(string fieldName, double    data) { CheckValid(); botWriterWriteDouble(nativeWriter, fieldName, data); }
 
        public void Write(string fieldName, LargeInt  data) { CheckValid(); botWriterWrite96(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, BigInt    data) { CheckValid(); botWriterWrite128(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, GreatInt  data) { CheckValid(); botWriterWrite192(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, HugeInt   data) { CheckValid(); botWriterWrite256(nativeWriter, fieldName, data.GetBytes()); }

        public void Write(string fieldName, GiantInt  data) { CheckValid(); botWriterWrite512(nativeWriter, fieldName, data.GetBytes()); }

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
                case Size.Auto:   botWriterInitAutoArray  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Tiny:   botWriterInitTinyArray  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Small:  botWriterInitSmallArray (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Short:  botWriterInitShortArray (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Medium: botWriterInitMediumArray(nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Large:  botWriterInitLargeArray (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Big:    botWriterInitBigArray   (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Great:  botWriterInitGreatArray (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Huge:   botWriterInitHugeArray  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                default: throw new Exception("Invalid size for writting an array !");
            }

            botWriterWriteCollectionElementTypes(nativeWriter, (byte[])(Array)elementTypes.ToArray(), (uint)elementTypes.Count);
            botWriterWriteCollectionCount(nativeWriter, (ulong)collection.LongCount());
            RecursiveArrayPush((ICollection)collection, elementTypes, 0);
        }

        private void PushArrayElements(Type type, Array array)
        {
            switch(type)
            {
                case Type.Bool     : botWriterPushArrayElementsBool(nativeWriter,                        (      bool[])array , (uint)array.Length); break;
                case Type.Byte     : botWriterPushArrayElements8   (nativeWriter,                        (      byte[])array , (uint)array.Length); break;
                case Type.Short    : botWriterPushArrayElements16  (nativeWriter,                        (    ushort[])array , (uint)array.Length); break;
                case Type.Tribyte  : botWriterPushArrayElements24  (nativeWriter,   Tribyte.GetBytesArray((  Tribyte[])array), (uint)array.Length); break;
                case Type.Pentabyte: botWriterPushArrayElements40  (nativeWriter, Pentabyte.GetBytesArray((Pentabyte[])array), (uint)array.Length); break;
                case Type.Hexabyte : botWriterPushArrayElements48  (nativeWriter,  Hexabyte.GetBytesArray(( Hexabyte[])array), (uint)array.Length); break;
                case Type.Heptabyte: botWriterPushArrayElements56  (nativeWriter, Heptabyte.GetBytesArray((Heptabyte[])array), (uint)array.Length); break;
                case Type.Large    : botWriterPushArrayElements96  (nativeWriter,  LargeInt.GetBytesArray(( LargeInt[])array), (uint)array.Length); break;
                case Type.Big      : botWriterPushArrayElements128 (nativeWriter,    BigInt.GetBytesArray(( BigInt  [])array), (uint)array.Length); break;
                case Type.Great    : botWriterPushArrayElements192 (nativeWriter,  GreatInt.GetBytesArray(( GreatInt[])array), (uint)array.Length); break;
                case Type.Huge     : botWriterPushArrayElements256 (nativeWriter,   HugeInt.GetBytesArray(( HugeInt [])array), (uint)array.Length); break;
                case Type.Giant    : botWriterPushArrayElements512 (nativeWriter,  GiantInt.GetBytesArray(( GiantInt[])array), (uint)array.Length); break;

                case Type.Int      : 
                    if(array.GetType() == typeof(float[]))
                        botWriterPushArrayElementsFloat (nativeWriter, (float[])array, (uint)array.Length);
                    else 
                        botWriterPushArrayElements32    (nativeWriter, ( uint[])array, (uint)array.Length); 
                    break;
                case Type.Long     : 
                    if(array.GetType() == typeof(double[]))
                        botWriterPushArrayElementsDouble(nativeWriter, (double[])array, (uint)array.Length);
                    else 
                        botWriterPushArrayElements64    (nativeWriter, ( ulong[])array, (uint)array.Length); break;

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
                    botWriterPushCollectionDimension(nativeWriter, (ulong)collection.Count, (byte)Standard.SizeFromCollection(currentType));
                
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
                    botWriterPushCollectionDimension(nativeWriter, (ulong)collection.Count, (byte)Standard.SizeFromCollection(elementTypes[depth - 1]));
                
                PushArrayElements(currentType, (Array)collection);
            }
        }

        public void WriteDictionary<K, V>(string fieldName, IDictionary<K, V> collection, Size size = Size.Auto)
        {
            switch(size)
            {
                case Size.Auto:   botWriterInitAutoDict  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Tiny:   botWriterInitTinyDict  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Small:  botWriterInitSmallDict (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Short:  botWriterInitShortDict (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Medium: botWriterInitMediumDict(nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Large:  botWriterInitLargeDict (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Big:    botWriterInitBigDict   (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Great:  botWriterInitGreatDict (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Huge:   botWriterInitHugeDict  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                default: throw new Exception("Invalid size for writting a dictionary !");
            }
        }

        public void WriteBlob(string fieldName, ICollection<byte> collection, Size size = Size.Auto)
        {
            switch(size)
            {
                case Size.Auto:   botWriterInitAutoBlob  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Tiny:   botWriterInitTinyBlob  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Small:  botWriterInitSmallBlob (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Short:  botWriterInitShortBlob (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Medium: botWriterInitMediumBlob(nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Large:  botWriterInitLargeBlob (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Big:    botWriterInitBigBlob   (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Great:  botWriterInitGreatBlob (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                case Size.Huge:   botWriterInitHugeBlob  (nativeWriter, fieldName, (ulong)collection.LongCount()); break;
                default: throw new Exception("Invalid size for writting a blob !");
            }
        }

        [DllImport("libBOT.so")] static private extern IntPtr botWriterCreate(string path, byte maxDepth);
        [DllImport("libBOT.so")] static private extern void botWriterFree(IntPtr writer);

        [DllImport("libBOT.so")] static private extern bool botWriterHasNameLength(IntPtr writer);
        [DllImport("libBOT.so")] static private extern bool botWriterHasFullWidth(IntPtr writer);
        [DllImport("libBOT.so")] static private extern byte botWriterGetFullWidth(IntPtr writer);

        [DllImport("libBOT.so")] static private extern void botWriterInitFile(IntPtr writer, byte botOptions);
        [DllImport("libBOT.so")] static private extern void botWriterSetNameLengthOption(IntPtr writer, byte nameLength);

        [DllImport("libBOT.so")] static private extern void botWriterObjectBegin(IntPtr writer, string fieldName);
        [DllImport("libBOT.so")] static private extern void botWriterObjectEnd(IntPtr writer);

        [DllImport("libBOT.so")] static private extern void botWriterFlush(IntPtr writer);

        [DllImport("libBOT.so")] static private extern void botWriterWriteBool(IntPtr writer, string fieldName, bool data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite8(IntPtr writer, string fieldName, byte data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite16(IntPtr writer, string fieldName, ushort data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite24(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite32(IntPtr writer, string fieldName, uint data);
        [DllImport("libBOT.so")] static private extern void botWriterWriteFloat(IntPtr writer, string fieldName, float data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite40(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite48(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite56(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite64(IntPtr writer, string fieldName, ulong data);
        [DllImport("libBOT.so")] static private extern void botWriterWriteDouble(IntPtr writer, string fieldName, double data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite96(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite128(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite192(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite256(IntPtr writer, string fieldName, byte[] data);
        [DllImport("libBOT.so")] static private extern void botWriterWrite512(IntPtr writer, string fieldName, byte[] data);

#region COLLECTIONS

        [DllImport("libBOT.so")] static private extern void botWriterWriteCollectionElementType(IntPtr writer, byte botType);
        [DllImport("libBOT.so")] static private extern void botWriterWriteCollectionElementTypes(IntPtr writer, byte[] botTypes, uint length);
        [DllImport("libBOT.so")] static private extern void botWriterWriteCollectionCount(IntPtr writer, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushCollectionDimension(IntPtr writer, ulong count, byte size);
        

        [DllImport("libBOT.so")] static private extern void botWriterInitAutoArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitTinyArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitSmallArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitShortArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitMediumArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitLargeArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitBigArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitGreatArray(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitHugeArray(IntPtr writer, string fieldName, ulong count);

        [DllImport("libBOT.so")] static private extern void botWriterInitAutoDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitTinyDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitSmallDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitShortDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitMediumDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitLargeDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitBigDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitGreatDict(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitHugeDict(IntPtr writer, string fieldName, ulong count);

        [DllImport("libBOT.so")] static private extern void botWriterInitAutoBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitTinyBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitSmallBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitShortBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitMediumBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitLargeBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitBigBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitGreatBlob(IntPtr writer, string fieldName, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterInitHugeBlob(IntPtr writer, string fieldName, ulong count);


        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElementsBool(IntPtr writer, bool[] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements8(IntPtr writer, byte[] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements16(IntPtr writer, ushort[] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements24(IntPtr writer, byte[][] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements32(IntPtr writer, uint[] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElementsFloat(IntPtr writer, float[] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements40(IntPtr writer, byte[][] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements48(IntPtr writer, byte[][] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements56(IntPtr writer, byte[][] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements64(IntPtr writer, ulong[] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElementsDouble(IntPtr writer, double[] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements96(IntPtr writer, byte[][] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements128(IntPtr writer,byte[][] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements192(IntPtr writer,byte[][] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements256(IntPtr writer,byte[][] data, ulong count);
        [DllImport("libBOT.so")] static private extern void botWriterPushArrayElements512(IntPtr writer,byte[][] data, ulong count);
    
#endregion
    }
}