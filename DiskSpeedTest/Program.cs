using InsaneGenius.Utilities;
using System.IO;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace DiskSpeedTest
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            RootCommand rootCommand = CreateCommandLineOptions();
            return rootCommand.Invoke(args);
        }

        private static RootCommand CreateCommandLineOptions()
        {
            // Root command and global options
            RootCommand rootCommand = new RootCommand("Utility to automate iterative IO performance tests.");

            // Path to the settings file must always be specified
            rootCommand.AddOption(
                new Option<string>("--settings")
                {
                    Description = "Path to settings file.",
                    Required = true
                });

            // Write defaults to settings file
            rootCommand.AddCommand(
                new Command("writedefaults")
                {
                    Description = "Write default values to settings file.",
                    Handler = CommandHandler.Create<string>(WriteDefaultsCommand)
                });

            // Run test
            rootCommand.AddCommand(
                new Command("runtests")
                {
                    Description = "Run all tests.",
                    Handler = CommandHandler.Create<string>(RunTestsCommand)
                });

            return rootCommand;
        }

        private static int WriteDefaultsCommand(string settings)
        {
            // Write default json to file
            File.WriteAllText(settings, Config.ToJson(new Config()));
            return 0;
        }

        private static int RunTestsCommand(string settings)
        { 
            ConsoleEx.WriteLine($"Loading config from : \"{settings}\"");
            if (!File.Exists(settings))
            {
                ConsoleEx.WriteLineError($"Config file not found : \"{settings}\"");
                return -1;
            }
            //string exampleJson = Config.ToJson(new Config());
            Config config = Config.FromFile(settings);
            if (config == null)
            {
                ConsoleEx.WriteLineError($"Unable to parse config file : \"{settings}\"");
                return -1;
            }

            // Timestamp the result files
            if (config.TimestampResultFile)
            {
                config.DiskSpeedTest.ResultFile = FileEx.TimeStampFileName(config.DiskSpeedTest.ResultFile);
                config.FileIterationTest.ResultFile = FileEx.TimeStampFileName(config.FileIterationTest.ResultFile);
            }

            // Run DiskSpeedTest
            int result = 0;
            if (config.DiskSpeedTest.Enabled)
            { 
                ConsoleEx.WriteLine("");
                ConsoleEx.WriteLine("Running DiskSpeed Test ...");
                DiskSpeedTest diskSpeedTest = new DiskSpeedTest(config.DiskSpeedTest);
                if (diskSpeedTest.Run() != 0)
                    result = -1;
                ConsoleEx.WriteLine("");
            }

            // Run FileIterationTest
            if (config.FileIterationTest.Enabled)
            { 
                ConsoleEx.WriteLine("");
                ConsoleEx.WriteLine("Running FileIteration Test ...");
                FileIterationTest fileIterationTest = new FileIterationTest(config.FileIterationTest);
                if (fileIterationTest.Run() != 0)
                    result = -1;
                ConsoleEx.WriteLine("");
            }

            return result;
        }
    }
}
