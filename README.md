# MutableString

MutableString is a C# struct that wraps the String class and provides functions for changing the string contents without creating a new string.

## Why would I want to do that?

In C# strings are immutable so its not possible to change a string without creating a new string and that allocates memory which will eventually be garbage collected.

In performance constrained environments such as video games, garbage collection is expensive so it can be desirable to modify a string rather than creating a new one.

## How Does It Work

The .NET framework defines the memory layout for string objects so that it can provide optimised code paths for manipulating strings. MutableStrings uses this layout to make changes directly to the character buffer of a string.

## Usage

```csharp
            MutableString mutableString1 = new MutableString("ABCDEF");
            MutableString mutableString2 = new MutableString("AAAAAA");

            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            // Displays ABCDEF==AAAAAA is False

            Console.WriteLine($"{mutableString1}=={mutableString2} is {mutableString1 == mutableString2}");
            mutableString2.SetSubString(3, "DEF");
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

## Requirements

The MutableString class uses unsafe contexts so needs to be compiled with the -unsafe compile option.
