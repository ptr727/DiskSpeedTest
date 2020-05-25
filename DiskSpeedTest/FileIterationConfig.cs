using System.Collections.Generic;
using InsaneGenius.Utilities;
using Newtonsoft.Json;

namespace DiskSpeedTest
{
    public class FileIterationConfig
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;

        [JsonProperty("resultfile")]
        public string ResultFile { get; set; } = @"FileIterationTestResult.csv";

        [JsonProperty("targets")]
        public List<string> Targets { get; } = new List<string>();

        [JsonProperty("folderdepth")]
        public int FolderDepth { get; set; } = 3;

        [JsonProperty("foldersperfolder")]
        public int FoldersPerFolder { get; set; } = 3;

        [JsonProperty("filesperfolder")]
        public int FilesPerFolder { get; set; } = 1000;

        [JsonProperty("filesize")]
        public int FileSize { get; set; } = 64 * Format.KiB;
    }
}
