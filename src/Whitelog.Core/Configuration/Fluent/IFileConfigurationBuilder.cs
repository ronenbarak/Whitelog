using Whitelog.Core.File;

namespace Whitelog.Core.Configuration.Fluent
{
    public interface IFileConfigurationBuilder
    {
        IFileConfigurationBuilder FilePath(string file);
        IFileConfigurationBuilder ArchiveFilePath(string file);
        IFileConfigurationBuilder DontArchive { get; }
        IFileConfigurationBuilder ArchiveEvery(ArchiveOptions archiveOptions);
        IFileConfigurationBuilder ArchiveEvery(long size);
        IFileConfigurationBuilder MaxArchiveFiles(int maxFiles);
        IFileConfigurationBuilder Append { get; }
    }

    class FileConfigurationBuilder : IFileConfigurationBuilder
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

        public IFileConfigurationBuilder FilePath(string file)
        {
            m_fileConfiguration.FilePath = file;
            return this;
        }

        public IFileConfigurationBuilder ArchiveFilePath(string file)
        {
            m_fileConfiguration.ArchiveFilePath = file;
            return this;
        }

        public IFileConfigurationBuilder DontArchive { get { m_fileConfiguration.Archive = false; return this; } }
        public IFileConfigurationBuilder ArchiveEvery(ArchiveOptions archiveOptions)
        {
            m_fileConfiguration.ArchiveEvery = archiveOptions;
            return this;
        }

        public IFileConfigurationBuilder MaxArchiveFiles(int maxFiles)
        {
            m_fileConfiguration.MaxArchiveFiles = maxFiles;
            return this;
        }

        public IFileConfigurationBuilder ArchiveEvery(long size)
        {
            m_fileConfiguration.ArchiveAboveSize = size;
            return this;
        }

        public IFileConfigurationBuilder Append { get { m_fileConfiguration.AppendToEnd = true; return this;} }
    }
}