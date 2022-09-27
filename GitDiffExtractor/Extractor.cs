namespace DiffExtractor;

public sealed class Extractor : IExtractor
{
    private readonly ILogger<IExtractor>? logger;

    public Extractor(ILogger<IExtractor>? logger)
    {
        this.logger = logger;
    }

    public void Extract(ExtractOptions options)
    {
        Directory.SetCurrentDirectory(options.WorkingDirectory.FullName);

        if (!Directory.Exists(".git"))
        {
            this.logger?.LogError("Unable to extract as .git directory not found.");

            return;
        }

        var current = options.CurrentBranch;
        var target = string.Empty;

        var branches = Exec("git branch").Split('\n').Select(l => l.Trim()).ToArray();

        if (string.IsNullOrWhiteSpace(current))
        {
            current = branches.Single(b => b.StartsWith("*")).Replace("* ", string.Empty);
        }

        target = branches.SingleOrDefault(b => b == options.TargetBranch);

        if (string.IsNullOrWhiteSpace(current) || string.IsNullOrWhiteSpace(target))
        {
            this.logger?.LogError(
                $"Unable to find '{current}' or '{target}' branch, please check your git branch and try again.");

            return;
        }

        this.logger?.LogInformation($"Current working directory: {Directory.GetCurrentDirectory()}");
        this.logger?.LogInformation($"Current target branch: {target}");
        this.logger?.LogInformation($"Current branch: {current}");

        var diffs = Exec($"git diff --name-status {target}..{current}")
            .Split('\n')
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split('\t'))
            .Select(p => new
            {
                Mode = p[0],
                File = p[1],
            })
            .Where(o => o.Mode != "D");

        if (!string.IsNullOrWhiteSpace(options.IgnoreRegex))
        {
            diffs = diffs.Where(o => !Regex.IsMatch(o.File, options.IgnoreRegex, RegexOptions.Compiled));
        }

        if (options.TestMode)
        {
            TestMode();
        }
        else
        {
            DoCopy();
        }

        void DoCopy()
        {
            var outputDirectory = options.OutputDirectory ?? new DirectoryInfo(Path.Combine("diff-extract", current));

            outputDirectory.Create();

            var source = options.ParallelCopy ? diffs.AsParallel() : diffs;

            foreach (var diff in source)
            {
                var file = diff.File;

                var fileInfo = new FileInfo(file);

                var dir = Path.GetDirectoryName(Path.GetRelativePath(".", fileInfo.FullName));

                var newDir = Path.Combine(outputDirectory.FullName, dir!);

                Directory.CreateDirectory(newDir);

                try
                {
                    fileInfo.CopyTo(Path.Combine(newDir, fileInfo.Name), true);
                }
                catch
                {
                    this.logger?.LogError($"Unable to make a copy of '{fileInfo.Name}'");
                }
            }

            this.logger?.LogInformation($"Copy of `{current}` branch has been created at {outputDirectory.FullName}.");
        }

        void TestMode()
        {
            var outputDirectory = options.OutputDirectory ?? new DirectoryInfo(Path.Combine("diff-extract", current));

            this.logger?.LogInformation("[Test Mode] {OutputDir} created", outputDirectory);

            var source = options.ParallelCopy ? diffs.AsParallel() : diffs;

            foreach (var diff in source)
            {
                var file = diff.File;

                var fileInfo = new FileInfo(file);

                var dir = Path.GetDirectoryName(Path.GetRelativePath(".", fileInfo.FullName));

                var newDir = Path.Combine(outputDirectory.FullName, dir!);

                var dest = Path.Combine(newDir, fileInfo.Name);

                this.logger?.LogInformation("[Test Mode] {Dest} copied", dest);
            }

            this.logger?.LogInformation(
                $"[Test Mode]  Copy of `{current}` branch has been created at {outputDirectory.FullName}.");
        }
    }

    private static string Exec(string command)
    {
        var psi = new ProcessStartInfo()
        {
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        };

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            psi.FileName = "cmd.exe";
            psi.Arguments = string.Format("/c \"{0}\"", command);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                 RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            psi.FileName = "/bin/bash";
            psi.Arguments = string.Format("-c \"{0}\"", command);
        }
        else
        {
            throw new NotImplementedException();
        }

        using var proc = Process.Start(psi);

        if (proc == null)
        {
            throw new InvalidOperationException($"Unable to start {psi.FileName}.");
        }

        return proc.StandardOutput.ReadToEnd();
    }
}