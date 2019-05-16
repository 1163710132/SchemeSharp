using System;
using System.Collections.Generic;

namespace SchemeSharp
{
    public delegate Token? Matcher(string input, int begin);

    public class Lexer
    {
        public List<(string, Matcher)> Matchers;
        public HashSet<string> Skips;

        public IEnumerator<Token> DoLex(string input)
        {
            int index = 0;
            while (true)
            {
                var isMatched = false;
                foreach (var (type, matcher) in Matchers)
                {
                    var matched = matcher(input, index);
                    if (matched.HasValue)
                    {
                        isMatched = true;
                        if (!Skips.Contains(matched.Value.Type))
                        {
                            yield return matched.Value;
                        }
                        index += matched.Value.Raw.Length;
                        break;
                    }
                }

                if (!isMatched)
                {
                    throw new Exception("No Matching Pattern");
                }
            }
        }
    }
}