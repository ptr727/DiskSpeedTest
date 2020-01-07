using System;
using System.Diagnostics;
using System.Text;

namespace DiskSpeedTest
{
    public class ProcessEx
    {
        public ProcessEx()
        {
            RedirectOutput = false;
            PrintOutput = false;
            _outputtext = new StringBuilder();
            RedirectError = false;
            PrintError = false;
            _errortext = new StringBuilder();
        }

        public int ExecuteEx(string executable, string parameters)
        {
            // Create new process
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = executable,
                    Arguments = parameters,
                    UseShellExecute = false
                }
            };

            // Output handlers
            void Outputhandler(object s, DataReceivedEventArgs e) => OutputHandler(e, this);
            void Errorhandler(object s, DataReceivedEventArgs e) => ErrorHandler(e, this);
            process.StartInfo.RedirectStandardOutput = RedirectOutput;
            process.OutputDataReceived += Outputhandler;
            process.StartInfo.RedirectStandardError = RedirectError;
            process.ErrorDataReceived += Errorhandler;
            try
            {
                // Start process
                _outputtext.Clear();
                _errortext.Clear();
                process.Start();

                // Read output and error
                if (RedirectOutput)
                    process.BeginOutputReadLine();
                if (RedirectError)
                    process.BeginErrorReadLine();

                // Wait for exit
                process.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            return process.ExitCode;
        }

        private static void OutputHandler(DataReceivedEventArgs e, ProcessEx redirected)
        {
            if (string.IsNullOrEmpty(e.Data))
                return;

            redirected._outputtext.AppendLine(e.Data);
            if (redirected.PrintOutput)
                Console.WriteLine(e.Data);
        }

        private static void ErrorHandler(DataReceivedEventArgs e, ProcessEx redirected)
        {
            if (string.IsNullOrEmpty(e.Data))
                return;

            redirected._errortext.AppendLine(e.Data);
            if (redirected.PrintError)
                Console.WriteLine(e.Data);
        }

        public bool RedirectOutput { get; set; }
        public bool PrintOutput { get; set; }
        public string OutputText => _outputtext.ToString();
        public bool RedirectError { get; set; }
        public bool PrintError { get; set; }
        public string ErrorText => _errortext.ToString();

        public static int Execute(string executable, string parameters)
        {
            // Create new process
            ProcessEx process = new ProcessEx();
            return process.ExecuteEx(executable, parameters);
        }

        public static int Execute(string executable, string parameters, out string output)
        {
            // Create new process
            ProcessEx process = new ProcessEx
            {
                RedirectOutput = true,
                PrintOutput = false
            };
            int exitcode = process.ExecuteEx(executable, parameters);
            output = process.OutputText;

            return exitcode;
        }

        public static int Execute(string executable, string parameters, out string output, out string error)
        {
            // Create new process
            ProcessEx process = new ProcessEx
            {
                RedirectOutput = true,
                PrintOutput = false,
                RedirectError = true,
                PrintError = false
            };
            int exitcode = process.ExecuteEx(executable, parameters);
            output = process.OutputText;
            error = process.ErrorText;

            return exitcode;
        }

        private readonly StringBuilder _outputtext;
        private readonly StringBuilder _errortext;
    }
}
