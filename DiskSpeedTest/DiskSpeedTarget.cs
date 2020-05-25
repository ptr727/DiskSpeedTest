using InsaneGenius.Utilities;
using System;
using System.IO;

namespace DiskSpeedTest
{
    public class DiskSpeedTarget
    {
        public bool DoesTargetExist()
        {
            // Does the target exists and is it the right size
            return File.Exists(FileName) &&
                   new FileInfo(FileName).Length == FileSize;
        }

        public bool CreateTarget()
        {
            // The default tool fill does not work well on ZFS, e.g. 64GB file is 2GB on disk
            // For write testing on COW there is no point in pre-filling the file
            // For read testing we need file contents that will result in disk IO not just decompression

            // Fill the file with random bytes
            return CreateRandomFilledFile(FileName, FileSize);
            // return DiskSpeedTool.CreateTestTarget(this) == 0;
        }

        public static bool CreateRandomFilledFile(string name, long size)
        {
            try
            {
                // Create the file
                using FileStream stream = File.Open(name, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                // Set the file length, this has no real impact on COW filesystems
                stream.SetLength(size);
                stream.Seek(0, SeekOrigin.Begin);

                // Buffer with random data
                const int buffersize = 2 * Format.MiB;
                byte[] buffer = new byte[buffersize];
                Random rand = new Random();

                // Write in buffer chunks
                long remaining = size;
                while (remaining > 0)
                {
                    // Fill buffer with random data
                    rand.NextBytes(buffer);
                    // Write
                    long writesize = Math.Min(remaining, Convert.ToInt64(buffer.Length));
                    stream.Write(buffer, 0, Convert.ToInt32(writesize));
                    // Remaining
                    remaining -= writesize;
                }

                // Close
                stream.Close();
            }
            catch (Exception e)
            {
                ConsoleEx.WriteLineError(e);
                return false;
            }
            return true;
        }

        public static bool CreateSparseFile(string name, long size)
        {
            try
            {
                // Create the file
                using FileStream stream = File.Open(name, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                // Set length
                stream.SetLength(size);

                // Close
                stream.Close();
            }
            catch (Exception e)
            {
                ConsoleEx.WriteLineError(e);
                return false;
            }
            return true;
        }

        public string FileName { get; set; }
        public long FileSize { get; set; } = 64L * Format.GiB;
    }
}
