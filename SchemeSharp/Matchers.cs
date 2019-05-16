using System.Collections.Generic;

namespace SchemeSharp
{
    public static class Matchers
    {
        public static readonly HashSet<char> SpecialInitials =
            new HashSet<char>
            {
                '!', '$', '%', '&', '*', '/', ':', '<', '=', '>', '?', '^', '_', '~'
            };
        
//        public static Token? MatchIdentifier(string input, int index)
//        {
//            while (index < input.Length)
//            {
//                
//            }
//        }
    }
}