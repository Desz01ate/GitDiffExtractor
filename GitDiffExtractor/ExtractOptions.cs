namespace DiffExtractor;

public sealed record ExtractOptions(
    string? CurrentBranch,
    string TargetBranch,
    DirectoryInfo WorkingDirectory,
    DirectoryInfo? OutputDirectory);