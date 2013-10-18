
namespace Whitelog.Interface.LogTitles
{
    public class DebugLogTitle : StringLogTitle 
    {
        public override long Id { get { return ReservedLogTitleIds.Debug; } }

        public DebugLogTitle(string message):base(message)
        {
        }

        public override string Title
        {
            get { return "Debug"; }
        }
    }
}
