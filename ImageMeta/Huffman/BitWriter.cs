using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageMeta.Huffman
{
    public class BitWriter
    {
        private List<byte> _buffer;
        private byte _singleByte;
        private byte _currentSize;
        private string _filePath;

        public BitWriter(string filePath)
        {
            _buffer = new List<byte>();
            _singleByte = 0;
            _currentSize = 0;
            _filePath = filePath;
        }

        public void Write(bool bit)
        {
            var bitInt = bit ? 1 : 0;
            if (_currentSize == 8)
            {
                _buffer.Add(_singleByte);
                _singleByte = 0;
                _currentSize = 0;
            }
            _singleByte = (byte)(_singleByte << 1 | bitInt);
            _currentSize++;
        }

        public void WriteBitToFile()
        {
            _singleByte = (byte)(_singleByte << (8 - _currentSize));
            _buffer.Add(_singleByte);
            var encodedFile = new BinaryWriter(
                new FileStream(_filePath, FileMode.Create), Encoding.ASCII);
            encodedFile.Write(_buffer.ToArray());
            encodedFile.Close();
        }
    }
}
