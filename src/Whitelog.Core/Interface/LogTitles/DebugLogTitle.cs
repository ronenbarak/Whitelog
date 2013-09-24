
namespace Whitelog.Interface.LogTitles
{
    public class DebugLogTitle : StringLogTitle 
    {
        public DebugLogTitle(string message):base(message)
        {
        }

        public override string Title
        {
            get { return "Debug"; }
        }
    }
}
