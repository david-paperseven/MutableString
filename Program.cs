using System;

namespace MutableString
{
    class Program
    {
        static void Main(string[] args)
        {
            MutableString mutableString1 = new MutableString("ABCDEF");
            MutableString mutableString2 = new MutableString("AAAAAA");
            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            mutableString2.SetSubString(3, "DEF");
            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            mutableString2.SetSubString(0, "ABC");
            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            mutableString2.SetString("ABC");
            Console.WriteLine($"mutableString == {mutableString2} length is {mutableString2.Length}");
            MutableString mutableString3 = new MutableString(32);
            mutableString3.SetString("12345678");
            Console.WriteLine($"mutableString == {mutableString3} length is {mutableString3.Length} capacity is {mutableString3.Capacity}");
            mutableString3.SetSubString(8, "12345678");
            Console.WriteLine($"mutableString == {mutableString3} length is {mutableString3.Length} capacity is {mutableString3.Capacity}");
        }
    }
}