using System.Collections.Generic;

namespace SchemeSharp
{
    public enum LiteralType
    {
        Boolean, Number, Character, String, Identifier
    }
    
    public struct Literal
    {
        public readonly string Raw;
        public readonly string Type;

        public Literal(string raw, string type)
        {
            Raw = raw;
            Type = type;
        }
    }
}