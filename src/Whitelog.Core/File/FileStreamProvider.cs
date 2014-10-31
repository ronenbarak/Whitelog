using System;
using System.IO;

namespace Whitelog.Core.File
{
	public class FileStreamProvider : IStreamProvider
	{
		private FileConfiguration m_configuration;
		private DateTime? m_nextArchive;
		public string FileName { get; private set; }

		public FileStreamProvider(FileConfiguration configuration)
		{
			m_configuration = configuration;
			FileName = GetFileName(configuration);
		}

		public Stream GetStream()
		{
			var filePath = GetFileName(m_configuration);
			if (System.IO.File.Exists(filePath))
			{
				if (m_configuration.AppendToEnd)
				{
					if (m_configuration.ArchiveEvery.HasValue)
					{
						var originalCreationDate = System.IO.File.GetCreationTime(filePath);
						if ((m_configuration.ArchiveEvery.Value == ArchiveOptions.Hour && originalCreationDate.AddHours(1) > DateTime.Now) ||
							(m_configuration.ArchiveEvery.Value == ArchiveOptions.Day && originalCreationDate.AddDays(1) > DateTime.Now) ||
							(m_configuration.ArchiveEvery.Value == ArchiveOptions.Week && originalCreationDate.AddDays(7) > DateTime.Now) ||
							(m_configuration.ArchiveEvery.Value == ArchiveOptions.Month && originalCreationDate.AddMonths(1) > DateTime.Now))
						{
							return CreateFile(filePath);
						}
					}
					else
					{
						return CreateFile(filePath);
					}
				}

				if (!m_configuration.Archive)
				{
					throw new Exception(string.Format("The file '{0}' already exist and not configured to be archived", filePath));
				}

				ArchiveFile(m_configuration, filePath);
				return CreateFile(filePath);
			}
			else
			{
				return CreateFile(filePath);
			}
		}

		public bool ShouldArchive(long currSize, int bytesToAdd, DateTime now)
		{
			if (m_configuration.ArchiveAboveSize.HasValue)
			{
				if ((currSize + bytesToAdd) > m_configuration.ArchiveAboveSize.Value)
				{
					return true;
				}
			}

			if (m_configuration.ArchiveEvery.HasValue)
			{
				if (!m_nextArchive.HasValue)
				{
					m_nextArchive = GetNextArchiveTime(m_configuration, now);
				}


				if (now > m_nextArchive.Value)
				{
					return true;
				}
			}

			return false;
		}

		public void Archive()
		{
			var filePath = GetFileName(m_configuration);
			if (System.IO.File.Exists(filePath))
			{
				ArchiveFile(m_configuration, filePath);
			}
		}

