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
            Delete("Log.Txt");
            Delete("Log.1.Txt");
            var fileStreamProvider = new FileStreamProvider(new FileConfiguration());
            using (new FileStreamProvider(new FileConfiguration()).GetStream())
            {   
            }

            // Archive
            using (new FileStreamProvider(new FileConfiguration()).GetStream())
            {
            }

            Assert.IsTrue(System.IO.File.Exists("Log.Txt"));
            Assert.IsTrue(System.IO.File.Exists("Log.1.Txt"));
        }

        [Test]
        public void CreateAFileFromUsingTemplatesWorks()
        {
            Delete(string.Format("MyLog-{0}.Txt", DateTime.Now.ToString("yyyy.MM.dd")));

            using (new FileStreamProvider(new FileConfiguration(){
                                                FilePath = @"{baseDir}\MyLog-{date}.txt",
                                                                 }).GetStream())
            {
            }

            Assert.IsTrue(System.IO.File.Exists(string.Format("MyLog-{0}.Txt", DateTime.Now.ToString("yyyy.MM.dd"))));
        }

        [Test]
        public void ArchiveingAFileUsingSequenceWorks()
        {
            Delete("MyLog.Tests.2.Txt");
            Delete("MyLog.Tests.1.Txt");
            Delete("MyLog.Tests.Txt");

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

            Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.Txt"));
            Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.1.Txt"));

            Assert.AreEqual(100, new FileInfo("MyLog.Tests.Txt").Length);
            Assert.AreEqual(1024, new FileInfo("MyLog.Tests.1.Txt").Length);


            using (new FileStreamProvider(new FileConfiguration()
                                         {
                                             FilePath = @"MyLog.Tests.Txt",
                                             ArchiveNumbering = ArchiveNumberingOptions.Sequence,
                                             Archive = true,
                                         }).GetStream())
            {
                
            }

            Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.2.Txt"));

            Assert.AreEqual(0, new FileInfo("MyLog.Tests.Txt").Length);
            Assert.AreEqual(100, new FileInfo("MyLog.Tests.2.Txt").Length);
            Assert.AreEqual(1024, new FileInfo("MyLog.Tests.1.Txt").Length);

            Delete("MyLog.Tests.2.Txt");
            Delete("MyLog.Tests.1.Txt");
            Delete("MyLog.Tests.Txt");

        }

        [Test]
        public void ArchivingToNonExsisitngDirectory()
        {
            Delete(@"Dir\MyLog.Tests.txt");
            Delete(@"Dir\MyLog.Tests.1.txt");
            Delete(@"Dir2\MyLog.Tests.1.txt");
            DeleteDir("Dir");
            DeleteDir("Dir2");

            using (var file = new FileStreamProvider(new FileConfiguration()
                                                    {
                                                        FilePath = @"Dir\MyLog.Tests.txt",
                                                        ArchiveFilePath = @"Dir2\MyLog.Tests.txt",
                                                        ArchiveNumbering = ArchiveNumberingOptions.Sequence,
                                                        Archive = true,
                                                    }).GetStream()) ;

            using (var file = new FileStreamProvider(new FileConfiguration()
                                                    {
                                                        FilePath = @"Dir\MyLog.Tests.txt",
                                                        ArchiveFilePath = @"Dir2\{SOURCENAME}",
                                                        ArchiveNumbering = ArchiveNumberingOptions.Sequence,
                                                        Archive = true,
                                                    }).GetStream()) ;

            Assert.IsTrue(System.IO.File.Exists(@"Dir\MyLog.Tests.txt"));
            Assert.IsTrue(System.IO.File.Exists(@"Dir2\MyLog.Tests.1.txt"));

        }

        [Test]
        public void ArchiveAFileWhenTimeIsDue()
        {
            Delete("MyLog.Tests.1.Txt");
            Delete("MyLog.Tests.Txt");

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

            Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.Txt"));
            Assert.IsFalse(System.IO.File.Exists("MyLog.Tests.1.Txt"));

            new FileInfo("MyLog.Tests.Txt").CreationTime = DateTime.Now.Subtract(TimeSpan.FromDays(100));

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

            Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.Txt"));
            Assert.IsTrue(System.IO.File.Exists("MyLog.Tests.1.Txt"));

            Assert.AreEqual(0, new FileInfo("MyLog.Tests.Txt").Length);
            Assert.AreEqual(1024, new FileInfo("MyLog.Tests.1.Txt").Length);

        }
    }
}