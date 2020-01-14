using InsaneGenius.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FileIterationTest
{
    class Program
    {
        // https://forums.unraid.net/bug-reports/stable-releases/680-smb-ver-4113-significant-performance-decrease-when-opening-files-in-folders-with-1000-files-in-them-r789/
        static void Main()
        {
            const string targetDirectory = @"C:\Temp\FileIterationTest";
            const int folderDepth = 3;

            // Clear target directory
            FileEx.CreateDirectory(targetDirectory);
            FileEx.DeleteInsideDirectory(targetDirectory);

            // Start the stopwatch
            ConsoleEx.WriteLine("Starting create test...");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            // Recursively create files and folders
            CreateFilesInFolder(targetDirectory, folderDepth);

            timer.Stop();
            ConsoleEx.WriteLine($"Create test time : {timer.Elapsed}");

            ConsoleEx.WriteLine("");

            // Start the stopwatch
            ConsoleEx.WriteLine("Starting read test...");
            timer.Start();

            // Recursively read files and folders
            ReadFilesInFolder(targetDirectory, out int folderCount, out int fileCount);

            timer.Stop();
            ConsoleEx.WriteLine($"Read test time : {timer.Elapsed}");
            ConsoleEx.WriteLine($"Folder count : {folderCount}, File count : {fileCount}");

            // Clear target directory
            FileEx.DeleteInsideDirectory(targetDirectory);
        }

        static void CreateFilesInFolder(string folder, int folderDepth)
        {
            const int filesPerFolder = 1000;
            const int fileSize = 64 * 1024;
            const int foldersPerFolder = 3;

            // Create files
            for (int fileCounter = 1; fileCounter <= filesPerFolder; fileCounter ++)
            {
                string filePath = FileEx.CombinePath(folder, $"TestFile-{fileCounter}.tmp");
                using FileStream stream = File.Create(filePath);
                stream.SetLength(fileSize);
            }

            // Stop recursion
            if (folderDepth == 0)
                return;

            // Recursively create folders
            for (int folderCounter = 1; folderCounter <= foldersPerFolder; folderCounter ++)
            {
                // Create child folder
                string childFolder = FileEx.CombinePath(folder, $"TestFolder-{folderCounter}");
                FileEx.CreateDirectory(childFolder);

                // Recursively create files and folders
                CreateFilesInFolder(childFolder, folderDepth - 1);
            }
        }

        static void ReadFilesInFolder(string targetDirectory, out int folderCount, out int fileCount)
        {
            // Get a list of all the files and directories
            FileEx.EnumerateDirectories(new List<string>{ targetDirectory }, out List<FileInfo> fileInfoList, out List<DirectoryInfo> dirInfoList);
            folderCount = dirInfoList.Count;
            fileCount = fileInfoList.Count;

            // Read every file
            foreach (FileInfo fileInfo in fileInfoList)
            {
                using FileStream stream = File.OpenRead(fileInfo.FullName);
                stream.Seek(fileInfo.Length, SeekOrigin.Begin);
            }
        }
    }
}
