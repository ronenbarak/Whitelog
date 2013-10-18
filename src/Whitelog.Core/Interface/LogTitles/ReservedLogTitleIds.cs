
namespace Whitelog.Interface.LogTitles
{
    public class ReservedLogTitleIds
    {
        public const long ReservedLogTitle = 1 << 62;

        public const long Fatal     = (1 << 0) | ReservedLogTitle;
        public const long Error     = (1 << 1) | ReservedLogTitle;
        public const long Warning   = (1 << 2) | ReservedLogTitle;
        public const long Info      = (1 << 3) | ReservedLogTitle;
        public const long Debug     = (1 << 4) | ReservedLogTitle;
        public const long Trace     = (1 << 5) | ReservedLogTitle;

        public const long Open      = (1 << 6) | ReservedLogTitle;
        public const long Close     = (1 << 7) | ReservedLogTitle;
        public const long Custom    = (1 << 8) | ReservedLogTitle;


        public const long All = Fatal | Error | Warning | Info | Debug | Trace | Open | Close | Custom;
    }
}
