using System;

namespace MutableString
{
    class Program
    {
        static void Main(string[] args)
        {
            Usage();
            Limitations();
            Formatting();
        }

        static void Usage()
        {
            Console.WriteLine("Usage Examples");
            MutableString mutableString1 = new MutableString("ABCDEF");
            MutableString mutableString2 = new MutableString("AAAAAA");

            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            // Displays ABCDEF==AAAAAA is False

            mutableString2.SetSubString(3, "DEF");
            // Displays ABCDEF==AAADEF is False

            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            mutableString2.SetSubString(0, "ABC", false);
            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            // Displays ABCDEF==ABCDEF is True

            Console.WriteLine($"{mutableString2} length is {mutableString2.Length} capacity is {mutableString2.Capacity}");
            // Displays ABCDEF length is 6 capacity is 6

            mutableString2.SetString("ABC");
            Console.WriteLine($"{mutableString2} length is {mutableString2.Length} capacity is {mutableString2.Capacity}");
            // Displays ABC length is 3

            MutableString mutableString3 = new MutableString(32);
            mutableString3.SetString("12345678");
            Console.WriteLine($"{mutableString3} length is {mutableString3.Length} capacity is {mutableString3.Capacity}");
            // Displays 12345678 length is 8 capacity is 32

            mutableString3.SetSubString(8, "12345678", true);
            Console.WriteLine($"{mutableString3} length is {mutableString3.Length} capacity is {mutableString3.Capacity}");
            // Displays 1234567812345678 length is 16 capacity is 32
        }

        static void Limitations()
        {
            Console.WriteLine("\nLimitation Example");
            MutableString mutableString1 = new MutableString("AAAAAA");
            MutableString mutableString2 = new MutableString("AAAAAA");
            mutableString2.SetSubString(3, "DEF");
            Console.WriteLine(mutableString1);
            Console.WriteLine(mutableString2);
            // Displays    AAADEF
            //             AAADEF
        }

        static void Formatting()
        {
            Console.WriteLine("\nFormatting Examples");
            MutableString mutableString = new MutableString(128);
            short[] sValues = {Int16.MinValue, -27, 0, 1042, Int16.MaxValue};
            Console.WriteLine("{0,10}  {1,10}", "Decimal", "Hex");
            foreach (short v in sValues)
            {
                mutableString.Format("{0,10:G}: {0,10:X}", v);
                Console.WriteLine(mutableString);
            }

            float[] fValues = {1603, 1794.68235f, 15436.14f};
            foreach (var value in fValues)
            {
                mutableString.Format("{0,12:C2}   {0,12:E3}   {0,12:F4}   {0,12:N3}  {1,12:P2}", Convert.ToDouble(value), Convert.ToDouble(value) / 10000);
                Console.WriteLine(mutableString);
            }
        }
    }
}