using System.Collections.Generic;
using System.Linq;

namespace ImageMeta.Huffman
{
    public class SingleChar
    {
        public int Count;
        public List<byte> Chars;
        public SingleChar(byte startChar)
        {
            Chars = new List<byte>() { startChar };
            Count = 1;
        }

        public void CombineChar(SingleChar targetChar, 
            ref Dictionary<byte, SingleChar> charsDict, 
            ref Dictionary<byte, CharCode> charCodes)
        {
            foreach(var char1 in Chars)
            {
                charCodes[char1].AddBit(true);
            }
            foreach(var char2 in targetChar.Chars)
            {
                charCodes[char2].AddBit(false);
            }
            Chars = Chars.Union(targetChar.Chars).ToList();
            Count += targetChar.Count;
            charsDict.Remove(charsDict.First(c => c.Value == targetChar).Key);
        }
    }
}
