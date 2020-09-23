/***
 * StackBuffer
 *
 * A character buffer allocated on the stack
 *
 */


using System;

namespace MutableString
{
    public ref struct StackBuffer
    {
        private Span<char> _buffer;
        private int _index;

        public StackBuffer(Span<char> charBuffer)
        {
            _buffer = charBuffer;
            _index = 0;
        }

        public void Clear()
        {
            _index = 0;
        }

        public int Count => _index;
        public int Length => _index;

        public char this[int index]
        {
            get => _buffer[index];
            set => _buffer[index] = value;
        }

        public void Append(char c)
        {
            _buffer[_index++] = c;
        }

        public void Append(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                _buffer[_index++] = s[i];
            }
        }

        public void Append(char c, int count)
        {
            while (count-- > 0)
                _buffer[_index++] = c;
        }

        public void Append(StackBuffer sourceBuffer)
        {
            for (int i = 0; i < sourceBuffer.Count; i++)
            {
                _buffer[_index++] = sourceBuffer[i];
            }
        }


        /// <summary>
        /// Reverses the characters inplace
        /// </summary>
        public void Reverse()
        {
            int start = 0, end = Count - 1;
            while (start < end)
            {
                char temp = _buffer[start];
                _buffer[start] = _buffer[end];
                _buffer[end] = temp;
                start++;
                end--;
            }
        }
    }
}