		private static void ArchiveFile(FileConfiguration configuration, string filePath)
		{
			string archiveFilePath = GetArchiveFileName(configuration, filePath);

			if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(archiveFilePath))))
			{
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(archiveFilePath)));
			}

			if (configuration.ArchiveNumbering == ArchiveNumberingOptions.Sequence)
			{
				var filesMatchDescription = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(archiveFilePath)),
					System.IO.Path.GetFileNameWithoutExtension(archiveFilePath) + ".*" + System.IO.Path.GetExtension(archiveFilePath));

				int maxIndexFound = 0;
				foreach (var currFile in filesMatchDescription)
				{
					var start = System.IO.Path.GetFileNameWithoutExtension(archiveFilePath).Length + 1;
					var legnth = System.IO.Path.GetFileName(currFile).Length - start - System.IO.Path.GetExtension(archiveFilePath).Length;

					var sequenceString = System.IO.Path.GetFileName(currFile).Substring(System.IO.Path.GetFileNameWithoutExtension(archiveFilePath).Length + 1, legnth);
					int sequenceNumber;
					if (int.TryParse(sequenceString, out sequenceNumber))
					{
						if (sequenceNumber > maxIndexFound)
						{
							maxIndexFound = sequenceNumber;
						}
					}
				}

				// The file now archived.
				System.IO.File.Move(filePath, System.IO.Path.Combine(System.IO.Path.GetDirectoryName(archiveFilePath), System.IO.Path.GetFileNameWithoutExtension(archiveFilePath)) + "." + (maxIndexFound + 1) + System.IO.Path.GetExtension(archiveFilePath));
			}
		}

		public static DateTime? GetNextArchiveTime(FileConfiguration configuration,DateTime now)
		{
			if (!configuration.ArchiveEvery.HasValue)
			{
				return null;
			}
			else
			{
				switch (configuration.ArchiveEvery.Value)
				{
				case ArchiveOptions.Hour:
					return new DateTime(now.Year,now.Month,now.Day,now.Hour,0,0).AddHours(1);
				case ArchiveOptions.Day:
					return now.Date.AddDays(1);
				case ArchiveOptions.Week:
					return now.Date.AddDays(7);
				case ArchiveOptions.Month:
					return new DateTime(now.Year, now.Month, 1).AddMonths(1);
				}
			}

			return null;
		}

		private static string GetArchiveFileName(FileConfiguration configuration, string originalPath)
		{
			string archiveFormat = configuration.ArchiveFilePath;
			if (string.IsNullOrWhiteSpace(archiveFormat))
			{
				archiveFormat = configuration.FilePath;
				if (string.IsNullOrWhiteSpace(archiveFormat))
				{
					archiveFormat = string.Format(@"{{SOURCEPATH}}{0}{{SOURCENAME}}",Path.DirectorySeparatorChar);
				}
			}

			string archiveFilePath = string.Empty;

			foreach (var part in StringParser.GetParts(archiveFormat))
			{
				if (part.IsConst)
				{
					archiveFilePath += part.Value;
				}
				else
				{
					switch (part.Value.ToUpper())
					{
					case "BASEDIR":
						archiveFilePath += System.AppDomain.CurrentDomain.BaseDirectory;
						break;
					case "WORKDIR":
						archiveFilePath += Environment.CurrentDirectory;
						break;
					case "USERDIR":
						archiveFilePath += Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
						break;
					case "DATE":
						archiveFilePath += DateTime.Now.ToString("yyyy.MM.dd");
						break;
					case "DATETIME":
						archiveFilePath += DateTime.Now.ToString("yyyy.MM.dd HH.mm.ss");
						break;
					case "SOURCENAME":
						archiveFilePath += System.IO.Path.GetFileName(originalPath);
						break;
					case "SOURCEPATH":
						archiveFilePath += System.IO.Path.GetDirectoryName(originalPath);
						break;
					}
				}
			}

			return archiveFilePath;
		}

		private static string GetFileName(FileConfiguration configuration)
		{
			string baseFilePath = configuration.FilePath;
			if (string.IsNullOrWhiteSpace(configuration.FilePath))
			{
				baseFilePath = string.Format(@"${{BaseDir}}{0}Log.txt",Path.DirectorySeparatorChar);
			}
			string filePath = string.Empty;

			foreach (var part in StringParser.GetParts(baseFilePath))
			{
				if (part.IsConst)
				{
					filePath += part.Value;
				}
				else
				{
					switch (part.Value.ToUpper())
					{
					case "BASEDIR":
						filePath += System.AppDomain.CurrentDomain.BaseDirectory;
						break;
					case "WORKDIR":
						filePath += Environment.CurrentDirectory;
						break;
					case "USERDIR":
						filePath += Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
						break;
					case "DATE":
						filePath += DateTime.Now.Date.ToString("yyyy.MM.dd");
						break;
					case "DATETIME":
						filePath += DateTime.Now.ToString("yyyy.MM.dd HH.mm.ss");
						break;
					}
				}
			}
			return filePath;
		}

		private FileStream CreateFile(string filePath)
		{
			if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(filePath))))
			{
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(filePath)));
			}
			// http://support.microsoft.com/?kbid=172190
			// http://stackoverflow.com/questions/2109152/unbelievable-strange-file-creation-time-problem
			// there is a bug with the creation time, to overcome this problem we need to change the file creation time manualy
			FileName = filePath;
			using (var strem = System.IO.File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
			{

			}
			new FileInfo(FileName).CreationTime = DateTime.Now;
			return System.IO.File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
		}
	}
}