using System.Collections;
using System.IO;
using System.Text;

namespace ImageMeta.Huffman
{
    public class BitReader
    {
        private byte[] _buffer;
        private BitArray _singleByte;
        private int _currentSize;
        private int _currentPos;
        private string _filePath;

        public BitReader(string filePath)
        {
            _currentSize = 0;
            _currentPos = 0;
            _filePath = filePath;
        }

        public int ReadBitFromFile()
        {
            var encodedFile = new BinaryReader(
                new FileStream(_filePath, FileMode.Open), Encoding.ASCII);
            var fileLength = (int) encodedFile.BaseStream.Length;
            _buffer = new byte[fileLength];
            encodedFile.Read(_buffer, 0, fileLength);
            encodedFile.Close();
            _singleByte = new BitArray(new[] { _buffer[_currentPos] });
            _currentSize = 7;
            return fileLength;
        }

        public bool Read()
        {
            if (_currentSize == -1)
            {
                _currentPos++;
                _singleByte = new BitArray(new [] { _buffer[_currentPos]});
                _currentSize = 7;
            }
            var bit = _currentSize < _singleByte.Length && _singleByte[_currentSize];
            _currentSize--;
            return bit;
        }
    }
}
