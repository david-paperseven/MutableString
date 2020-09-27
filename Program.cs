using System;

namespace Performance
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TestAllTypes();
            //
            // Usage();
            // Limitations();
            // Formatting();
        }

        private static void TestAllTypes()
        {
            var mutableString = new MutableString(128);

            short[] sValues = {short.MinValue, -27, 0, 1042, short.MaxValue};
            Console.WriteLine("{0,10}  {1,10}", "Decimal", "Hex");
            foreach (var v in sValues)
            {
                var systemString = string.Format("{0,20:G}: {0,20:X}", v);
                mutableString.Format("{0,20:G}: {0,20:X}", v);
                Console.WriteLine($"'{mutableString}' == '{systemString}'  is {mutableString == systemString}");
            }

            int[] iValues = {int.MinValue, -27, 0, 1042, int.MaxValue};
            Console.WriteLine("{0,10}  {1,10}", "Decimal", "Hex");
            foreach (var v in iValues)
            {
                mutableString.Format("{0,20:G}: {0,20:X}", v);
                var systemString = string.Format("{0,20:G}: {0,20:X}", v);
                Console.WriteLine($"'{mutableString}' == '{systemString}'  is {mutableString == systemString}");
            }

            long[] lValues = {long.MinValue, -27, 0, 1042, long.MaxValue};
            Console.WriteLine("{0,10}  {1,10}", "Decimal", "Hex");
            foreach (var v in lValues)
            {
                var systemString = string.Format("{0,20:G}: {0,20:X}", v);
                mutableString.Format("{0,20:G}: {0,20:X}", v);
                Console.WriteLine($"'{mutableString}' == '{systemString}'  is {mutableString == systemString}");
            }

            ushort[] uValues = {ushort.MinValue, 27, 0, 1042, ushort.MaxValue};
            Console.WriteLine("{0,10}  {1,10}", "Decimal", "Hex");
            foreach (var v in uValues)
            {
                var systemString = string.Format("{0,20:G}: {0,20:X}", v);
                mutableString.Format("{0,20:G}: {0,20:X}", v);
                Console.WriteLine($"'{mutableString}' == '{systemString}'  is {mutableString == systemString}");
            }

            uint[] uiValues = {uint.MinValue, 27, 0, 1042, uint.MaxValue};
            Console.WriteLine("{0,10}  {1,10}", "Decimal", "Hex");
            foreach (var v in uiValues)
            {
                mutableString.Format("{0,20:G}: {0,20:X}", v);
                var systemString = string.Format("{0,20:G}: {0,20:X}", v);
                Console.WriteLine($"'{mutableString}' == '{systemString}'  is {mutableString == systemString}");
            }

            ulong[] ulValues = {ulong.MinValue, 27, 0, 1042, ulong.MaxValue};
            Console.WriteLine("{0,10}  {1,10}", "Decimal", "Hex");
            foreach (var v in ulValues)
            {
                mutableString.Format("{0,20:G}: {0,20:X}", v);
                var systemString = string.Format("{0,20:G}: {0,20:X}", v);
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

        private static void Formatting()
        {
            Console.WriteLine("\nFormatting Examples");
            var mutableString = new MutableString(128);
            short[] sValues = {short.MinValue, -27, 0, 1042, short.MaxValue};
            Console.WriteLine("{0,10}  {1,10}", "Decimal", "Hex");
            foreach (var v in sValues)
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