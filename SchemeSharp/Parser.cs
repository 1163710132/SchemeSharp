using System.Collections.Generic;

namespace SchemeSharp
{
    public class Parser
    {
        public IEnumerator<Node<Token>> Parse(IEnumerator<Token> input)
        {
            var stack = new Stack<Node<Token>>();
            stack.Push(Node<Token>.OfBranch());
            while (input.MoveNext())
            {
                Token token = input.Current;
                switch (token.Type)
                {
                    case nameof(TokenType.LeftPar):
                    {
                        stack.Push(Node<Token>.OfBranch());
                        break;
                    }

                    case nameof(TokenType.RightPar):
                    {
                        var child = stack.Pop();
                        stack.Peek().Children.Add(child);
                        break;
                    }

                    default:
                    {
                        stack.Peek().Children.Add(Node<Token>.OfLeaf(token));
                        break;
                    }
                }
            }

            foreach (var node in stack.Peek().Children)
            {
                yield return node;
            }
        }
    }
}