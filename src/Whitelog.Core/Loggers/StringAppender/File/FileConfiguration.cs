namespace Whitelog.Core.Loggers.StringAppender.File
{
    public class FileConfiguration
    {
        public FileConfiguration()
        {
            ArchiveNumbering = ArchiveNumberingOptions.Sequence;
            ReplaceFileContentsOnEachWrite = false;
            AppendToEnd = false;
        }
        
        public string FilePath { get; set; }
        public long? ArchiveAboveSize { get; set; }
        public int? MaxArchiveFiles { get; set; }
        public string ArchiveFilePath { get; set; }
        public ArchiveNumberingOptions ArchiveNumbering { get; set; }
        public ArchiveOptions? ArchiveEvery { get; set; }
        public bool ReplaceFileContentsOnEachWrite { get; set; }
        public bool AppendToEnd { get; set; }
    }
}