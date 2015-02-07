namespace Whitelog.Core.Binary.Serializer
{
    public interface ISerializer
    {
        void Flush();
        void Serialize(int value);
        void SerializeVariant(int value);
        void Serialize(long value);
        void Serialize(double value);
        void Serialize(bool value);
        void Serialize(byte value);
        void Serialize(short value);
        void Serialize(string value);
        void Serialize(byte[] value, int srcOffset, int count);
    }
}