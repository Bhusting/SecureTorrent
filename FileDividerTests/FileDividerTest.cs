using System;
using System.IO;
using System.Threading.Tasks;
using Domain;
using Domain.Files;
using Xunit;

namespace FileDividerTests
{
    public class FileDividerTest
    {
        [Fact]
        public async Task DivideAndSaveFile()
        {
            var fileStream =
                File.OpenRead(@"C:\repos\SecureTorrent\SecureTorrent\FileDividerTests\TestFolder\test.txt");

            await FileSplitter.SplitAndSaveFile(fileStream, "test", @"C:\repos\SecureTorrent\SecureTorrent\FileDividerTests\TestFolder\", ".txt");
        } 
         
        [Fact]
        public async Task DivideFile()
        {
        }
    }
}
