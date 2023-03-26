namespace FFF
{
    public interface ISerializable
    {
        public bool IsDefault { get; }

        public void Write(Writer writer);
        public void Read();
    }

    /// <summary>
    /// If your class is serialized as a type other than object, use this instead of ISerializable. 
    /// <br></br>For example, a 2 components vector of 32-bits integers can be serialize as a 64-bits long, 
    /// thus your vector class can implement ISerializedAs and it would be treated like a primitive, not an object. 
    /// </summary>
    public interface ISerializedAs
    {
        public static Type SerializedAsType() { throw new Exception("You must declare the SerializedAsType method !"); }

        public void Write(Writer writer);
    }
}