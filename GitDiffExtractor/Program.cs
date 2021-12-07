using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GitDiffExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { ".", "main" };
            }

            Directory.SetCurrentDirectory(args[0]);

            if (!Directory.Exists(".git"))
            {
                Console.WriteLine("Current working directory contains no .git folder");
                return;
            }

            var branches = Exec("git branch").Split("\n").Select(x => x.Trim());

            var current = branches.Single(x => x.StartsWith("*")).Replace("* ", "");
            var compare = branches.Single(x => x == args[1]);

            var diffs = Exec($"git diff --name-status {compare}..{current}").Split("\n");

            var outDir = Directory.CreateDirectory(Path.Combine("patch", current));

            foreach (var diff in diffs)
            {
                if (string.IsNullOrWhiteSpace(diff))
                {
                    continue;
                }

                var split = diff.Split("\t");

                var mode = split[0];

                // skip deleted files
                if (mode == "D")
                {
                    continue;
                }

                var file = split[1];

                var fileInfo = new FileInfo(file);

                var dir = Path.GetDirectoryName(Path.GetRelativePath(".", fileInfo.FullName));

                var newDir = Path.Combine(outDir.FullName, dir);

                Directory.CreateDirectory(newDir);

                fileInfo.CopyTo(Path.Combine(newDir, fileInfo.Name), true);
            }

            Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", outDir.FullName);
        }

        static string Exec(string command)
        {
            var psi = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = string.Format("/c \"{0}\"", command),
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };

            var proc = Process.Start(psi);

            return proc.StandardOutput.ReadToEnd();
        }
    }
}
