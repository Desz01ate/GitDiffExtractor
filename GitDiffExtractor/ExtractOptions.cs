namespace DiffExtractor;

public sealed record ExtractOptions(
    string? CurrentBranch,
    string TargetBranch,
    string? IgnoreRegex,
    bool ParallelCopy,
    DirectoryInfo WorkingDirectory,
    DirectoryInfo? OutputDirectory);