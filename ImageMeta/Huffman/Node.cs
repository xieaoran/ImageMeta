namespace ImageMeta.Huffman
{
    public enum NodeType
    {
        Root,
        Middle,
        Last
    }
    public class Node
    {
        public NodeType Type;
        public object Data;
        public Node LeftNode;
        public Node RightNode;
        public Node(NodeType type, object data)
        {
            Type = type;
            Data = data;
            LeftNode = null;
            RightNode = null;
        }
    }
}
