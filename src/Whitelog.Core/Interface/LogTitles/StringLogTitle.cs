namespace Whitelog.Interface.LogTitles
{
    public abstract class StringLogTitle : ILogTitle
    {
        private readonly string m_message;
        public abstract string Title { get; }
        
        protected StringLogTitle(string message)
        {
            m_message = message;
        }

        public string Message { get { return m_message; } }
    }

    public class CustomStringLogTitle : StringLogTitle
    {
        private readonly string m_title;
        private readonly string m_message;

        public CustomStringLogTitle(string title, string message):base(message)
        {
            m_message = message;
            m_title = title;
        }

        public override string Title
        {
            get { return m_title; }
        }

        public string Message
        {
            get { return m_message; }
        }
    }
}