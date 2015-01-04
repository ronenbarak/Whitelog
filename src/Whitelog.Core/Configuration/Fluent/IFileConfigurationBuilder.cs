using System.Security.Cryptography.X509Certificates;
using Whitelog.Core.File;

namespace Whitelog.Core.Configuration.Fluent
{
    public interface IArchiveOptions
    {
        IFileConfigurationBuilder Hour {get;}
        IFileConfigurationBuilder Day { get; }
        IFileConfigurationBuilder Week { get; }
        IFileConfigurationBuilder Month { get; }
        IFileConfigurationBuilder Size(long size);
    }

    public interface IFileConfigurationBuilder
    {
        IFileConfigurationBuilder Path(string file);
        IFileConfigurationBuilder ArchivePath(string file);
        IFileConfigurationBuilder DontArchive { get; }
        IArchiveOptions ArchiveEvery { get; }
        IFileConfigurationBuilder MaxArchiveFiles(int maxFiles);
        IFileConfigurationBuilder Append { get; }
    }

    class FileConfigurationBuilder : IFileConfigurationBuilder , IArchiveOptions
    {
        private FileConfiguration m_fileConfiguration;

        public FileConfigurationBuilder(string filePath)
        {
            m_fileConfiguration = new FileConfiguration()
                                  {
                                      FilePath = filePath,
                                  };
        }
        public FileConfiguration GetFileConfiguration()
        {
            return m_fileConfiguration;
        }

        public IFileConfigurationBuilder Path(string file)
        {
            m_fileConfiguration.FilePath = file;
            return this;
        }

        public IFileConfigurationBuilder ArchivePath(string file)
        {
            m_fileConfiguration.ArchiveFilePath = file;
            return this;
        }

        public IFileConfigurationBuilder DontArchive { get { m_fileConfiguration.Archive = false; return this; } }
        public IArchiveOptions ArchiveEvery { get { return this; } }

        public IFileConfigurationBuilder MaxArchiveFiles(int maxFiles)
        {
            m_fileConfiguration.MaxArchiveFiles = maxFiles;
            return this;
        }

        public IFileConfigurationBuilder Append { get { m_fileConfiguration.AppendToEnd = true; return this; } }

        #region IArchiveOptions

        public IFileConfigurationBuilder Hour { get {m_fileConfiguration.ArchiveEvery = ArchiveOptions.Hour; return this;} }
        public IFileConfigurationBuilder Day { get { m_fileConfiguration.ArchiveEvery = ArchiveOptions.Day; return this; } }
        public IFileConfigurationBuilder Week { get { m_fileConfiguration.ArchiveEvery = ArchiveOptions.Week; return this; } }
        public IFileConfigurationBuilder Month { get { m_fileConfiguration.ArchiveEvery = ArchiveOptions.Month; return this; } }
        public IFileConfigurationBuilder Size(long size)
        {
            m_fileConfiguration.ArchiveAboveSize = size; 
            return this;
        }
        #endregion
    }
}