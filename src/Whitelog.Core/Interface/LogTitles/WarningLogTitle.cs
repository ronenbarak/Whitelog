namespace Whitelog.Interface.LogTitles
{
    public class WarningLogTitle : StringLogTitle
    {
        public override long Id { get { return ReservedLogTitleIds.Warning; } }

        public WarningLogTitle(string message)
            : base(message)
        {
        }


        public override string Title
        {
            get { return "Warning"; }
        }
    }
}