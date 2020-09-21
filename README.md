# MutableString

MutableString is a C# struct that wraps the String class and provides functions for changing the string contents without creating a new string.

## Why would I want to do that?

In performance constrained environments such as video games garbage collection is expensive so it can be desirable to modify a string rather than creating a new one.

## Usage

```csharp
            MutableString mutableString = new MutableString("AAAAAA");
            Console.WriteLine("mutableString is " + mutableString);
            Console.WriteLine($"mutableString == 'AAAAAA' is {mutableString == "AAAAAA"}");
            mutableString.SetSubString(1, "BCDE");
            mutableString[5] = 'F';
            Console.WriteLine("mutableString is " + mutableString);
            Console.WriteLine($"mutableString == 'ABCDEF' is {mutableString == "ABCDEF"}");
```
```
Console Output:
> mutableString is AAAAAA
> mutableString == 'AAAAAA' is True
> mutableString is ABCDEF
> mutableString == 'ABCDEF' is True
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
```
```
Console Output:
> AAADEF
> AAADEF
```
