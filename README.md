# diff-extractor
A tool to make a copy of diff files between git branches

### Prerequisite
- Git (obliviously) [https://git-scm.com/]
- .NET 6 Runtime [https://dotnet.microsoft.com/download/dotnet/6.0]

### Supported OS
- Windows
- Linux

### How to use
The tool will compare current branch, for example `add-feature-abc` with `main` branch on default when the program is run without additional arguments at current directory

### Supported command line arguments

| Name    | Required | Default Value | Description                               |
|---------|----------|---------------|-------------------------------------------|
| dir     | yes      | -             | Working directory to start the extraction |
| current | no       | -             | Current branch to compare                 |
| target  | no       | main          | Target branch to compare with             |

