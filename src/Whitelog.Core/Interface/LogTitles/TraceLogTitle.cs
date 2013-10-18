namespace Whitelog.Interface.LogTitles
{
    public class TraceLogTitle : StringLogTitle
    {
        public override long Id { get { return ReservedLogTitleIds.Trace; } }

        public TraceLogTitle(string message)
            : base(message)
        {
        }

        public override string Title
        {
            get { return "Trace"; }
        }
    }
}