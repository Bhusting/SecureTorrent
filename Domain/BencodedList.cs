using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class BencodedList
    {
        private StringBuilder _result = new StringBuilder("l");

        public void AddString(string input)
        {

            var str = Bencoder.ConvertByteString(input);

            _result.Append(str);

        }

        public void AddInteger(int input)
        {

            var str = Bencoder.ConvertInteger(input);

            _result.Append(str);


        }

        public void AddList(BencodedList list)
        {
            var str = list.Build();

            _result.Append(str);

        }

        public void AddDictionary(BencodedDictionary dictionary)
        {
            var str = dictionary.Build();

            _result.Append(str);

        }

        public string Build()
        {
            _result.Append("e");

            return _result.ToString();
        }
    }
}
