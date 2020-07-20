using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Files
{
    public static class FileSplitter
    {

        public static async Task SplitAndSaveFile(Stream fileStream, string fileName, string outputPath, string fileExtension)
        {
            var data = PrepareFile(fileStream);
            

            await SplitStream(fileStream, data.Item1, data.Item2, fileName, outputPath, fileExtension);
        }

        public static async Task<Tuple<int, Stream[]>> SplitFile(Stream fileStream)
        {
            var data = PrepareFile(fileStream);

            var streams = await SplitStream(fileStream, data.Item1, data.Item2);

            var result = new Tuple<int, Stream[]>(data.Item1, streams);

            return result; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream">File Stream</param>
        /// <returns>Tuple with the size of each part and the number of parts.</returns>
        public static Tuple<int, int> PrepareFile(Stream fileStream)
        {
            long partsLength = 0;
            var numberToSplitStream = 6;
            while (true)
            {
                partsLength = fileStream.Length / numberToSplitStream;
                partsLength = (int)partsLength + 1;

                if (partsLength <= 512000)
                {
                    break;
                }
                else
                {
                    numberToSplitStream++;
                }
            }

            return new Tuple<int, int>((int)partsLength, numberToSplitStream);
        }

        public static async Task SplitStream(Stream stream, int partSize, int partCount, string fileName, string outputPath, string fileExtension)
        {
            var offset = 0;
            for (int partIndex = 0; partIndex < partCount; partIndex++)
            {
                var filePart = new byte[partSize];
                for (var x = 0; x < partSize; x++)
                {
                    filePart[x] = (byte)stream.ReadByte();
                    offset++;
                }

                await using (var fileStream = File.OpenWrite($"{outputPath}/{fileName}-{partIndex}{fileExtension}"))
                {
                    await fileStream.WriteAsync(filePart, 0, partSize);
                }
            }
        }

        public static async Task<Stream[]> SplitStream(Stream mainStream, int partSize, int partCount)
        {
            var result = new Stream[partCount];

            var pos = 0;
            var offset = 0;
            for (int partIndex = 0; partIndex < partCount; partIndex++)
            {
                var filePart = new byte[partSize];
                for (var x = 0; x < partSize; x++)
                {
                    filePart[x] = (byte)mainStream.ReadByte();
                    offset++;
                }
                
                result[pos] = new MemoryStream(filePart);
                pos++;
            }

            return result;
        }
    }
}
