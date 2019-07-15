# DbTable v1 Documentation
Thank you for choosing our first product, DbTable! If you don't know, DbTable is a small-scale database, designed for C#. This documentation will show you how to use it correctly!
## Table of Content
1. [Installation]()

2. [Usage]()

    2.1. [Overview]()
    
    2.2. [How to create it at first ?]()
    
    2.3. [Add some more funny entries!]()
    
    2.4. [Yay, I got his name, but how to look it up ?]()
    
    2.5. [Ouch, delete that naughty entry!]()
    
    2.6. [Nooo, I made a typo! How to fix it ?]()
    
3. [Advanced tools]()

    3.1. [Finally, I did it! But how to keep them for later use ?]()
    
    3.2. [I have an interesting string, how to view it ?]()
    
    3.3. [I have a .dbtable file, how to read it ?]()
    
    3.4. [Experimental: Command and filtering]()

## Installation
Before installing or using the package, you **must** install the following components:
* .NET Core 3.0 SDK (currently SDK 3.0.100-preview6-012264)
* (Optional) NUnit 3.0 or higher (to run tests)

After that, you have many options to install this lovely package!
#### Preferred method: NuGet Package Manager (PM)
PM is C# developers' best friend when working with dependencies.
```powershell
PM> Install-Package MikeDev.Db
```
However, if you don't want to get the latest release, you can specify ```-Version``` attribute (Please refer to [NuGet page](https://www.nuget.org/packages/MikeDev.Db/) for all version numbers).
#### Build it yourself!
You can obtain the source code and build it:
```bash
$ git clone https://github.com/bincity2003/MikeDev.git
$ cd MikeDev/src
$ dotnet build
```
## Usage
### Overview
In this section, we will demonstrate the ability of our product!
This is a sample program:
```csharp
using System;
using MikeDev.Db;

namespace SampleProgram
{
    public static class Program
    {
        static void Main(string[] args)
        {
            // Initialize new DbTable instance with 2 fields
            string[] Fields = {"Age", "Occupation"};
            DbTable Table = new DbTable(Fields);
            Console.WriteLine("Number of fields: " + Table.GetFieldLength);
            Console.WriteLine("Field names: " + Table.FieldNames.ToString());
            
            // Add a single entry
            Table.AddEntry("Mike", "16", "Student");
            Console.WriteLine("Number of entries: ", Table.Count);
            Console.WriteLine("Mike's age is: ", Table["Mike"][0]);
            
            // Replace that entry
            Table.ReplaceEntry("Mike", "18", "Student");
            Console.WriteLine("After change, Mike's age is: ", Table["Mike"][0]);
            
            // Remove that entry
            Table.RemoveEntry("Mike");
            Console.WriteLine("Number of entries: ", Table.Count);
        }
    }
}
```
In this sample program, you can see pretty much of the functionalities. For more details, you should refer to later sections!
