using System.Collections.Generic;

namespace Whitelog.Core.String.Layout.StringLayoutFactory
{
    public static class AllLayoutFactories
    {
        public static readonly IEnumerable<IStringLayoutFactory> Factories = new IStringLayoutFactory[]
                                                                             {
                                                                                 new DateStringLayoutFactory(),
                                                                                 new NewLineStringLayoutFactory(),
                                                                                 new ObjectStringLayoutFactory(),
                                                                                 new ScopeIdStringLayoutFactory(),
                                                                                 new ThreadIdStringLayoutFactory(),
                                                                                 new TitleStringLayoutFactory(),
                                                                             };
    }
}