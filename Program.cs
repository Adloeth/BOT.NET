using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace FFF;

public struct Vector2i
{
    public int x;
    public int y;

    public Vector2i(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Vector2i vector))
            return false;
        return this.x == vector.x && this.y == vector.y;
    }
    
    public override int GetHashCode() => HashCode.Combine(x, y);

    public static bool operator ==(Vector2i a, Vector2i b) => a.Equals(b);
    public static bool operator !=(Vector2i a, Vector2i b) => !a.Equals(b);
}

public class Test : ISerializable
{
    private Vector2i position;
    private Tribyte whatever;
    private int test0;
    private Test anotherObj;

    public Test(Vector2i position, Tribyte whatever, int test0, Test anotherObj)
    {
        this.position = position;
        this.whatever = whatever;
        this.test0 = test0;
        this.anotherObj = anotherObj;
    }

    public bool IsDefault => position == new Vector2i() && whatever == 0 && test0 == 0 && Standard.IsDefault(anotherObj);

    public void Write(Writer writer)
    {
        writer.Write("position", position.x, position.y);
        writer.Write("whatever", whatever);
        writer.Write("anotherObj", anotherObj);
        writer.Write("test0", test0);
    }

    public void Read()
    {

    }
}

public class Test0 : ISerializable
{
    private int test;
    private ISerializable tribyte;

    public Test0(int test, ISerializable tribyte)
    {
        this.test = test;
        this.tribyte = tribyte;
    }

    public bool IsDefault => test == 0 && tribyte == null;

    public void Write(Writer writer)
    {
        writer.Write("Test", test);
        writer.Write("test", tribyte);
    }

    public void Read()
    {

    }
}


internal class Program
{

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string path = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
        Console.WriteLine("Path is '" + path + "/test.fff" + "'");
        using(Writer writer = new Writer(path + "/test.fff"/*, Options.FixedNameLength*/))
        {
            //writer.SetNameLength(5);
            //byte[] data = new byte[64];
            //data[0] = 1;
            Test nested1 = new Test(new Vector2i(-18, 500), new Tribyte(), -1, null);
            Test nested0 = new Test(new Vector2i(), new Tribyte(10), 0, nested1);
            Test test = new Test(new Vector2i(10, 5), new Tribyte(57), -1, nested0);
            writer.Write("Root0", test);
            //writer.Write("Root1", new Test0(10, new Test0(5, null)));
        }

        /*using(TextWriter writer = (TextWriter)new StreamWriter(File.OpenWrite(path + "/code.cs")))
        {
            GenerateCode(writer, "Tribyte", 3, "uint");
            GenerateCode(writer, "Pentabyte", 5, "ulong");
            GenerateCode(writer, "Hexabyte", 6, "ulong");
            GenerateCode(writer, "Heptabyte", 7, "ulong");
            
            GenerateCode(writer, "LargeInt", 12);
            GenerateCode(writer, "BigInt", 16);
            GenerateCode(writer, "GreatInt", 24);
            GenerateCode(writer, "HugeInt", 32);
            GenerateCode(writer, "GiantInt", 64);
        }*/
    }

    static void GenerateCode(TextWriter stream, string name, int size, string convertFrom = null)
    {
        stream.WriteLine("[StructLayout(LayoutKind.Sequential, Pack = 1)]");
        stream.WriteLine("public struct " + name);
        stream.WriteLine('{');
        stream.WriteLine("\tprivate byte[] data;");
        stream.WriteLine("");
        stream.Write("\tpublic " + name + "(");
        for(int i = 0; i < size; i++)
        {
            if(i > 0)
                stream.Write(", ");
            stream.Write("byte b" + i);
        }
        stream.WriteLine(")");
        stream.WriteLine("\t{");
        stream.Write("\t\tthis.data = new byte[" + size + "] { ");
        for(int i = 0; i < size; i++)
        {
            if(i > 0)
                stream.Write(", ");
            stream.Write("b" + i);
        }
        stream.Write(" };");
        stream.WriteLine("");
        stream.WriteLine("\t}");
        stream.WriteLine("");

        if(convertFrom != null)
        {
            stream.Write("\tpublic " + name + "(" + convertFrom + " data) : this(");

            uint shift = 8;
            for(int i = 0; i < size; i++)
            {
                if(i == 0)
                {
                    stream.Write("(byte)(data & 0xFF)");
                    continue;
                }

                stream.Write(", (byte)((data >> " + shift + ") & 0xFF)");
                shift += 8;
            }
            stream.Write(") { }");
            stream.WriteLine("");
            stream.WriteLine("");
        }

        stream.WriteLine("\tpublic " + name + "(params byte[] data)");
        stream.WriteLine("\t{");
        stream.WriteLine("\t\tthis.data = new byte[" + size + "];");
        stream.WriteLine("\t\tfor(int i = 0; i < " + size + "; i++)");
        stream.WriteLine("\t\t\tthis.data[i] = data[i];");
        stream.WriteLine("\t}");
        stream.WriteLine("");

        stream.WriteLine("\tpublic byte[] GetBytes() => data;");

        stream.WriteLine('}');
        stream.WriteLine("");
        stream.WriteLine("");
    }
}