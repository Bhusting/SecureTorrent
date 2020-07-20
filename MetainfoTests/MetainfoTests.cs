using Domain.Files;
using System;
using System.IO;
using System.Threading.Tasks;
using Domain;
using Xunit;

namespace MetainfoTests
{
    public class MetainfoTests
    {
        [Fact]
        public async Task BuildMetainfoFile()
        {

            var fileStream =
                File.OpenRead(@"C:\repos\SecureTorrent\FileDividerTests\TestFolder\test.txt");

            var streams = await FileSplitter.SplitFile(fileStream);

            var info = new SingleFileInfo("test", (int)fileStream.Length, streams.Item1, streams.Item2);

            var metaInfo = new Metainfo(info, "http://localhost:8000");

            var bencodedMetaInfo = metaInfo.Bencode();

            var deserializedMetainfo = Metainfo.Deserialize(bencodedMetaInfo);

            Assert.True(metaInfo.Info.Pieces[0] == deserializedMetainfo.Info.Pieces[0]);
            Assert.True(metaInfo.Info.Pieces[1] == deserializedMetainfo.Info.Pieces[1]);
            Assert.True(metaInfo.Info.Pieces[2] == deserializedMetainfo.Info.Pieces[2]);
            Assert.True(metaInfo.Info.Pieces[3] == deserializedMetainfo.Info.Pieces[3]);
            Assert.True(metaInfo.Info.Pieces[4] == deserializedMetainfo.Info.Pieces[4]);
            Assert.True(metaInfo.Info.Pieces[5] == deserializedMetainfo.Info.Pieces[5]);
        }
    }
}
