namespace Whitelog.Interface.LogTitles
{
    public class ErrorLogTitle : StringLogTitle
    {
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