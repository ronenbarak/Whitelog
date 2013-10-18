namespace Whitelog.Interface.LogTitles
{
    public class InfoLogTitle : StringLogTitle
    {
        public override long Id { get { return ReservedLogTitleIds.Info; } }

        public InfoLogTitle(string message)
            : base(message)
        {
        }

        public override string Title
        {
            get { return "Info"; }
        }
    }
}