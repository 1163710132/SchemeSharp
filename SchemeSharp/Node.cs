using System.Collections.Generic;

namespace SchemeSharp
{
    public class Node<T>
    {
        public List<Node<T>> Children { get; }
        public bool IsLeaf => Children == null;
        public bool IsBranch => Children != null;
        public bool HasChildren => Children != null && Children.Count > 0;
        public T Value { get; }

        private Node(T value)
        {
            Value = value;
            Children = null;
        }

        private Node()
        {
            Value = default(T);
            Children = new List<Node<T>>();
        }

        public static Node<T> OfLeaf(T value)
        {
            return new Node<T>(value);
        }

        public static Node<T> OfBranch()
        {
            return new Node<T>();
        }
    }
}