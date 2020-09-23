using System;
using System.Globalization;

namespace MutableString
{
    public static class MutableStringExtensions
    {
        private const int MAX_CHARS = 255;

        private static readonly char[] asciiDigits = new char[]
            {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

        /// <summary>
        /// Format<>
        /// Used templated args instead of the usual array of params to avoid boxing
        ///  </summary>
        public static void Format<A>(this MutableString text, string format, A arg0)
            where A : IConvertible
        {
            FormatHelper(text, format, 1, arg0, 0, 0, 0, 0);
        }

        public static void Format<A, B>(this MutableString text, string format, A arg0, B arg1)
            where A : IConvertible
            where B : IConvertible
        {
            FormatHelper(text, format, 2, arg0, arg1, 0, 0, 0);
        }

        public static void Format<A, B, C>(this MutableString text, string format, A arg0, B arg1, C arg2)
            where A : IConvertible
            where B : IConvertible
            where C : IConvertible
        {
            FormatHelper(text, format, 3, arg0, arg1, arg2, 0, 0);
        }

        public static void Format<A, B, C, D>(this MutableString text, string format, A arg0, B arg1, C arg2, D arg3)
            where A : IConvertible
            where B : IConvertible
            where C : IConvertible
            where D : IConvertible
        {
            FormatHelper(text, format, 4, arg0, arg1, arg2, arg3, 0);
        }

        public static void Format<A, B, C, D, E>(this MutableString text, string format, A arg0, B arg1, C arg2, D arg3)
            where A : IConvertible
            where B : IConvertible
            where C : IConvertible
            where D : IConvertible
            where E : IConvertible
        {
            FormatHelper(text, format, 4, arg0, arg1, arg2, arg3, arg3);
        }

        private static void FormatError()
        {
            throw new FormatException("Format_InvalidString");
        }

        static void FormatHelper<A, B, C, D, E>(MutableString text, string format, int numArgs, A arg0, B arg1, C arg2, D arg3, E arg4)
            where A : IConvertible
            where B : IConvertible
            where C : IConvertible
            where D : IConvertible
            where E : IConvertible
        {
            IFormatProvider provider = CultureInfo.InvariantCulture;
            int pos = 0;
            int len = format.Length;
            char ch = '\x0';

            // Stackbuffer provide temporary buffer space allocated on the stack
            StackBuffer charBuffer = new StackBuffer(stackalloc char[MAX_CHARS]);
            StackBuffer tempBuffer = new StackBuffer(stackalloc char[MAX_CHARS]);
            StackBuffer fmt = new StackBuffer(stackalloc char[MAX_CHARS]);
            while (true)
            {
                int p = pos;
                int i = pos;
                while (pos < len)
                {
                    ch = format[pos];

                    pos++;
                    if (ch == '}')
                    {
                        if (pos < len && format[pos] == '}') // Treat as escape character for }}
                            pos++;
                        else
                            FormatError();
                    }

                    if (ch == '{')
                    {
                        if (pos < len && format[pos] == '{') // Treat as escape character for {{
                            pos++;
                        else
                        {
                            pos--;
                            break;
                        }
                    }

                    charBuffer.Append(ch);
                }

                if (pos == len)
                    break;
                pos++;
                if (pos == len || (ch = format[pos]) < '0' || ch > '9')
                    FormatError();
                int index = 0;
                do
                {
                    index = index * 10 + ch - '0';
                    pos++;
                    if (pos == len)
                        FormatError();
                    ch = format[pos];
                } while (ch >= '0' && ch <= '9' && index < 1000000);

                // index is argument number
                if (index >= numArgs)
                    throw new FormatException("Format_IndexOutOfRange");
                while (pos < len && (ch = format[pos]) == ' ')
                    pos++;
                bool leftJustify = false;
                int width = 0;
                if (ch == ',')
                {
                    pos++;
                    while (pos < len && format[pos] == ' ') pos++;

                    if (pos == len)
                        FormatError();
                    ch = format[pos];
                    if (ch == '-')
                    {
                        leftJustify = true;
                        pos++;
                        if (pos == len)
                            FormatError();
                        ch = format[pos];
                    }

                    if (ch < '0' || ch > '9')
                        FormatError();
                    do
                    {
                        width = width * 10 + ch - '0';
                        pos++;
                        if (pos == len)
                            FormatError();
                        ch = format[pos];
                    } while (ch >= '0' && ch <= '9' && width < 1000000);
                }

                while (pos < len && (ch = format[pos]) == ' ')
                    pos++;
                if (ch == ':')
                {
                    pos++;
                    p = pos;
                    i = pos;
                    fmt.Clear();
                    while (true)
                    {
                        if (pos == len)
                            FormatError();
                        ch = format[pos];
                        pos++;
                        if (ch == '{')
                        {
                            if (pos < len && format[pos] == '{') // Treat as escape character for {{
                                pos++;
                            else
                                FormatError();
                        }
                        else if (ch == '}')
                        {
                            if (pos < len && format[pos] == '}') // Treat as escape character for }}
                                pos++;
                            else
                            {
                                pos--;
                                break;
                            }
                        }

                        fmt.Append(ch); // number formatting places - not currently used 
                    }
                }

                if (ch != '}')
                    FormatError();
                pos++;

                tempBuffer.Clear();
                // switch based on the argument number in the brackets {}
                switch (index)
                {
                    case 0:
                    {
                        EvalArg(ref tempBuffer, fmt, arg0);
                        break;
                    }
                    case 1:
                    {
                        EvalArg(ref tempBuffer, fmt, arg1);
                        break;
                    }
                    case 2:
                    {
                        EvalArg(ref tempBuffer, fmt, arg2);
                        break;
                    }
                    case 3:
                    {
                        EvalArg(ref tempBuffer, fmt, arg3);
                        break;
                    }
                    case 4:
                    {
                        EvalArg(ref tempBuffer, fmt, arg4);
                        break;
                    }
                }

                // tempBuffer.Count is the length of the arg string
                int pad = width - tempBuffer.Count;
                if (!leftJustify && pad > 0)
                {
                    charBuffer.Append(' ', pad);
                }

                // append the arg string to the main character string
                charBuffer.Append(tempBuffer);

                if (leftJustify && pad > 0)
                {
                    charBuffer.Append(' ', pad);
                }
            }

            text.SetStackBuffer(0, charBuffer);
        }

        /// <summary>
        /// Integer
        /// Handles the 16,32,64bit signed and unsigned integers
        /// </summary>
        static void Integer(ref StackBuffer buffer, StackBuffer format, long value, bool signed)
        {
            bool negSign = signed && (value < 0);
            char fmt = ParseFormatSpecifier(format, out int digits);
            char fmtUpper = (char) (fmt & 0xFFDF); // ensure fmt is upper-cased for purposes of comparison
            if ((fmtUpper == 'G' && digits < 1) || fmtUpper == 'D')
            {
                EvalInt(ref buffer, (ulong) Math.Abs(value), digits, 10, negSign);
            }
            else if (fmtUpper == 'X')
            {
                EvalInt(ref buffer, (ulong) Math.Abs(value), digits, 16, negSign);
            }
        }

        /// <summary>
        /// EvalInt
        /// Converts integers into ascii
        /// </summary>
        private static void EvalInt(ref StackBuffer buffer, ulong intVal, int digits, ulong baseVal, bool negSign)
        {
            do
            {
                // Lookup from static char array, to cover hex values too
                buffer.Append(asciiDigits[intVal % baseVal]);
                intVal /= baseVal;
                digits--;
            } while (intVal != 0);

            while (digits-- > 0)
                buffer.Append('0');
            if (negSign)
                buffer.Append('-');
            buffer.Reverse();
        }

        /// <summary>
        /// FloatingPoint
        /// Handles single and double precision floating point values
        /// </summary>
        static void FloatingPoint(ref StackBuffer buffer, StackBuffer format, double value)
        {
            char fmt = ParseFormatSpecifier(format, out int digits);
            ftoa.Convert(ref buffer, value, digits);
        }

        /// <summary>
        /// EvalArg
        /// Entry point for evaluating the generic arguments
        /// </summary>
        static void EvalArg<T>(ref StackBuffer buffer, StackBuffer format, T arg)
            where T : IConvertible
        {
            switch (arg?.GetTypeCode())
            {
                case TypeCode.Int16:
                {
                    Integer(ref buffer, format, arg.ToInt16(NumberFormatInfo.CurrentInfo), true);
                    break;
                }
                case TypeCode.Int32:
                {
                    Integer(ref buffer, format, arg.ToUInt32(NumberFormatInfo.CurrentInfo), true);
                    break;
                }
                case TypeCode.Int64:
                {
                    Integer(ref buffer, format, arg.ToInt64(NumberFormatInfo.CurrentInfo), true);
                    ;
                    break;
                }
                case TypeCode.UInt16:
                {
                    Integer(ref buffer, format, arg.ToInt16(NumberFormatInfo.CurrentInfo), false);
                    break;
                }
                case TypeCode.UInt32:
                {
                    Integer(ref buffer, format, arg.ToInt32(NumberFormatInfo.CurrentInfo), false);
                    break;
                }
                case TypeCode.UInt64:
                {
                    Integer(ref buffer, format, arg.ToInt64(NumberFormatInfo.CurrentInfo), false);
                    break;
                }
                case TypeCode.Single:
                {
                    FloatingPoint(ref buffer, format, arg.ToSingle(NumberFormatInfo.CurrentInfo));
                    break;
                }
                case TypeCode.Double:
                {
                    FloatingPoint(ref buffer, format, arg.ToDouble(NumberFormatInfo.CurrentInfo));
                    break;
                }
                case TypeCode.String:
                {
                    buffer.Append(arg.ToString());
                    break;
                }
            }
        }

        /// <summary>
        /// ParseFormatSpecifier
        /// https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/Number.Formatting.cs
        /// </summary>
        /// <returns></returns>
        static char ParseFormatSpecifier(StackBuffer format, out int digits)
        {
            char c = default;
            if (format.Length > 0)
            {
                // If the format begins with a symbol, see if it's a standard format
                // with or without a specified number of digits.
                c = format[0];
                if ((uint) (c - 'A') <= 'Z' - 'A' ||
                    (uint) (c - 'a') <= 'z' - 'a')
                {
                    // Fast path for sole symbol, e.g. "D"
                    if (format.Length == 1)
                    {
                        digits = -1;
                        return c;
                    }

                    if (format.Length == 2)
                    {
                        // Fast path for symbol and single digit, e.g. "X4"
                        int d = format[1] - '0';
                        if ((uint) d < 10)
                        {
                            digits = d;
                            return c;
                        }
                    }
                    else if (format.Length == 3)
                    {
                        // Fast path for symbol and double digit, e.g. "F12"
                        int d1 = format[1] - '0', d2 = format[2] - '0';
                        if ((uint) d1 < 10 && (uint) d2 < 10)
                        {
                            digits = d1 * 10 + d2;
                            return c;
                        }
                    }

                    // Fallback for symbol and any length digits.  The digits value must be >= 0 && <= 99,
                    // but it can begin with any number of 0s, and thus we may need to check more than two
                    // digits.  Further, for compat, we need to stop when we hit a null char.
                    int n = 0;
                    int i = 1;
                    while (i < format.Length && (((uint) format[i] - '0') < 10) && n < 10)
                    {
                        n = (n * 10) + format[i++] - '0';
                    }

                    // If we're at the end of the digits rather than having stopped because we hit something
                    // other than a digit or overflowed, return the standard format info.
                    if (i == format.Length || format[i] == '\0')
                    {
                        digits = n;
                        return c;
                    }
                }
            }

            // Default empty format to be "G"; custom format is signified with '\0'.
            digits = -1;
            return format.Length == 0 || c == '\0'
                ? // For compat, treat '\0' as the end of the specifier, even if the specifier extends beyond it.
                'G'
                : '\0';
        }
    }
}