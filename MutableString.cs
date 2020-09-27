using System;

namespace Performance
{
    public struct MutableString
    {
        public MutableString(string s)
        {
            _string = s;
            _capacity = _string.Length;
            _lock = new object();
        }

        public MutableString(int capacity)
        {
            _string = new string('\0', capacity);
            _capacity = _string.Length;
            _lock = new object();
        }

        private readonly object _lock;

        public string String => _string;

        // the underlying string object
        private string _string { get; }

        public int Capacity => _capacity;
        private int _capacity;
        public int Length => _string.Length;

        // implicitly cast from the system string type
        public static implicit operator MutableString(string s)
        {
            return new MutableString(s);
        }

        // implicitly cast to the system string type
        public static implicit operator string(MutableString s)
        {
            return s.ToString();
        }


        // SetSubString overwrites a part of the character buffer with a new sequence
        // of characters. If optionally updates the length to truncate the string 
        // at the end of the new sequence
        public void SetSubString(int destPos, string src, bool updateLength = false)
        {
            if (src.Length > Capacity - destPos) throw new ArgumentOutOfRangeException();

            unsafe
            {
                fixed (char* pDest = _string)
                fixed (char* pSrc = src)
                {
                    // the chars are 2 bytes wide
                    Buffer.MemoryCopy(pSrc, &pDest[destPos], Capacity * 2, src.Length * 2);
                }
            }

            if (updateLength)
                SetLength(destPos + src.Length);
        }

        // SetString overwrites the character buffer with new characters
        // and set the buffer length which has the effect of replacing 
        // the existing string with the new one
        public void SetString(string src)
        {
            if (src.Length > Length)
                throw new ArgumentOutOfRangeException();
            SetLength(src.Length);
            SetSubString(0, src);
        }

        // Set the string using the contents of a StackBuffer
        public void SetStackBuffer(int destPos, StackBuffer buffer)
        {
            var newLength = destPos + buffer.Count;
            if (destPos + buffer.Count > Capacity)
                throw new ArgumentOutOfRangeException();
            SetLength(newLength);
            for (var i = destPos; i < newLength; i++)
                this[i] = buffer[i];
        }

        // Sets the length of the character buffer
        // in the underlying native object
        private void SetLength(int newLength)
        {
            if (newLength > _capacity) throw new ArgumentOutOfRangeException();

            unsafe
            {
                // acquire a mutual exclusion lock
                lock (_lock)
                {
                    /*
                      https://github.com/dotnet/runtime/blob/master/src/coreclr/src/vm/object.h
                      StringLength is stored immediately before the char buffer
                      
                      quoted from object.h
                      
                    *   StringObject
                    *
                    *   Special String implementation for performance.
                    *
                    *   m_StringLength - Length of string in number of WCHARs
                    *   m_FirstChar    - The string buffer
                    *
                       class StringObject : public Object
                       {
                           DWORD   m_StringLength;
                           WCHAR   m_FirstChar;
                           ....
                       }
                    */

                    fixed (char* charBuffer = _string)
                    {
                        var pLength = (int*) charBuffer;
                        pLength -= 1;
                        *pLength = newLength;
                    }
                } // release the lock
            }
        }


        public override string ToString()
        {
            return _string;
        }

        public bool Contains(string s)
        {
            return _string.Contains(s);
        }


        public static bool operator ==(MutableString a, MutableString b)
        {
            return EqualsHelper(a, b);
        }

        public static bool operator !=(MutableString a, MutableString b)
        {
            return !EqualsHelper(a, b);
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || this.GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                MutableString mutableString = (MutableString) obj;
                return EqualsHelper(this, mutableString);
            }
        }

        public override int GetHashCode()
        {
            return _string.GetHashCode();
        }

        // this helper function is taken from the C# reference source
        // https://referencesource.microsoft.com/#mscorlib/system/string.cs,11648d2d83718c5e,references
        private static bool EqualsHelper(MutableString strA, MutableString strB)
        {
            unsafe
            {
                var length = strA.Length;

                fixed (char* ap = strA.String)
                fixed (char* bp = strB.String)
                {
                    var a = ap;
                    var b = bp;

                    // unroll the loop
                    while (length >= 10)
                    {
                        if (*(int*) a != *(int*) b) return false;
                        if (*(int*) (a + 2) != *(int*) (b + 2)) return false;
                        if (*(int*) (a + 4) != *(int*) (b + 4)) return false;
                        if (*(int*) (a + 6) != *(int*) (b + 6)) return false;
                        if (*(int*) (a + 8) != *(int*) (b + 8)) return false;
                        a += 10;
                        b += 10;
                        length -= 10;
                    }

                    // This depends on the fact that the String objects are
                    // always zero terminated and that the terminating zero is not included
                    // in the length. For odd string sizes, the last compare will include
                    // the zero terminator.
                    while (length > 0)
                    {
                        if (*(int*) a != *(int*) b)
                            break;
                        a += 2;
                        b += 2;
                        length -= 2;
                    }

                    return length <= 0;
                }
            }
        }

        public char this[int index]
        {
            // use the getter from the underlying string object
            get => _string[index];

            set
            {
                if (index < 0 || index >= Capacity)
                    throw new IndexOutOfRangeException();
                // for the setter we need access to the native char buffer
                unsafe
                {
                    // acquire a mutual exclusion lock
                    lock (_lock)
                    {
                        // pin the string's char buffer so we can access it 
                        fixed (char* valueChars = _string)
                        {
                            valueChars[index] = value;
                        }
                    } // release the lock
                }
            }
        }
    }
}