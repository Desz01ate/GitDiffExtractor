namespace DiffExtractor;

public sealed record ExtractOptions(
    string? CurrentBranch,
    string TargetBranch,
    string? IgnoreRegex,
    bool ParallelCopy,
    bool TestMode,
    DirectoryInfo WorkingDirectory,
    DirectoryInfo? OutputDirectory);