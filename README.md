# diff-extractor
A tool to make a copy of diff files between git branches

### Prerequisite
- [Git](https://git-scm.com/) (obliviously)
- [.NET 6 Runtime](https://dotnet.microsoft.com/download/dotnet/6.0)

### Supported OS
- Windows
- Linux

### How to use
Download the tool from [Release](https://github.com/Desz01ate/diff-extractor/releases) section

simply run the tool by

```
dotnet ./GitDiffExtractor.dll --dir ~/some/working/directory
```

above command will result in extract copy of current branch in `~/some/working/directory' by comparing against main branch

### Supported command line arguments

| Name    | Required | Default Value | Description                               |
|---------|----------|---------------|-------------------------------------------|
| dir     | yes      | -             | Working directory to start the extraction |
| current | no       | -             | Current branch to compare                 |
| target  | no       | main          | Target branch to compare with             |