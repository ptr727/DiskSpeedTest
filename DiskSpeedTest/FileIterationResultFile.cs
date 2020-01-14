using System;
using System.IO;

namespace DiskSpeedTest
{
    public class FileIterationResultFile
    {
        public FileIterationResultFile(string fileName)
        {
            FileName = fileName;
        }

        public void WriteHeader()
        {
            // Create or replace the contents of the file
            File.WriteAllText(FileName, Header + Environment.NewLine);
        }

        public void AddResult(FileIterationConfig config, string target, int foldercount, int fileCount, TimeSpan createTime, TimeSpan readTime, TimeSpan deleteTime)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            // Add a result line
            string result = $"{DateTime.UtcNow:s}, \"{target}\", {config.FileSize}, {config.FolderDepth}" +
                $", {config.FoldersPerFolder}, {config.FilesPerFolder}, {foldercount}, {fileCount}, {createTime.TotalSeconds}, {readTime.TotalSeconds}, {deleteTime.TotalSeconds}";
            File.AppendAllText(FileName, result + Environment.NewLine);
        }

        public void AddFailedResult(FileIterationConfig config, string target)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            // Add a result line
            string result = $"{DateTime.UtcNow:s}, \"{target}\", {config.FileSize}, {config.FolderDepth}" +
                $", {config.FoldersPerFolder}, {config.FilesPerFolder}, 0, 0, 0.0, 0.0, 0.0";
            File.AppendAllText(FileName, result + Environment.NewLine);
        }

        public string FileName { get; }
        private const string Header = "UTC, Target, FileSize, FolderDepth, FoldersPerFolder, FilesPerFolder, FolderCount, FileCount, CreateTime, ReadTime, DeleteTime";
    }
}
