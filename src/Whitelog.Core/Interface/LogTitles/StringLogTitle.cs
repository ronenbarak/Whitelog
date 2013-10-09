namespace Whitelog.Interface.LogTitles
{
    public interface IMessageLogTitle : ILogTitle
    {
        string Message { get; }
    }

    public abstract class StringLogTitle : IMessageLogTitle
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

        public CustomStringLogTitle(string title, string message):base(message)
        {
            m_title = title;
        }

        public override string Title
        {
            get { return m_title; }
        }
    }
}