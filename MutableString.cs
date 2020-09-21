using System;

namespace MutableString
{
    public struct MutableString
    {
        public MutableString(string s)
        {
            _string = s;
            _modified = false;
        }

        public string String => _string;

        // the underlying string object
        private string _string { get; set; }

        // we record whether the string has been mutated so as to know
        // whether we can use C#'s existing string equality function or
        // whether we need to check each character
        private bool _modified;


        public int Length => _string.Length;

        // implicitly cast to the system string type
        public static implicit operator MutableString(string s)
        {
            return new MutableString(s);
        }

        public void SetString(string src)
        {
            if (src.Length > Length)
                throw new IndexOutOfRangeException();
            _string.SetLength(src.Length);
            SetSubString(0, src);
        }

        public void SetSubString(int destPos, string src)
        {
            if (src.Length > Length - destPos)
            {
                throw new IndexOutOfRangeException();
            }

            unsafe
            {
                fixed (char* pDest = _string)
                fixed (char* pSrc = src)
                {
                    // the chars are 2 bytes wide
                    Buffer.MemoryCopy(pSrc, &pDest[destPos], _string.Length * 2, src.Length * 2);
                }
            }
        }

        public override string ToString()
        {
            return _string;
        }

        public static bool operator ==(MutableString a, MutableString b)
        {
            if (!(a._modified || b._modified))
                return string.Equals(a.String, b.String);
            return EqualsHelper(a, b);
        }

        public static bool operator !=(MutableString a, MutableString b)
        {
            if (!(a._modified || b._modified))
                return !string.Equals(a.String, b.String);
            return !EqualsHelper(a, b);
        }

        // this helper function is taken from the C# reference source
        // https://referencesource.microsoft.com/#mscorlib/system/string.cs,11648d2d83718c5e,references
        private static bool EqualsHelper(MutableString strA, MutableString strB)
        {
            unsafe
            {
                int length = strA.Length;

                fixed (char* ap = strA.String)
                fixed (char* bp = strB.String)
                {
                    char* a = ap;
                    char* b = bp;

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

                    return (length <= 0);
                }
            }
        }

        public char this[int index]
        {
            // use the getter from the underlying string object
            get => _string[index];
            set
            {
                if (index < 0 || index >= Length)
                    throw new IndexOutOfRangeException();
                unsafe
                {
                    // pin the string's char buffer so we can access it 
                    fixed (char* valueChars = _string)
                    {
                        valueChars[index] = value;
                    }
                }

                _modified = true;
            }
        }
    }
}