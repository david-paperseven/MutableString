namespace MutableString
{
    // Ported from 
    // https://github.com/antongus/stm32tpl/blob/master/ftoa.c
    public class ftoa
    {
        private const int MAX_PRECISION = 10;

        static double[] rounders =
        {
            0.5, // 0
            0.05, // 1
            0.005, // 2
            0.0005, // 3
            0.00005, // 4
            0.000005, // 5
            0.0000005, // 6
            0.00000005, // 7
            0.000000005, // 8
            0.0000000005, // 9
            0.00000000005 // 10
        };

        public static void Convert(ref StackBuffer buffer, double f, int precision)
        {
            long intPart;

            // check precision bounds
            if (precision > MAX_PRECISION)
                precision = MAX_PRECISION;

            // sign stuff
            if (f < 0)
            {
                f = -f;
                buffer.Append('-');
            }

            if (precision < 0) // negative precision == automatic precision guess
            {
                if (f < 1.0) precision = 6;
                else if (f < 10.0) precision = 5;
                else if (f < 100.0) precision = 4;
                else if (f < 1000.0) precision = 3;
                else if (f < 10000.0) precision = 2;
                else if (f < 100000.0) precision = 1;
                else precision = 0;
            }

            // round value according the precision
            if (precision > 0)
                f += rounders[precision];

            // integer part...
            intPart = (long) f;
            f -= intPart;

            if (intPart == 0)
                buffer.Append('0');
            else
            {
                StackBuffer tempBuffer = new StackBuffer(stackalloc char[MAX_PRECISION]);
                // convert (reverse order)
                while (intPart != 0)
                {
                    tempBuffer.Append((char) ('0' + intPart % 10));
                    intPart /= 10;
                }

                tempBuffer.Reverse();
                for (int i = 0; i < tempBuffer.Count; i++)
                {
                    buffer.Append(tempBuffer[i]);
                }
            }

            // decimal part
            if (precision != 0)
            {
                // place decimal point
                buffer.Append('.');

                // convert
                while (precision-- != 0)
                {
                    f *= 10.0;
                    char c = (char) f;
                    buffer.Append((char) ('0' + c));
                    f -= c;
                }
            }
        }
    }
}