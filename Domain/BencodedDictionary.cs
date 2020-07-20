using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class BencodedDictionary
    {

        private StringBuilder _result = new StringBuilder("d");

        public void Add(string key, string value)
        {

            _result.Append(Bencoder.ConvertByteString(key));
            _result.Append(Bencoder.ConvertByteString(value));
        }

        public void Add(string key, int value)
        {
            _result.Append(Bencoder.ConvertByteString(key));
            _result.Append(Bencoder.ConvertInteger(value));
        }

        public void Add(string key, BencodedList value)
        {
            var str = value.Build();

            _result.Append(Bencoder.ConvertByteString(key));
            _result.Append(str);
        }

        public void Add(string key, BencodedDictionary value)
        {
            var str = value.Build();

            _result.Append(Bencoder.ConvertByteString(key));
            _result.Append(str);
        }

        public string Build()
        {
            _result.Append("e");

            return _result.ToString();
        }
    }
}
