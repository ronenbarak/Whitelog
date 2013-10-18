namespace Whitelog.Core.Loggers.StringAppender.File
{
    public class FileConfiguration
    {
        public FileConfiguration()
        {
            ArchiveNumbering = ArchiveNumberingOptions.Sequence;
            ReplaceFileContentsOnEachWrite = false;
            DirPath = ".";
        }
        
        public string FileName { get; set; }
        public string DirPath { get; set; }
        public long? ArchiveAboveSize { get; set; }
        public int? MaxArchiveFiles { get; set; }
        public string ArchiveFileName { get; set; }
        public ArchiveNumberingOptions ArchiveNumbering { get; set; }
        public ArchiveOptions? ArchiveEvery { get; set; }
        public bool ReplaceFileContentsOnEachWrite { get; set; }
    }
}