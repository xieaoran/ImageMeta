using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ImageMeta.Huffman
{
    public class HuffmanFile
    {
        private Dictionary<byte, SingleChar> _charsDict;
        private Dictionary<byte, CharCode> _charCodes;
        private Node _rootNode;
        private byte[] _buffer;
        private List<byte> _decodedBytes;

        public HuffmanFile(string keyPath)
        {
            using (var keyReader = new StreamReader(keyPath))
            {
                var keyJson = keyReader.ReadToEnd();
                _charCodes = JsonConvert.DeserializeObject<Dictionary<byte, CharCode>>(keyJson);
                ReadCodeToTree();
            }
        }
        public HuffmanFile(byte[] buffer)
        {
            ReadBuffer(buffer);
            GenerateCode();
        }

        private void ReadBuffer(byte[] buffer)
        {
            _charsDict = new Dictionary<byte, SingleChar>();
            _charCodes = new Dictionary<byte, CharCode>();
            _buffer = buffer;
            foreach (var currentChar in _buffer)
            {
                if (_charsDict.ContainsKey(currentChar))
                {
                    _charsDict[currentChar].Count++;
                    _charCodes[currentChar].Count++;
                }
                else
                {
                    _charsDict[currentChar] = new SingleChar(currentChar);
                    _charCodes[currentChar] = new CharCode(currentChar);
                }
            }
        }

        public void GenerateCode()
        {
            while (_charsDict.Count > 1)
            {
                _charsDict = _charsDict.OrderByDescending(c => c.Value.Count).ToDictionary(c => c.Key, c => c.Value);
                var trueChar = _charsDict.ElementAt(_charsDict.Count - 2).Value;
                var falseChar = _charsDict.ElementAt(_charsDict.Count - 1).Value;
                trueChar.CombineChar(falseChar, ref _charsDict, ref _charCodes);
            }
        }

        public void EncodeFile(string filePath, string keyPath)
        {
            var writer = new BitWriter(filePath);
            foreach (var currentChar in _buffer)
            {
                _charCodes[currentChar].WriteCode(writer);
            }
            writer.WriteBitToFile();
            var keyJson = JsonConvert.SerializeObject(_charCodes);
            using (var keyWriter = new StreamWriter(keyPath))
            {
                keyWriter.Write(keyJson);
            }
        }

        public void ReadCodeToTree()
        {
            _rootNode = new Node(NodeType.Root, null);
            foreach (var charCode in _charCodes)
            {
                var currentNode = _rootNode;
                foreach (var currentCode in charCode.Value.Code)
                {
                    if (currentCode == false)
                    {
                        if (currentNode.LeftNode == null)
                            currentNode.LeftNode = new Node(NodeType.Middle, false);
                        currentNode = currentNode.LeftNode;
                    }
                    else
                    {
                        if (currentNode.RightNode == null)
                            currentNode.RightNode = new Node(NodeType.Middle, true);
                        currentNode = currentNode.RightNode;
                    }
                }
                var lastNode = new Node(NodeType.Last, charCode.Key);
                currentNode.LeftNode = lastNode;
                currentNode.RightNode = null;
            }
        }

        public byte[] ReadEncodedFile(string filePath)
        {
            _decodedBytes = new List<byte>();
            var currentNode = _rootNode;
            var reader = new BitReader(filePath);
            var fileLength = reader.ReadBitFromFile();
            for (var index = 0; index < fileLength * 8; index++)
            {
                var bit = reader.Read();
                currentNode = (!bit) ? currentNode.LeftNode : currentNode.RightNode;
                if (currentNode.LeftNode.Type != NodeType.Last) continue;
                var charByte = (byte) currentNode.LeftNode.Data;
                _decodedBytes.Add(charByte);
                currentNode = _rootNode;
            }
            return _decodedBytes.ToArray();
        }
    }
}
