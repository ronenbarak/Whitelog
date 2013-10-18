using System;
using System.Text;

namespace Whitelog.Core.String.StringBuffer
{
    public interface IStringBufferAllocation : IDisposable
    {
        StringBuilder StringBuilder { get; }
    }

    public interface IStringBuffer
    {
        IStringBufferAllocation Allocate();
    }
}
