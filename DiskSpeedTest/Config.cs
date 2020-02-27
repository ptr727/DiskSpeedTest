using System.IO;
using Newtonsoft.Json;

namespace DiskSpeedTest
{
    public class Config
    {
        public Config()
        {
            DiskSpeedTest = new DiskSpeedConfig();
            FileIterationTest = new FileIterationConfig();
        }

        [JsonProperty("timestampresultfile")]
        public bool TimestampResultFile { get; set; } = true;

        [JsonProperty("diskspeedtest")]
        public DiskSpeedConfig DiskSpeedTest { get; }

        [JsonProperty("fileiterationtest")]
        public FileIterationConfig FileIterationTest { get; }

        public static Config FromFile(string fileName)
        {
            return FromJson(File.ReadAllText(fileName));
        }

        public static string ToJson(Config config) =>
            JsonConvert.SerializeObject(config, JsonSettings);

        public static Config FromJson(string json) =>
            JsonConvert.DeserializeObject<Config>(json, JsonSettings);

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Formatting = Formatting.Indented
        };
    }
}
