namespace Whitelog.Interface.LogTitles
{
    public class ErrorLogTitle : StringLogTitle
    {
        public override long Id { get { return ReservedLogTitleIds.Error; } }

        public ErrorLogTitle(string message)
            : base(message)
        {
        }

        public override string Title
        {
            get { return "Error"; }
        }
    }
}