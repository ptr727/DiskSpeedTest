using System;

namespace DiskSpeedTest
{
    static class DiskSpeed
    {
        public static void CreateTestTarget(TestTarget target)
        {
            // E.g.
            // diskspd.exe -c64G \\storage\testcache\testfile64g.dat
            ExecDskSpd($"-c{target.FileSize} {target.FileName}", out string _);
        }

        public static void RunSpeedTest(TestTarget target, TestParameter parameter, out string xml)
        {
            // https://github.com/microsoft/diskspd/wiki/Command-line-and-parameters
            string commands = $"-w{parameter.WriteRatio} -b{parameter.BlockSize} -F{parameter.ThreadCount} -o{parameter.OutstandingOperations} -W{parameter.WarmupTime} -d{parameter.TestTime} -r -Rxml";

            // Disable remote caching on file shares
            if (target.FileName.StartsWith(@"\\"))
                commands += " -Srw";

            // E.g.
            // diskspd -w50 -b512K -F2 -r -o8 -W60 -d120 -Srw -Rtext \\storage\testcache\testfile64g.dat > d:\diskspd_unraid_cache.txt
            ExecDskSpd($"{commands} {target.FileName}", out xml);
        }

        private static int ExecDskSpd(string command, out string xml)
        {
            const string diskSpeed = "diskspd.exe";
            Console.WriteLine($"Running : {diskSpeed} {command}");
            return ProcessEx.Execute(diskSpeed, command, out xml);
        }
    }
}
