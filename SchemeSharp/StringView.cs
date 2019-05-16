using System.Collections;
using System.Collections.Generic;

namespace SchemeSharp
{
    public class StringView: IEnumerable<char>
    {
        private string _source;
        private int _begin;
        public int Length { get; }

        public StringView(string source)
        {
            _source = source;
        }

        public StringView(StringView source, int begin, int length)
        {
            _source = source._source;
            _begin = begin + source._begin;
            Length = length;
        }

        public IEnumerator<char> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return _source[_begin + i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}