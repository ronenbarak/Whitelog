namespace Whitelog.Core.Binary.Deserilizer
{
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