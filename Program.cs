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
        }
    }
}