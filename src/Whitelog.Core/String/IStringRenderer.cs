using System.Text;

namespace Whitelog.Core
{
    public interface IStringRenderer
    {
        void Render(object data, StringBuilder stringBuilder);
    }
}