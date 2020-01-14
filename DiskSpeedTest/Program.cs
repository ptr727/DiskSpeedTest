using InsaneGenius.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiskSpeedTest
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            // Load config from file
            if (args.Length != 1)
            {
                ConsoleEx.WriteLineError("Usage : DiskSpeedTest.exe [JSON config file]");
                return -1;
            }
            string configFile = args[0];
            ConsoleEx.WriteLine($"Loading config from : \"{configFile}\"");
            if (!File.Exists(configFile))
            {
                ConsoleEx.WriteLineError($"Config file not found : \"{configFile}\"");
                return -1;
            }
            //string exampleJson = Config.ToJson(new Config());
            Config config = Config.FromFile(configFile);
            if (config == null)
            {
                ConsoleEx.WriteLineError($"Unable to parse config file : \"{configFile}\"");
                return -1;
            }

            // Timestamp the result files
            if (config.TimestampResultFile)
            {
                config.DiskSpeedTest.ResultFile = Format.TimeStampFileName(config.DiskSpeedTest.ResultFile);
                config.FileIterationTest.ResultFile = Format.TimeStampFileName(config.FileIterationTest.ResultFile);
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
