# MutableString

MutableString is a C# struct that wraps the String class and provides functions for changing the string contents without creating a new string.

## Why Would I Want To Do That?

In C# strings are immutable so its not possible to change a string without creating a new string and that allocates memory which will eventually be garbage collected.

In performance constrained environments such as video games, garbage collection is expensive so it can be desirable to modify a string rather than creating a new one.

## How Does It Work?

The .NET framework requires the memory layout for string objects to be consistent so that it can provide optimised code paths for manipulating strings. 
MutableStrings uses this layout to make changes directly to the character buffer of a string.

MutableStrings has an implementation of the String.Format() which uses the stack to allocate temporary scratch space and then writes the result to the native character buffer of the input string.

The Format() function supports ``int16, int32, int64, uint16, uint32, uint64, float, double and string`` types.

It uses the familiar syntax of ```Format("{0} {1} {2}",arg1,arg2,arg3)``` with rounding, padding and precision parameters.

## Usage

```csharp
            MutableString mutableString1 = new MutableString("ABCDEF");
            MutableString mutableString2 = new MutableString("AAAAAA");

            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            // Displays ABCDEF==AAAAAA is False

            mutableString2.SetSubString(3, "DEF");
            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            // Displays ABCDEF==AAADEF is False
          
            mutableString2.SetSubString(0, "ABC", false);
            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            // Displays ABCDEF==ABCDEF is True

            Console.WriteLine($"{mutableString2} length is {mutableString2.Length} capacity is {mutableString2.Capacity}");
            // Displays ABCDEF length is 6 capacity is 6

            mutableString2.SetString("ABC");
            Console.WriteLine($"{mutableString2} length is {mutableString2.Length} capacity is {mutableString2.Capacity}");
            // Displays ABC length is 3 capacity is 6

            MutableString mutableString3 = new MutableString(32);
            mutableString3.SetString("12345678");
            Console.WriteLine($"{mutableString3} length is {mutableString3.Length} capacity is {mutableString3.Capacity}");
            // Displays 12345678 length is 8 capacity is 32

            mutableString3.SetSubString(8, "12345678", true);
            Console.WriteLine($"{mutableString3} length is {mutableString3.Length} capacity is {mutableString3.Capacity}");
            // Displays 1234567812345678 length is 16 capacity is 32
```

## Formatting

```csharp 
            MutableString mutableString = new MutableString(128);
            short[] sValues = {Int16.MinValue, -27, 0, 1042, Int16.MaxValue};
            Console.WriteLine("{0,10}  {1,10}", "Decimal", "Hex");
            foreach (short v in sValues)
            {
                mutableString.Format("{0,10:G}: {0,10:X}", v);
                Console.WriteLine(mutableString);
            }
            //   Decimal         Hex
            //    -32768:      -8000
            //       -27:        -1B
            //         0:          0
            //      1042:        412
            //     32767:       7FFF

            float[] fValues = {1603, 1794.68235f, 15436.14f};
            foreach (var value in fValues)
            {
                mutableString.Format("{0,12:C2}   {0,12:E3}   {0,12:F4}   {0,12:N3}  {1,12:P2}", Convert.ToDouble(value), Convert.ToDouble(value) / 10000);
                Console.WriteLine(mutableString);
            }
            //   1603.00       1603.000      1603.0000       1603.000          0.16
            //   1794.68       1794.682      1794.6824       1794.682          0.18
            //  15436.14      15436.140     15436.1396      15436.140          1.54
```

## Limitations

C# stores strings in an internal hash map called the intern pool in order to conserve memory. Consequently, an instance of a literal string with a particular value only exists once in the system.

This means if you instantiate two MutableStrings with the same intial string they will both point to the same string buffer and modifying one will modify the other.

```csharp
            MutableString mutableString1 = new MutableString("AAAAAA");
            MutableString mutableString2 = new MutableString("AAAAAA");
            mutableString2.SetSubString(3, "DEF");
            Console.WriteLine(mutableString1);
            Console.WriteLine(mutableString2);
            // Displays    AAADEF
            //             AAADEF

```
This limitation does not apply when using the constructor that takes a capacity rather than an initial string. Consequently this form is normally preferred.
``` csharp
            MutableString    mutableString = new MutableString(128);
```

## Requirements

The MutableString class uses unsafe contexts so needs to be compiled with the -unsafe compile option.
