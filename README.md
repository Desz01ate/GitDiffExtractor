# GitDiffExtractor
A tool to make a copy of diff files between git branches

### Prerequisite
- Git (obliviously) [https://git-scm.com/]
- .NET 6 Runtime [https://dotnet.microsoft.com/download/dotnet/6.0]

### Supported OS
- Windows
- Linux

### How to use
The tool will compare current branch, for example `add-feature-abc` with `main` branch on default when the program is run without additional arguments at current directory

you can give a specify arguments with

args
- 0: for target working directory (default: .)
- 1: for target comparer branch (default: main)
