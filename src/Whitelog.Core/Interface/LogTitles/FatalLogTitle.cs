namespace Whitelog.Interface.LogTitles
{
    public class FatalLogTitle : StringLogTitle
    {
        public override long Id { get { return ReservedLogTitleIds.Fatal; } }

        public FatalLogTitle(string message)
            : base(message)
        {
        }

        public override string Title
        {
            get { return "Fatal"; }
        }
    }
}