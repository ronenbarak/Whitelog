namespace Whitelog.Interface.LogTitles
{
    public class StringLogTitle : ILogTitle
    {
        private readonly string m_title;
        public string Title { get { return m_title; } }
        
        protected StringLogTitle(string title)
        {
            m_title = title;
        }
    }

    public class CustomStringLogTitle : StringLogTitle
    {
        public CustomStringLogTitle(string title):base(title)
        {
        }
    }
}