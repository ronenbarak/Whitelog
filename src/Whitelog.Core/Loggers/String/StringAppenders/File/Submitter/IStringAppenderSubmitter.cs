using System;

namespace Whitelog.Core.Loggers.String.StringAppenders.File.Submitter
{
    public interface IStringAppenderSubmitter
    {
        void Submit(string text,DateTime timestamp);
    }
}