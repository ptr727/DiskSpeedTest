﻿using InsaneGenius.Utilities;
using System;

namespace DiskSpeedTest
{
    internal static class DiskSpeedTool
    {
        public static int CreateTestTarget(DiskSpeedTarget target)
        {
            // E.g.
            // diskspd.exe -c64G \\storage\testcache\testfile64g.dat
            return ExecDiskSpd($"-c{target.FileSize} {target.FileName}", out string _);
        }

        public static int RunSpeedTest(DiskSpeedTarget target, DiskSpeedParameter parameter, out string xml)
        {
            // https://github.com/microsoft/diskspd/wiki/Command-line-and-parameters
            string commands = $"-z -Zr -w{parameter.WriteRatio} -b{parameter.BlockSize} -F{parameter.ThreadCount} -o{parameter.OutstandingOperations} -W{parameter.WarmupTime} -d{parameter.TestTime} -r -Rxml";

            // Disable remote caching on file shares
            if (target.FileName.StartsWith(@"\\", StringComparison.InvariantCulture))
                commands += " -Srw";

            // E.g.
            // diskspd -z -Zr -w50 -b512K -F2 -r -o8 -W60 -d120 -Srw -Rtext \\storage\testcache\testfile64g.dat > d:\diskspd_unraid_cache.txt
            return ExecDiskSpd($"{commands} {target.FileName}", out xml);
        }

        private static int ExecDiskSpd(string command, out string xml)
        {
            const string diskSpdExe = "diskspd.exe";
            ConsoleEx.WriteLine($"Running : {diskSpdExe} {command}");
            return ProcessEx.Execute(diskSpdExe, command, out xml);
        }
    }
}
