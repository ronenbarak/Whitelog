using Whitelog.Barak.Common.ExtensionMethods;
using Whitelog.Interface;
using Whitelog.Interface.LogTitles;

namespace Whitelog.Core.PakageDefinitions.Pack
{
    public class OpenLogScopeTitlePackageDefinition : PackageDefinition<OpenLogScopeTitle>
    {
        public OpenLogScopeTitlePackageDefinition()
        {
            DefineCacheString(ObjectHelper.GetProperty<StringLogTitle>(x => x.Message).Name, title => title.Message);
            Define(x => x.ParentLogId, x => x.ParentLogId);
        }
    }
}