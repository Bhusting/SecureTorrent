using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Metainfo
    {
        public string Announce { get; set; }

        public SingleFileInfo Info { get; set; }

        public Metainfo(SingleFileInfo info, string announce)
        {
            Info = info;
            Announce = announce;
        }

        
        public string Bencode()
        {
            var dict = new BencodedDictionary();

            dict.Add("Announce", Announce);
            dict.Add("Info", Info.GenerateDictionary());

            return dict.Build();
        }

        // TODO: Separate the deserializing of each dictionary item into their own function. Possibly into the Bencoder class?
        public static Metainfo Deserialize(string bencodedMetainfo)
        {
            var pos = 1;

            int size = 0;
            var str = new StringBuilder();

            // Skip over 8:Announce
            pos += 10;

            while (true)
            {
                if (bencodedMetainfo[pos] != ':')
                {
                    str.Append(bencodedMetainfo[pos]);
                    pos++;
                }
                else
                {
                    size = int.Parse(str.ToString());
                    pos++;
                    break;
                }
            }

            str = new StringBuilder();
            var current = pos;
            for (; pos < (current + size); pos++)
            {
                str.Append(bencodedMetainfo[pos]);
            }

            var announce = str.ToString();

            str = new StringBuilder();

            for (; pos < bencodedMetainfo.Length; pos++)
            {
                str.Append(bencodedMetainfo[pos]);
            }

            var info = SingleFileInfo.Deserialize(str.ToString());

            var metainfo = new Metainfo(info, announce);

            return metainfo;
        }
        
    }
}
