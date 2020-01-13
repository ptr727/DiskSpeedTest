using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DiskSpeedTest
{
    public class Config
    {
        public Config()
        {
            TestTargets = new List<string>();
        }

        [JsonProperty("resultsfile")]
        public string ResultsFile { get; set; } = @"SpeedTestResults.csv";

        [JsonProperty("testtargets")]
        public List<string> TestTargets { get; }

        [JsonProperty("testtargetsize")]
        public long TestTargetSize { get; set; } = 64L * Format.GiB;

        [JsonProperty("blocksizebegin")]
        public int BlockSizeBegin { get; set; } = 4 * Format.KiB;

        [JsonProperty("blocksizeend")]
        public int BlockSizeEnd { get; set; } = 2 * Format.MiB;

        [JsonProperty("warmuptime")]
        public int WarmupTime { get; set; } = 30;

        [JsonProperty("testtime")]
        public int TestTime { get; set; } = 120;

        [JsonProperty("resttime")]
        public int RestTime { get; set; }

        public static Config FromFile(string fileName)
        {
            return FromJson(File.ReadAllText(fileName));
        }

        public static string ToJson(Config config) =>
            JsonConvert.SerializeObject(config, Settings);

        public static Config FromJson(string json) =>
            JsonConvert.DeserializeObject<Config>(json, Settings);

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Formatting = Formatting.Indented
        };
    }
}
