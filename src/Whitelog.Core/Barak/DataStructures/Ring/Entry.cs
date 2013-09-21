namespace Whitelog.Barak.Common.DataStructures.Ring
{
    public sealed class Entry<T>
    {
        public enum StateOptions
        {
            Allocated,
            Commited,
            Consumed,
        };

        public T Data { get; set; }

        public StateOptions State;
        
        public long Sequence;

        public Entry(T data)
        {
            Data = data;
        }
    }
}