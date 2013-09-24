namespace Whitelog.Interface.LogTitles
{
    public class WarningLogTitle : StringLogTitle
    {
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