# MikeDev [![Build Status](https://dev.azure.com/bincity2003/MikeDev/_apis/build/status/bincity2003.MikeDev?branchName=master)](https://dev.azure.com/bincity2003/MikeDev/_build/latest?definitionId=3&branchName=master) [![Build Status](https://travis-ci.org/bincity2003/MikeDev.svg?branch=master)](https://travis-ci.org/bincity2003/MikeDev) ![GitHub](https://img.shields.io/github/license/bincity2003/MikeDev.svg?color=red&label=License&logo=MIT)
This is the place where I will stored all general-purpose project in the future.
## Description
Ever feeling hard to implement a brand new database for your project ? No more. Because you have us! This repository will bring you many useful libraries that one day you will need!
## All releases
| Status                 | Package name     | Version  | Releases       |
|------------------------|------------------|----------|----------------|
| ![Build Status][da-bd] | MikeDev.Database | 1.0.0    | [NuGet][da-re] |
| ![Build Status][de-bd] | MikeDev.Debug    | 1.0.1    | [NuGet][de-re] |
| ![Build Status][co-bd] | MikeDev.Config   | 1.0.0    | [NuGet][co-re] |
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
MikeDev.Config      (for CConfig)
```
**After installation, just start working!**
## Known issues
### DbTable
No known issue is found recently.
### MLogger
No known issue is found recently.
### CConfig
No known issue is found recently.
## Contribution
Any contribution is welcome! But you should first refer to our
[Contribution Guidelines](https://github.com/bincity2003/MikeDev/blob/master/CONTRIBUTING.md)
to get more information on how to get involved!
## License
MikeDev repo is licensed under [MIT License](https://github.com/bincity2003/MikeDev/blob/master/LICENSE)

[da-re]: https://www.nuget.org/packages/MikeDev.Database
[de-re]: https://www.nuget.org/packages/MikeDev.Debug
[co-re]: https://www.nuget.org/packages/MikeDev.Config
[da-bd]: https://travis-ci.org/bincity2003/MikeDev.svg?branch=dbtable-development
[de-bd]: https://travis-ci.org/bincity2003/MikeDev.svg?branch=mlogger-development
[co-bd]: https://travis-ci.org/bincity2003/MikeDev.svg?branch=cconfig-development
