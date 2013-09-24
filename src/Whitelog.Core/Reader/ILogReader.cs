using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Whitelog.Core.FileLog;
using Whitelog.Core.ListWriter;
using Whitelog.Core.Reader.ExpendableList;
using Whitelog.Core.Serializer.MemoryBuffer;

namespace Whitelog.Core.Reader
{
    public interface ILogReader
    {        
        bool TryRead();
    }
}
