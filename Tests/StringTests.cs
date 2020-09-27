using System;
using System.Diagnostics;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;
using Performance;

namespace Tests
{
    public class Tests
    {

        [Test]
        public void FormatTest1()
        {
            MutableString mutableString = new MutableString(128);
            short[] sValues = {Int16.MinValue, -27, 0, 1042, Int16.MaxValue};
            Console.WriteLine("{0,10}  {1,10}", "Decimal", "Hex");
            foreach (short v in sValues)
            {
                mutableString.Format("{0,10:G}: {0,10:X}", v);
                string systemString = String.Format("{0,10:G}: {0,10:X}", v);
                Console.WriteLine($"{mutableString}=={systemString}");
                Assert.That(mutableString == systemString);
            }
        }
    }
}