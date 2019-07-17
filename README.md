# MikeDev [![Build Status](https://dev.azure.com/bincity2003/MikeDev/_apis/build/status/bincity2003.MikeDev?branchName=master)](https://dev.azure.com/bincity2003/MikeDev/_build/latest?definitionId=3&branchName=master) [![Build Status](https://travis-ci.org/bincity2003/MikeDev.svg?branch=master)](https://travis-ci.org/bincity2003/MikeDev) ![GitHub](https://img.shields.io/github/license/bincity2003/MikeDev.svg?color=red&label=License&logo=MIT) ![GitHub release](https://img.shields.io/github/release/bincity2003/MikeDev.svg?logoColor=orange)
This is the place where I will stored all general-purpose project in the future.
## Description
Here, I'll publish all my (in free time) projects. Currently, it has:
* [DbTable 1.0.2](https://github.com/bincity2003/MikeDev/tree/dbtable-development/MikeDev.Db) (A simple NoSQL database)
* [MLogger 1.0.1](https://github.com/bincity2003/MikeDev/tree/mlogger-development/MikeDev.Debug) (A simple debugging tool)
## Prerequisites
* .NET Core 3.0 SDK (currently SDK 3.0.100-preview6-012264)
* Your favorite text editor
* NUnit 3.0 or higher (in order to run tests)
## Installation
### Git
All you have to do is to clone this repository and build it:
```bash
$ git clone https://github.com/bincity2003/MikeDev.git
$ cd MikeDev/src
$ dotnet build
```
### NuGet package manager
You can use NuGet package manager to install the latest release.
```powershell
PM> Install-Package <package_name>
```
Available package names are:
```
MikeDev.Database    (for DbTable)
MikeDev.Debug       (for MLogger)
```
**After installation, just start working!**
## Known issues
### DbTable
No known issue is found recently.
### MLogger
No known issue is found recently.
## Contribution
Any contribution is welcome! But you should first refer to our
[Contribution Guidelines](https://github.com/bincity2003/MikeDev/blob/master/CONTRIBUTING.md)
to get more information on how to involve!
## License
MikeDev repo is licensed under [MIT License](https://github.com/bincity2003/MikeDev/blob/master/LICENSE)
