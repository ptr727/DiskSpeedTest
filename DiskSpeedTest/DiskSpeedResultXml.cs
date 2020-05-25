using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Xml;

// Code generated from SpdDisk XML output
// https://xmltocsharp.azurewebsites.net/

namespace DiskSpeedTest
{
    public class DiskSpeedResultXml
    {
		public static Results FromXml(string xml)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Results));
			using XmlReader xmlReader = XmlReader.Create(new StringReader(xml));
			return xmlSerializer.Deserialize(xmlReader) as Results;
		}

		[XmlRoot(ElementName = "Target")]
		public class Target
		{
			[XmlElement(ElementName = "BytesCount")]
			public string BytesCount { get; set; }
			[XmlElement(ElementName = "IOCount")]
			public string IoCount { get; set; }
		}

		[XmlRoot(ElementName = "TimeSpan")]
		public class TimeSpan
		{
			[XmlElement(ElementName = "TestTimeSeconds")]
			public string TestTimeSeconds { get; set; }
			[XmlElement(ElementName = "Thread")]
			public List<Thread> Thread { get; set; }
		}

		[XmlRoot(ElementName = "Thread")]
		public class Thread
		{
			[XmlElement(ElementName = "Target")]
			public Target Target { get; set; }
		}

		[XmlRoot(ElementName = "Results")]
		public class Results
		{
			[XmlElement(ElementName = "TimeSpan")]
			public TimeSpan TimeSpan { get; set; }
		}
	}
}
