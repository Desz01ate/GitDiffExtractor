namespace DiffExtractor;

public sealed record ExtractOptions(
    string? CurrentBranch,
    string TargetBranch,
    string? IgnoreRegex,
    DirectoryInfo WorkingDirectory,
    DirectoryInfo? OutputDirectory);