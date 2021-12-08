using CommandLine;

internal class Options
{
    [Option("auto", Required = false, HelpText = "Automatically compares current branch with main branch, ignores current and target argument", Default = true)]
    public bool Auto { get; set; }

    [Option("current", Required = false, HelpText = "Current branch to compare", Default = null)]
    public string Current { get; set; }

    [Option("target", Required = false, HelpText = "Target branch to compare with", Default = "main")]
    public string Target { get; set; }

    [Option("dir", Required = true, HelpText = "Set working directory to start the extraction")]
    public string WorkingDirectory { get; set; }
}