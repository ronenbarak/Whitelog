namespace Whitelog.Interface.LogTitles
{
    public class InfoLogTitle : StringLogTitle
    {
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