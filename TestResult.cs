using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

// https://xmltocsharp.azurewebsites.net/
namespace DiskSpeedTest
{
	public static class TestResult
	{
		public static Results FromXml(string xml)
		{
			XmlSerializer xmlserializer = new XmlSerializer(typeof(Results));
			TextReader textreader = new StringReader(xml);
			return xmlserializer.Deserialize(textreader) as Results;
		}

		[XmlRoot(ElementName = "Tool")]
		public class Tool
		{
			[XmlElement(ElementName = "Version")]
			public string Version { get; set; }
			[XmlElement(ElementName = "VersionDate")]
			public string VersionDate { get; set; }
		}

		[XmlRoot(ElementName = "Group")]
		public class Group
		{
			[XmlAttribute(AttributeName = "Group")]
			public string _Group { get; set; }
			[XmlAttribute(AttributeName = "MaximumProcessors")]
			public string MaximumProcessors { get; set; }
			[XmlAttribute(AttributeName = "ActiveProcessors")]
			public string ActiveProcessors { get; set; }
			[XmlAttribute(AttributeName = "ActiveProcessorMask")]
			public string ActiveProcessorMask { get; set; }
			[XmlAttribute(AttributeName = "Processors")]
			public string Processors { get; set; }
		}

		[XmlRoot(ElementName = "Node")]
		public class Node
		{
			[XmlAttribute(AttributeName = "Node")]
			public string _Node { get; set; }
			[XmlAttribute(AttributeName = "Group")]
			public string Group { get; set; }
			[XmlAttribute(AttributeName = "Processors")]
			public string Processors { get; set; }
		}

		[XmlRoot(ElementName = "Socket")]
		public class Socket
		{
			[XmlElement(ElementName = "Group")]
			public Group Group { get; set; }
		}

		[XmlRoot(ElementName = "HyperThread")]
		public class HyperThread
		{
			[XmlAttribute(AttributeName = "Group")]
			public string Group { get; set; }
			[XmlAttribute(AttributeName = "Processors")]
			public string Processors { get; set; }
		}

		[XmlRoot(ElementName = "ProcessorTopology")]
		public class ProcessorTopology
		{
			[XmlElement(ElementName = "Group")]
			public Group Group { get; set; }
			[XmlElement(ElementName = "Node")]
			public Node Node { get; set; }
			[XmlElement(ElementName = "Socket")]
			public Socket Socket { get; set; }
			[XmlElement(ElementName = "HyperThread")]
			public List<HyperThread> HyperThread { get; set; }
		}

		[XmlRoot(ElementName = "System")]
		public class System
		{
			[XmlElement(ElementName = "ComputerName")]
			public string ComputerName { get; set; }
			[XmlElement(ElementName = "Tool")]
			public Tool Tool { get; set; }
			[XmlElement(ElementName = "RunTime")]
			public string RunTime { get; set; }
			[XmlElement(ElementName = "ProcessorTopology")]
			public ProcessorTopology ProcessorTopology { get; set; }
		}

		[XmlRoot(ElementName = "WriteBufferContent")]
		public class WriteBufferContent
		{
			[XmlElement(ElementName = "Pattern")]
			public string Pattern { get; set; }
		}

		[XmlRoot(ElementName = "Target")]
		public class Target
		{
			[XmlElement(ElementName = "Path")]
			public string Path { get; set; }
			[XmlElement(ElementName = "BlockSize")]
			public string BlockSize { get; set; }
			[XmlElement(ElementName = "BaseFileOffset")]
			public string BaseFileOffset { get; set; }
			[XmlElement(ElementName = "SequentialScan")]
			public string SequentialScan { get; set; }
			[XmlElement(ElementName = "RandomAccess")]
			public string RandomAccess { get; set; }
			[XmlElement(ElementName = "TemporaryFile")]
			public string TemporaryFile { get; set; }
			[XmlElement(ElementName = "UseLargePages")]
			public string UseLargePages { get; set; }
			[XmlElement(ElementName = "WriteBufferContent")]
			public WriteBufferContent WriteBufferContent { get; set; }
			[XmlElement(ElementName = "ParallelAsyncIO")]
			public string ParallelAsyncIO { get; set; }
			[XmlElement(ElementName = "Random")]
			public string Random { get; set; }
			[XmlElement(ElementName = "ThreadStride")]
			public string ThreadStride { get; set; }
			[XmlElement(ElementName = "MaxFileSize")]
			public string MaxFileSize { get; set; }
			[XmlElement(ElementName = "RequestCount")]
			public string RequestCount { get; set; }
			[XmlElement(ElementName = "WriteRatio")]
			public string WriteRatio { get; set; }
			[XmlElement(ElementName = "Throughput")]
			public string Throughput { get; set; }
			[XmlElement(ElementName = "ThreadsPerFile")]
			public string ThreadsPerFile { get; set; }
			[XmlElement(ElementName = "IOPriority")]
			public string IOPriority { get; set; }
			[XmlElement(ElementName = "Weight")]
			public string Weight { get; set; }
			[XmlElement(ElementName = "BytesCount")]
			public string BytesCount { get; set; }
			[XmlElement(ElementName = "FileSize")]
			public string FileSize { get; set; }
			[XmlElement(ElementName = "IOCount")]
			public string IOCount { get; set; }
			[XmlElement(ElementName = "ReadBytes")]
			public string ReadBytes { get; set; }
			[XmlElement(ElementName = "ReadCount")]
			public string ReadCount { get; set; }
			[XmlElement(ElementName = "WriteBytes")]
			public string WriteBytes { get; set; }
			[XmlElement(ElementName = "WriteCount")]
			public string WriteCount { get; set; }
		}

