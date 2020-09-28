using System;
using System.Collections.Generic;

namespace Performance
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TestIntegerTypes();
            TestFloats();
            TestDoubles();
            //Usage();
            //Limitations();
        }

        private static void TestFloats()
        {
            var    mutableString = new MutableString(128);
            string numberFormat  = "{0,20:F3}:{0,20:F6}";
            TestFloatingPointTypes(mutableString, new List<float> {0.0001f, 0.25f, 1000.40002f, 1.0f / 10.0f, (float) Math.PI, float.Epsilon}, numberFormat);
            TestFloatingPointTypes(mutableString, new List<float> {(float) Math.PI, float.Epsilon}, numberFormat);
        }

        private static void TestDoubles()
        {
            var    mutableString = new MutableString(128);
            string numberFormat  = "{0,20:F10}:{0,20:F10}";
            TestFloatingPointTypes(mutableString, new List<double> {0.0001, 0.25, 1000.40002, 1.0 / 10.0, Math.PI, float.Epsilon}, numberFormat);
            TestFloatingPointTypes(mutableString, new List<double> {Math.PI, float.Epsilon}, numberFormat);
        }

        private static void TestFloatingPointTypes<T>(MutableString mutableString, IList<T> values, string numberFormat) where T : IConvertible
        {
            Console.WriteLine("\n----------------------------------------" + typeof(T) + "----------------------------------------");
            foreach (var v in values)
            {
                var systemString = string.Format(numberFormat, v);
                mutableString.Format(numberFormat, v);
                Console.WriteLine($"'{mutableString}' == '{systemString}'  is {mutableString == systemString}");
            }
        }

        private static void TestIntegerTypes()
        {
            var    mutableString = new MutableString(128);
            string numberFormat  = "{0,20:G}: {0,20:X}";

            TestIntegers(mutableString, new List<short> {short.MinValue, -27, 0, 1042, short.MaxValue}, numberFormat);
            TestIntegers(mutableString, new List<int> {int.MinValue, -27, 0, 1042, int.MaxValue}, numberFormat);
            TestIntegers(mutableString, new List<long> {long.MinValue, -27, 0, 1042, long.MaxValue}, numberFormat);
            TestIntegers(mutableString, new List<ushort> {ushort.MinValue, 27, 0, 1042, ushort.MaxValue}, numberFormat);
            TestIntegers(mutableString, new List<uint> {ushort.MinValue, 27, 0, 1042, uint.MaxValue}, numberFormat);
            TestIntegers(mutableString, new List<ulong> {ulong.MinValue, 27, 0, 1042, ulong.MaxValue}, numberFormat);
        }

        static void TestIntegers<T>(MutableString mutableString, IList<T> values, string numberFormat) where T : IConvertible
        {
            Console.WriteLine("\n----------------------------------------" + typeof(T) + "----------------------------------------");
            foreach (var v in values)
            {
                var systemString = string.Format(numberFormat, v);
                mutableString.Format(numberFormat, v);
                Console.WriteLine($"'{mutableString}' == '{systemString}'  is {mutableString == systemString}");
            }
        }

        private static void Usage()
        {
            Console.WriteLine("Usage Examples");
            var mutableString1 = new MutableString("ABCDEF");
            var mutableString2 = new MutableString("AAAAAA");

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

            var mutableString3 = new MutableString(32);
            mutableString3.SetString("12345678");
            Console.WriteLine($"{mutableString3} length is {mutableString3.Length} capacity is {mutableString3.Capacity}");
            // Displays 12345678 length is 8 capacity is 32

            mutableString3.SetSubString(8, "12345678", true);
            Console.WriteLine($"{mutableString3} length is {mutableString3.Length} capacity is {mutableString3.Capacity}");
            // Displays 1234567812345678 length is 16 capacity is 32
        }

        private static void Limitations()
        {
            Console.WriteLine("\nLimitation Example");
            var mutableString1 = new MutableString("AAAAAA");
            var mutableString2 = new MutableString("AAAAAA");
            mutableString2.SetSubString(3, "DEF");
            Console.WriteLine(mutableString1);
            Console.WriteLine(mutableString2);
            // Displays    AAADEF
            //             AAADEF
        }

    }
}