using System.Text;

namespace Whitelog.Interface
{
    public interface IStringRenderer
    {
        void Render(object data, StringBuilder stringBuilder);
    }
}