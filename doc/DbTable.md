# DbTable v1 Documentation
Thank you for choosing our first product, DbTable! If you don't know, DbTable is a small-scale database, designed for C#. This documentation will show you how to use it correctly!
## Table of Content
1. [Installation](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#installation)
    
    1.1. [Using NuGet](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#using-nuget-package-manager)

    1.2. [Manual installation](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.mdinstall-the-build-manually)

    1.3. [Self-built binary](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#build-it-yourself)

2. [Usage](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#usage)

    2.1. [Overview](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#overview)
    
    2.2. [How to create it at first ?](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#create-a-new-dbtable-instance)
    
    2.3. [Add some more funny entries!](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#add-new-entries)
    
    2.4. [Yay, I got his name, but how to look it up ?](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#value-lookup)
    
    2.5. [Ouch, delete that naughty entry!](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#remove-existing-entries)
    
    2.6. [Nooo, I made a typo! How to fix it ?](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#replace-existing-entries)
    
3. [Advanced tools](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#advanced-tools)

    3.1. [Finally, I did it! But how to keep them for later use ?](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#export)
    
    3.2. [I have an interesting string, how to view it ?](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#import)
    
    3.3. [I have a .dbtable file, how to read it ?](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#import)
    
    3.4. [Experimental: Command and filtering](https://github.com/bincity2003/MikeDev/blob/master/doc/DbTable.md#command-and-filtering)

## Installation
Before installing or using the package, you **must** install the following components:
* .NET Core 3.0 SDK (currently SDK 3.0.100-preview6-012264)
* (Optional) NUnit 3.0 or higher (to run tests)

After that, you have many options to install this lovely package!
#### Using NuGet Package Manager
NuGet Package Manager (PM) is C# developers' best friend when working with dependencies.
```powershell
PM> Install-Package MikeDev.Db
```
However, if you don't want to get the latest release, you can specify ```-Version``` attribute (Please refer to [NuGet page](https://www.nuget.org/packages/MikeDev.Db/) for all version numbers).
#### Install the build manually
If you hate NuGet for some reasons, you can, of course, download the binaries from our [GitHub Release page]()
Then, decompress it and include it as dependencies in your project!
#### Build it yourself
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
### Create a new DbTable instance
You can create an new DbTable, just by supplying the field names.
```csharp
DbTable table = new DbTable(new string[] {"Name", "Age", "Occupation"});
```
As a reminder, after finish using it, please call DbTable.Dispose() for GC optimization.
#### FAQ
**Q**: Why do I need to give the field's names on instantiation ?

**A**: Because the fields' names are static, meaning you give once, at instantiation.


**Q**: Can I change the fields' name later ?

**A**: Unfortunately, you can't. Since they're static, meaning you can't change it after.
### Add new entries
There are two way to add entries, either every single one or all of them at once. This example will demonstrate both methods.  
You can use ```DbTable.Add(string name, params string[] value)```:

```csharp
DbTable table = new DbTable(new string[] { "Name", "Age", "Occupation" });

table.Add("Mike", "Mike Nguyen", "16", "Student");			// The first param is actually the (unique) identifier.
table.Add("John", "John Tran", "18", "Teacher");
```

or you can use ```DbTable.Add(string[] names, params string[][] values)```:

```csharp
DbTable table = new DbTable(new string[] { "Name", "Age", "Occupation" });

string[] names = { "Mike", "John", "Kate" };
string[][] values = { { "Mike Nguyen", "16", "Student" },
		      { "John Tran", "18", "Teacher" },
		      { "Kate Huynh", "14", "Student"} };

table.Add(names, values);
```
#### FAQ
**Q**: What if I didn't give the correct number of values ?

**A**: Then, a DbTableException will occur, telling you the reason.


**Q**: Why do I need to give the name twice ?

**A**: Actually, you don't. While the first param is called ```name```, it's actually a (unique) identifier for the entry.
I'm considering changing to ```id``` for clarity.
### Value look-up
Given the name, you can easily look it up using ```DbTable[string name]```.
This property will return a string array of values.

Example:
```csharp
DbTable table = new DbTable(new string[] { "Name", "Age", "Occupation" });
table.Add("Mike", "Mike Nguyen", "16", "Student");

string[] values = table["Mike"];
```

#### FAQ:
**Q**: What if I didn't give it the correct name ?

**A**: Again, a DbTableException will occur!
### Remove existing entries
As same as the Add methods, we also have two ways to remove existing entries, either one at a time or all of them once.
You can use ```DbTable.Remove(string name)```:

```csharp
DbTable table = new DbTable(new string[] { "Name", "Age", "Occupation" });

table.Add("Mike", "Mike Nguyen", "16", "Student");
// Adding entries ....

table.Remove("Mike");
```

or you can use ```DbTable.Remove(string[] names)```:

```csharp
DbTable table = new DbTable(new string[] { "Name", "Age", "Occupation" });

table.Add("Mike", "Mike Nguyen", "16", "Student");
table.Add("John", "John Tran", "18", "Teacher");
// Adding entries ....

string[] names = { "Mike", "John" };
table.Remove(names);
```
#### FAQ:
**Q**: What if I didn't give it the correct name ?

**A**: Again, a DbTableException will occur!
### Replace existing entries
Once again, you have two ways to replace entries.
You can use ```DbTable.Replace(string name, params string[] value)``` or ```DbTable.Replace(string[] names, params string[][] value)```
## Advanced tools
### Export
You can export data and save it for later use. You can do this in two ways, either exporting to string or exporting to file.
If you're exporting to string, you can also protect it with encryption...
Usage: ```DbTable.Export()``` or ```DbTable.Export(string filename)```
### Import
Likewise, you can also import data from external storage in two ways (constructors), either importing string or importing from file.

Usage: ```DbTable.DbTable(string fileName)``` or ```DbTable.DbTable(string data, bool isData)```

**Note**: In the second way, isData must be set to true. Otherwise, exception will occur.
### Command and Filtering
In development! Stay tuned!
