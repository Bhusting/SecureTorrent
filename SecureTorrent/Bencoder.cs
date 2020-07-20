using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SecureBencoder
{
    public static class Bencoder
    {
        /// <summary>
        /// Encodes a String.
        /// </summary>
        /// <param name="input">String to Encode.</param>
        /// <returns>Encoded String.</returns>
        public static string ConvertByteString(string input)
        {
            return $"{input.Length}:{input}";
        }

        public static string ConvertStream(Stream stream)
        {
            return $"{stream.Length}:{stream}";
        }

        /// <summary>
        /// Encodes an Integer.
        /// </summary>
        /// <param name="input">Integer to Encode.</param>
        /// <returns>Encoded String.</returns>
        public static string ConvertInteger(int input)
        {
            return $"i{input}e";
        }
    }
}
