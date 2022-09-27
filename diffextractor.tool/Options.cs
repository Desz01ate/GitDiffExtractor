using CommandLine;

namespace DiffExtractor.Tool;

internal class Options
{
    [Option("current", Required = false, HelpText = "Current branch to compare", Default = null)]
    public string? Current { get; set; }

    [Option("target", Required = false, HelpText = "Target branch to compare with", Default = "main")]
    public string Target { get; set; } = null!;

    [Option("dir", Required = true, HelpText = "Working directory to start the extraction")]
    public string WorkingDirectory { get; set; } = null!;

    [Option("outdir", Required = false, HelpText = "Working directory to start the extraction")]
    public string? OutputDirectory { get; set; }

    [Option("ignregex", Required = false, HelpText = "Ignore files that match given regular expression.")]
    public string? IgnoreRegex { get; set; }

    [Option("prlcpy", Required = false, HelpText = "Set if copy should be executed in parallel", Default = false)]
    public bool ParallelCopy { get; set; }
}