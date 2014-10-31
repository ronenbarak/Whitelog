using System;
using System.Dynamic;
using System.IO;
using NUnit.Framework;
using Whitelog.Core.File;

namespace Whitelog.Tests
{
	[TestFixture]
	public class FileHandlerTests
	{
		private void Delete(string filePath)
		{
			try
			{
				System.IO.File.Delete(filePath);
			}
			catch (Exception e)
			{

			}
		}

		private void DeleteDir(string dir2)
		{
			try
			{
				System.IO.Directory.Delete(dir2);
			}
			catch (Exception e)
			{

			}
		}

		[Test]
		public void CreateAFileFromEmptyConfigurationWorks()
		{
			Delete("Log.txt");
			Delete("Log.1.txt");
			var fileStreamProvider = new FileStreamProvider(new FileConfiguration());
			using (new FileStreamProvider(new FileConfiguration()).GetStream())
			{   
			}

			// Archive
			using (new FileStreamProvider(new FileConfiguration()).GetStream())
			{
			}

			Assert.IsTrue(System.IO.File.Exists("Log.txt"));
			Assert.IsTrue(System.IO.File.Exists("Log.1.txt"));
		}

		[Test]
		public void CreateAFileFromUsingTemplatesWorks()
		{
			Delete(string.Format("MyLog-{0}.txt", DateTime.Now.ToString("yyyy.MM.dd")));

			using (new FileStreamProvider(new FileConfiguration(){
				FilePath = string.Format(@"{{baseDir}}{0}MyLog-{{date}}.txt",Path.DirectorySeparatorChar),
			}).GetStream())
			{
			}

			Assert.IsTrue(System.IO.File.Exists(string.Format("MyLog-{0}.txt", DateTime.Now.ToString("yyyy.MM.dd"))));
		}

		[Test]
		public void ArchiveingAFileUsingSequenceWorks()
		{
			Delete("MyLog.Tests.2.txt");
			Delete("MyLog.Tests.1.txt");
			Delete("MyLog.Tests.txt");

			using (var file = new FileStreamProvider(new FileConfiguration()
				{
					FilePath = @"MyLog.Tests.txt",
					ArchiveNumbering = ArchiveNumberingOptions.Sequence,
					Archive = true,
				}).GetStream())
			{
				file.Write(new byte[1024],0,1024);
			}


			using (var file = new FileStreamProvider(new FileConfiguration()
				{
					FilePath = @"MyLog.Tests.txt",
					ArchiveNumbering = ArchiveNumberingOptions.Sequence,
					Archive = true,
				}).GetStream())
			{
				file.Write(new byte[100], 0, 100);
			}

			Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.txt"));
			Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.1.txt"));

			Assert.AreEqual(100, new FileInfo("MyLog.Tests.txt").Length);
			Assert.AreEqual(1024, new FileInfo("MyLog.Tests.1.txt").Length);


			using (new FileStreamProvider(new FileConfiguration()
				{
					FilePath = @"MyLog.Tests.txt",
					ArchiveNumbering = ArchiveNumberingOptions.Sequence,
					Archive = true,
				}).GetStream())
			{

			}

			Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.2.txt"));

			Assert.AreEqual(0, new FileInfo("MyLog.Tests.txt").Length);
			Assert.AreEqual(100, new FileInfo("MyLog.Tests.2.txt").Length);
			Assert.AreEqual(1024, new FileInfo("MyLog.Tests.1.txt").Length);

			Delete("MyLog.Tests.2.txt");
			Delete("MyLog.Tests.1.txt");
			Delete("MyLog.Tests.txt");

		}

		[Test]
		public void ArchivingToNonExsisitngDirectory()
		{
			Delete(string.Format(@"Dir{0}MyLog.Tests.txt",Path.DirectorySeparatorChar));
			Delete(string.Format(@"Dir{0}MyLog.Tests.1.txt", Path.DirectorySeparatorChar));
			Delete(string.Format(@"Dir2{0}MyLog.Tests.1.txt", Path.DirectorySeparatorChar));
			DeleteDir("Dir");
			DeleteDir("Dir2");

			using (var file = new FileStreamProvider(new FileConfiguration()
				{
					FilePath = string.Format(@"Dir{0}MyLog.Tests.txt",Path.DirectorySeparatorChar),
					ArchiveFilePath = string.Format(@"Dir2{0}MyLog.Tests.txt", Path.DirectorySeparatorChar),
					ArchiveNumbering = ArchiveNumberingOptions.Sequence,
					Archive = true,
				}).GetStream()) ;

			using (var file = new FileStreamProvider(new FileConfiguration()
				{
					FilePath = string.Format(@"Dir{0}MyLog.Tests.txt",Path.DirectorySeparatorChar),
					ArchiveFilePath =  "Dir2" + Path.DirectorySeparatorChar + "{SOURCENAME}",
					ArchiveNumbering = ArchiveNumberingOptions.Sequence,
					Archive = true,
				}).GetStream()) ;

			Assert.IsTrue(System.IO.File.Exists(string.Format(@"Dir{0}MyLog.Tests.txt",Path.DirectorySeparatorChar)));
			Assert.IsTrue(System.IO.File.Exists(string.Format(@"Dir2{0}MyLog.Tests.1.txt", Path.DirectorySeparatorChar)));

		}

		[Test]
		public void ArchiveAFileWhenTimeIsDue()
		{
			Delete("MyLog.Tests.1.txt");
			Delete("MyLog.Tests.txt");

			using (var file = new FileStreamProvider(new FileConfiguration()
				{
					FilePath = @"MyLog.Tests.txt",
					ArchiveNumbering = ArchiveNumberingOptions.Sequence,
					Archive = true,
					AppendToEnd = true,
					ArchiveEvery = ArchiveOptions.Month,
				}).GetStream())
			{
				file.Write(new byte[1024], 0, 1024);
			}

			using (var file = new FileStreamProvider(new FileConfiguration()
				{
					FilePath = @"MyLog.Tests.txt",
					ArchiveNumbering = ArchiveNumberingOptions.Sequence,
					Archive = true,
					AppendToEnd = true,
					ArchiveEvery = ArchiveOptions.Month,
				}).GetStream())
			{
				Assert.AreEqual(1024, file.Length);
			}

			Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.txt"));
			Assert.IsFalse(System.IO.File.Exists("MyLog.Tests.1.txt"));

			// Not working on mono:
			new FileInfo("MyLog.Tests.txt").CreationTime = DateTime.Now.Subtract(TimeSpan.FromDays(100));

			using (var file = new FileStreamProvider(new FileConfiguration()
				{
					FilePath = @"MyLog.Tests.txt",
					ArchiveNumbering = ArchiveNumberingOptions.Sequence,
					Archive = true,
					AppendToEnd = true,
					ArchiveEvery = ArchiveOptions.Month,
				}).GetStream())
			{
				Assert.AreEqual(0, file.Length);
			}

			Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.txt"));
			Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.1.txt"));

			Assert.AreEqual(0, new FileInfo("MyLog.Tests.txt").Length);
			Assert.AreEqual(1024, new FileInfo("MyLog.Tests.1.txt").Length);

		}
	}
}