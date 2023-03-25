namespace FFF
{
    public interface ISerializable
    {
        public bool IsDefault { get; }

        public void Write(Writer writer);
        public void Read();
    }
}