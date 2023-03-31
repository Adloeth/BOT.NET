# FFFNet

This is a C# implementation of the FFF C++ dynamic library.

It is only half done as the Reader is missing :/

This project was for me to learn how dynamic libraries work and to make a fast and compact serializer for my other projects (especially video games). More informations about the FFF is available on the FuziotFileFormat repository.

## Development

Because this project uses a C++ library, most methods cannot be fully generic and need both C# primitive types (byte, bool, int, etc...) and custom types added by the FFF library (Tribyte, Pentabyte, BigInt, etc...). This result in multiple parts of the code needing to be copy-pasted around 20 times each with slight variations in between. There is a Variant type (named `Primitive`) but checking for types through reflections every time will slow things down a lot so manually copy-pasting everything was more efficient but REALLY annoying.

In C++ this is not a problem as we have macros. But no such concept is present in C# unfortunatly. Instead there are Visual Studio's snippets which doesn't really work here or the Source Generators which requires Roslyn a lot of other setup. Because I wanted a quick and dirty way to copy-paste templates of codes around my files, I decided to code my own "precompiler" in Python. I will definitly redo it someday... That thing is messy.

Here is how you use it : execute the script with the following parameters :
- `build` will compile all macros and call `dotnet build`
- `run` will compile all macros and call `dotnet run`
- `clear` will remove all implementations (more on that later)

The macros in themselves work the same as in C++ but need to be in comments otherwise C# will complain. You then need to write `#macro` for the precompiler to recognize you are using a macro in some way. Then, there are keywords to use to access different features :
- `decl` will declare a macro that will be saved in a dictionary, it needs a name followed by parenthesis where you can add arguments like in a function.
- `end` will end the declaration of the macro, everything in between the `decl` and the `end` macro will be the macro's body, that will be copy-pasted when you actually use the macro.
- `import` will append a file to the source code. Mostly useful inside a macro's body.
- `execute` will execute a python script and will append it's ouput to the source code. Mostly useful inside a macro's body.
- `use` will call a macro, careful : order matters, you can't call a macro that is not already declared. Macros are not shared between files ! You can then pass any string as parameters of the macro and they will get replaced once the template is appended to the source code.
- `impl` and `implend` when calling `use`, the compiled macro body (called an implementation) will be placed in between these tags and will be deleted everytime you recompile. You shouldn't add them nor touch them, it is done automatically and it can inflict serious damage to you source code. They prevent the language from yelling that code doesn't exist. If they annoy you, you can execute the script with the parameter `clear` and they will get removed.
- `header` is unused for now, the idea is for `import`ed files to have a language associated with them so that syntax highlighting is displayed in them if I ever want to make a VSCode cross-language extension for this macro thing. 
Because the macro body is in a comment, syntax highlighting is not used, this is why I originally added `import`, but it can also make code smaller.

You can also use `##` like in C++ to concatenate strings.

### Examples

#### Simple Use

This source code :

```cs
/*#macro decl MY_MACRO(type)
public type Copy(type a)
{
    return new type(a);
}
#macro end*/

//#macro use MY_MACRO( byte)
//#macro use MY_MACRO(short)
//#macro use MY_MACRO(  int)
//#macro use MY_MACRO( long)
```

Will result in :

```cs
/*#macro decl MY_MACRO(type)
public type Copy(type a)
{
    return new type(a);
}
#macro end*/

//#macro use MY_MACRO( byte)
//#macro impl
public byte Copy(byte a)
{
    return new byte(a);
}
//#macro implend
//#macro use MY_MACRO(short)
//#macro impl
public short Copy(short a)
{
    return new short(a);
}
//#macro implend
//#macro use MY_MACRO(  int)
//#macro impl
public int Copy(int a)
{
    return new int(a);
}
//#macro implend
//#macro use MY_MACRO( long)
//#macro impl
public long Copy(long a)
{
    return new long(a);
}
//#macro implend
```

Executing `python3 compiler.py clear` will revert back to the first source code (removing the `impl` and `implend` and everything in between).

#### Concatenation

This source code :

```cs
/*#macro decl MY_MACRO(typeName, type)
public void Test##typeName(type a)
{
    Console.WriteLine(a);
}
#macro end*/

//#macro use MY_MACRO(8, byte)
//#macro use MY_MACRO(16, short)
//#macro use MY_MACRO(ConcatTest, int)
```

Will result in :

```cs
/*#macro decl MY_MACRO(typeName, type)
public void Test##typeName(type a)
{
    Console.WriteLine(a);
}
#macro end*/

//#macro use MY_MACRO(8, byte)
//#macro impl
public void Test8(byte a)
{
    Console.WriteLine(a);
}
//#macro implend
//#macro use MY_MACRO(16, short)
//#macro impl
public void Test16(short a)
{
    Console.WriteLine(a);
}
//#macro implend
//#macro use MY_MACRO(ConcatTest, int)
//#macro impl
public void TestConcatTest(int a)
{
    Console.WriteLine(a);
}
//#macro implend
```