		[XmlRoot(ElementName = "Targets")]
		public class Targets
		{
			[XmlElement(ElementName = "Target")]
			public Target Target { get; set; }
		}

		[XmlRoot(ElementName = "TimeSpan")]
		public class TimeSpan
		{
			[XmlElement(ElementName = "CompletionRoutines")]
			public string CompletionRoutines { get; set; }
			[XmlElement(ElementName = "MeasureLatency")]
			public string MeasureLatency { get; set; }
			[XmlElement(ElementName = "CalculateIopsStdDev")]
			public string CalculateIopsStdDev { get; set; }
			[XmlElement(ElementName = "DisableAffinity")]
			public string DisableAffinity { get; set; }
			[XmlElement(ElementName = "Duration")]
			public string Duration { get; set; }
			[XmlElement(ElementName = "Warmup")]
			public string Warmup { get; set; }
			[XmlElement(ElementName = "Cooldown")]
			public string Cooldown { get; set; }
			[XmlElement(ElementName = "ThreadCount")]
			public string ThreadCount { get; set; }
			[XmlElement(ElementName = "RequestCount")]
			public string RequestCount { get; set; }
			[XmlElement(ElementName = "IoBucketDuration")]
			public string IoBucketDuration { get; set; }
			[XmlElement(ElementName = "RandSeed")]
			public string RandSeed { get; set; }
			[XmlElement(ElementName = "Targets")]
			public Targets Targets { get; set; }
			[XmlElement(ElementName = "TestTimeSeconds")]
			public string TestTimeSeconds { get; set; }
			[XmlElement(ElementName = "ProcCount")]
			public string ProcCount { get; set; }
			[XmlElement(ElementName = "CpuUtilization")]
			public CpuUtilization CpuUtilization { get; set; }
			[XmlElement(ElementName = "Thread")]
			public List<Thread> Thread { get; set; }
		}

		[XmlRoot(ElementName = "TimeSpans")]
		public class TimeSpans
		{
			[XmlElement(ElementName = "TimeSpan")]
			public TimeSpan TimeSpan { get; set; }
		}

		[XmlRoot(ElementName = "Profile")]
		public class Profile
		{
			[XmlElement(ElementName = "Progress")]
			public string Progress { get; set; }
			[XmlElement(ElementName = "ResultFormat")]
			public string ResultFormat { get; set; }
			[XmlElement(ElementName = "Verbose")]
			public string Verbose { get; set; }
			[XmlElement(ElementName = "TimeSpans")]
			public TimeSpans TimeSpans { get; set; }
		}

		[XmlRoot(ElementName = "CPU")]
		public class CPU
		{
			[XmlElement(ElementName = "Group")]
			public string Group { get; set; }
			[XmlElement(ElementName = "Id")]
			public string Id { get; set; }
			[XmlElement(ElementName = "UsagePercent")]
			public string UsagePercent { get; set; }
			[XmlElement(ElementName = "UserPercent")]
			public string UserPercent { get; set; }
			[XmlElement(ElementName = "KernelPercent")]
			public string KernelPercent { get; set; }
			[XmlElement(ElementName = "IdlePercent")]
			public string IdlePercent { get; set; }
		}

		[XmlRoot(ElementName = "Average")]
		public class Average
		{
			[XmlElement(ElementName = "UsagePercent")]
			public string UsagePercent { get; set; }
			[XmlElement(ElementName = "UserPercent")]
			public string UserPercent { get; set; }
			[XmlElement(ElementName = "KernelPercent")]
			public string KernelPercent { get; set; }
			[XmlElement(ElementName = "IdlePercent")]
			public string IdlePercent { get; set; }
		}

		[XmlRoot(ElementName = "CpuUtilization")]
		public class CpuUtilization
		{
			[XmlElement(ElementName = "CPU")]
			public List<CPU> CPU { get; set; }
			[XmlElement(ElementName = "Average")]
			public Average Average { get; set; }
		}

		[XmlRoot(ElementName = "Thread")]
		public class Thread
		{
			[XmlElement(ElementName = "Id")]
			public string Id { get; set; }
			[XmlElement(ElementName = "Target")]
			public Target Target { get; set; }
		}

		[XmlRoot(ElementName = "Results")]
		public class Results
		{
			[XmlElement(ElementName = "System")]
			public System System { get; set; }
			[XmlElement(ElementName = "Profile")]
			public Profile Profile { get; set; }
			[XmlElement(ElementName = "TimeSpan")]
			public TimeSpan TimeSpan { get; set; }
		}
	}
}