using CommandLine;
using Microsoft.Extensions.Logging;

namespace DiffExtractor.Tool;

public class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
        {
            using var logger = LoggerFactory.Create(builder => builder.AddConsole());

            IExtractor extractor = new Extractor(logger.CreateLogger<IExtractor>());

            extractor.Extract(new ExtractOptions(
                options.Current,
                options.Target,
                new DirectoryInfo(options.WorkingDirectory),
                string.IsNullOrWhiteSpace(options.OutputDirectory)
                ? default
                : new DirectoryInfo(options.OutputDirectory)));
        });
    }
}