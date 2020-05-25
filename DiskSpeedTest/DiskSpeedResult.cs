using System;
using System.Globalization;

namespace DiskSpeedTest
{
	public class DiskSpeedResult
	{
		public void FromXml(string xml)
		{
			// Parse xml
			DiskSpeedResultXml.Results xmlResults = DiskSpeedResultXml.FromXml(xml);

			// Calculated IO's and Bytes per second
			Bytes = 0;
			Ios = 0;
            foreach (DiskSpeedResultXml.Thread thread in xmlResults.TimeSpan.Thread)
            {
                Bytes += Convert.ToInt64(thread.Target.BytesCount, CultureInfo.InvariantCulture);
                Ios += Convert.ToInt64(thread.Target.IoCount, CultureInfo.InvariantCulture);
            }
			Seconds = Convert.ToDouble(xmlResults.TimeSpan.TestTimeSeconds, CultureInfo.InvariantCulture);
			BytesPerSec = Bytes / Seconds;
			IosPerSec = Ios / Seconds;
		}

		public long Bytes { get; private set; }
		public long Ios { get; private set; }
		public double Seconds { get; private set; }
		public double BytesPerSec { get; private set; }
		public double IosPerSec { get; private set; }
	}
}