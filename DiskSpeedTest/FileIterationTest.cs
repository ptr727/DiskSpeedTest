using InsaneGenius.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DiskSpeedTest
{
    public class FileIterationTest
    {
        public FileIterationTest(FileIterationConfig config)
        {
            Config = config;
            Buffer = new byte[BufferSize];
            Random random = new Random();
            random.NextBytes(Buffer);
        }

        public int Run()
        {
            // Result file
            FileIterationResultFile resultFile = new FileIterationResultFile(Config.ResultFile);
            ConsoleEx.WriteLine($"Writing results to : \"{resultFile.FileName}\"");
            resultFile.WriteHeader();
            ConsoleEx.WriteLine("");

            // Run the test for each of the target folders
            int result = 0;
            foreach (string target in Config.Targets)
            {
                // Clear target directory
                ConsoleEx.WriteLine($"Starting CreateTest for \"{target}\"");
                if (!FileEx.CreateDirectory(target) ||
                    !FileEx.DeleteInsideDirectory(target))
                {
                    resultFile.AddFailedResult(Config, target);
                    ConsoleEx.WriteLineError($"Failed to create or clear target \"{target}\"");
                    ConsoleEx.WriteLine("");

                    // Next target
                    result = -1;
                    continue;
                }

                // Recursively create files and folders
                Stopwatch timer = new Stopwatch();
                timer.Start();
                CreateFilesInFolder(target, Config.FolderDepth);
                timer.Stop();
                TimeSpan createTime = timer.Elapsed;
                ConsoleEx.WriteLine($"CreateTest Time : {createTime}");
                ConsoleEx.WriteLine("");

                // Recursively read files and folders
                ConsoleEx.WriteLine($"Starting ReadTest for \"{target}\"");
                timer.Restart();
                ReadFilesInFolder(target, out int folderCount, out int fileCount);
                timer.Stop();
                TimeSpan readTime = timer.Elapsed;
                ConsoleEx.WriteLine($"ReadTest Time : {readTime}, Folder Count : {folderCount}, File Count : {fileCount}");
                ConsoleEx.WriteLine("");

                // Clear target directory
                ConsoleEx.WriteLine($"Starting DeleteTest for \"{target}\"");
                timer.Restart();
                FileEx.DeleteInsideDirectory(target);
                timer.Stop();
                TimeSpan deleteTime = timer.Elapsed;
                ConsoleEx.WriteLine($"DeleteTest Time : {deleteTime}");
                ConsoleEx.WriteLine("");

                // Write result
                resultFile.AddResult(Config, target, folderCount, fileCount, createTime, readTime, deleteTime);
            }

            return result;
        }

        private void CreateFilesInFolder(string folder, int folderDepth)
        {
            // Create files
            for (int fileCounter = 1; fileCounter <= Config.FilesPerFolder; fileCounter ++)
            {
                string filePath = FileEx.CombinePath(folder, $"TestFile-{fileCounter}.tmp");
                WriteFile(filePath);
            }

            // Stop recursion
            if (folderDepth == 0)
                return;

            // Recursively create folders
            for (int folderCounter = 1; folderCounter <= Config.FoldersPerFolder; folderCounter ++)
            {
                // Create child folder
                string childFolder = FileEx.CombinePath(folder, $"TestFolder-{folderCounter}");
                FileEx.CreateDirectory(childFolder);

                // Recursively create files and folders
                CreateFilesInFolder(childFolder, folderDepth - 1);
            }
        }

        private void ReadFilesInFolder(string folder, out int folderCount, out int fileCount)
        {
            // Get a list of all the files and directories
            FileEx.EnumerateDirectories(new List<string> { folder }, out List<FileInfo> fileInfoList, out List<DirectoryInfo> dirInfoList);
            folderCount = dirInfoList.Count;
            fileCount = fileInfoList.Count;

            // Read every file
            foreach (FileInfo fileInfo in fileInfoList)
            {
                ReadFile(fileInfo.FullName);
            }
        }

        private void WriteFile(string filePath)
        {
            using FileStream stream = File.Create(filePath);
            int size = Config.FileSize;
            do
            {
                int chunk = size > Buffer.Length ? Buffer.Length : size;
                stream.Write(Buffer, 0, chunk);
                size -= chunk;
            }
            while (size > 0);
        }

        private void ReadFile(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            int size = (int)stream.Length;
            do
            {
                int chunk = size > Buffer.Length ? Buffer.Length : size;
                stream.Read(Buffer, 0, chunk);
                size -= chunk;
            }
            while (size > 0);
        }

        private FileIterationConfig Config;
        const int BufferSize = 64 * Format.KiB;
        byte[] Buffer;
    }
}
