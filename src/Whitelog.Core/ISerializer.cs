namespace Whitelog.Core
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
        void Serialize(string value);
        void Serialize(byte[] value, int srcOffset, int count);
    }

    public interface IDeserializer
    {
        int DeserializeInt();
        int DeserializeVariantInt();
        long DeserializeLong();
        double DeserializeDouble();
        bool DeserializeBool();
        byte DeserializeByte();
        string DeserializeString();
        byte[] DeserializeByteArray();
        byte[] DeserializeByteArray(int length);
    }
}