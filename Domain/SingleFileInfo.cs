using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain
{
    public class SingleFileInfo
    {
        public string FileName { get; set; }

        public int FileLength { get; set; }

        public int PieceLength { get; set; }
        
        public List<string> Pieces { get; set; }
        
        public SingleFileInfo(string fileName, int fileLength, int pieceLength, Stream[] streams)
        {
            FileName = fileName;
            FileLength = fileLength;
            PieceLength = pieceLength;
            Pieces = HashStreams(streams);
        }

        public SingleFileInfo(string fileName, int fileLength, int pieceLength, List<string> pieces)
        {
            FileName = fileName;
            FileLength = fileLength;
            PieceLength = pieceLength;
            Pieces = pieces;
        }

        public List<string> HashStreams(Stream[] streams)
        {
            var hashes = new List<string>();

            foreach (var stream in streams)
            {

                using (var bs = new BufferedStream(stream))
                {
                    using (var sha1 = new SHA1Managed())
                    {
                        
                        var hash = sha1.ComputeHash(bs);

                        var str = new StringBuilder();
                        foreach (var x in hash)
                        {
                            str.Append(x.ToString("X2"));
                        }

                        hashes.Add(str.ToString());
                    }
                }
            }

            return hashes;
        }

        public BencodedDictionary GenerateDictionary()
        {
            var dict = new BencodedDictionary();

            dict.Add("FileName", FileName);
            dict.Add("FileLength", FileLength);
            dict.Add("PieceLength", PieceLength);

            var str = new StringBuilder();
            foreach (var piece in Pieces)
            {
                str.Append(piece);
            }

            dict.Add("Pieces", str.ToString());
            
            return dict;
        }

        // TODO: Convert to using generic deserializer
        public static SingleFileInfo Deserialize(string bencodedInfo)
        {

            var pos = 6;

            int size = 0;
            var str = new StringBuilder();

            // Skip 8:FileName
            pos += 11;

            // Read size of FileName
            while (true)
            {
                if (bencodedInfo[pos] != ':')
                {
                    str.Append(bencodedInfo[pos]);
                    pos++;
                }
                else
                {
                    size = int.Parse(str.ToString());
                    pos++;
                    break;
                }
            }

            // Read FileName
            str = new StringBuilder();
            var current = pos;
            for (; pos < (current + size); pos++)
            {
                str.Append(bencodedInfo[pos]);
            }
            
            var fileName = str.ToString();

            // Skip 10:FileLengthi
            pos += 14;

            str = new StringBuilder();

            // Read FileLength
            for (; bencodedInfo[pos] != 'e'; pos++)
            {
                str.Append(bencodedInfo[pos]);
            }

            var fileLength = int.Parse(str.ToString());

            // Skip 11:PieceLengthi
            pos += 16;

            str = new StringBuilder();
            // Read FileLength
            for (; bencodedInfo[pos] != 'e'; pos++)
            {
                str.Append(bencodedInfo[pos]);
            }

            var pieceLength = int.Parse(str.ToString());

            var pieces = new List<string>();

            // Skip over e6:Pieces
            pos += 9;
            
            str = new StringBuilder();

            while (true)
            {
                if (bencodedInfo[pos] != ':')
                {
                    str.Append(bencodedInfo[pos]);
                    pos++;
                }
                else
                {
                    size = int.Parse(str.ToString());
                    pos++;
                    break;
                }
            }

            var x = bencodedInfo[pos];

            while (pos != bencodedInfo.Length - 2)
            {
                str = new StringBuilder();
                for (size = 0; size < 40; size++)
                {
                    str.Append(bencodedInfo[pos]);
                    pos++;
                }
                pieces.Add(str.ToString());
            }

            var info = new SingleFileInfo(fileName, fileLength, pieceLength, pieces);

            return info;
        }
    }
}
