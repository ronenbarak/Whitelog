namespace Whitelog.Interface.LogTitles
{
    public class FatalLogTitle : StringLogTitle
    {
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