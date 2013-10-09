using System.Text;

namespace Whitelog.Core.String
{
    public interface IStringRenderer
    {
        void Render(object data, StringBuilder stringBuilder);
    }
}