using System;

namespace SchemeSharp
{
    public struct Token
    {
        public bool Equals(Token other)
        {
            return string.Equals(Raw, other.Raw) && string.Equals(Type, other.Type);
        }

        public override bool Equals(object obj)
        {
            return obj is Token other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Raw, Type);
        }

        public readonly string Raw;
        public readonly string Type;

        public Token(string raw, string type)
        {
            Raw = raw;
            Type = type;
        }

        public static bool operator==(Token token1, Token token2)
        {
            return token1.Raw.Equals(token2.Raw) && token1.Type.Equals(token2.Type);
        } 
        public static bool operator!=(Token token1, Token token2)
        {
            return !(token1 == token2);
        }
    }

    public enum TokenType
    {
        Identifier, Boolean, Number, Character, String, LeftPar, RightPar
    }
}