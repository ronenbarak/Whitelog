using System;
using System.IO;

namespace Whitelog.Core.File
{
    public enum ArchiveOptions
    {
        Hour,
        Day,
        Week,
        Month,
    }

    public enum ArchiveNumberingOptions
    {
        Rolling,
        Sequence,
    }

    public interface IStreamProvider
    {
        Stream GetStream();
        bool ShouldArchive(long currSize, int bytesToAdd,DateTime now);
        void Archive();
    }

    public class FileConfiguration
    {
        public FileConfiguration()
        {
            ArchiveNumbering = ArchiveNumberingOptions.Sequence;
            AppendToEnd = false;
            Archive = true;
        }
        
        public string FilePath { get; set; }
        public bool Archive { get; set; }
        public long? ArchiveAboveSize { get; set; }
        public int? MaxArchiveFiles { get; set; }
        public string ArchiveFilePath { get; set; }
        public ArchiveNumberingOptions ArchiveNumbering { get; set; }
        public ArchiveOptions? ArchiveEvery { get; set; }
        public bool AppendToEnd { get; set; }

        public FileConfiguration Clone()
        {
            return new FileConfiguration()
            {
                Archive = Archive,
                AppendToEnd = AppendToEnd,
                ArchiveAboveSize = ArchiveAboveSize,
                ArchiveEvery = ArchiveEvery,
                ArchiveFilePath = ArchiveFilePath,
                ArchiveNumbering = ArchiveNumbering,
                FilePath = FilePath,
                MaxArchiveFiles = MaxArchiveFiles                
            };
        }
    }
}