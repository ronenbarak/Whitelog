using System;

namespace Whitelog.Core.String.Layout
{
    public interface IMessageParamaterHanlderFactory
    {
        IStringLayoutWriter Create(string property, Type type);
    }
}