namespace Whitelog.Core.Binary.Deserilizer.Unpack
{
    public class EmptyConstractorUnpackageDefinition<T> :UnpackageDefinition<T> where T :new()
    {

        public EmptyConstractorUnpackageDefinition()
        {
        }

        public EmptyConstractorUnpackageDefinition(int definitionId)
            : base(definitionId)
        {
        }

        protected override T CreateInstance()
        {
            return new T();
        }
    }
}