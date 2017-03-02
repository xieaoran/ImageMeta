using System;
using System.Collections.Generic;

namespace ImageMeta.Huffman
{
    public class CharCode
    {
        private byte _char;
        public List<bool> Code;
        public int Count;

        public CharCode(byte charByte)
        {
            _char = charByte;
            Code = new List<bool>();
            Count = 1;
        }
        public void AddBit(bool bit)
        {
            Code.Insert(0, bit);
        }
        public void WriteCode(BitWriter writer)
        {
            foreach (var bit in Code)
            {
                writer.Write(bit);
            }
        }
    }
}
