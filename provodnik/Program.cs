using System;
using System.IO;
using System.Diagnostics;

public static class Explorer
{
    private static void Main()
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();
        string[] driveOptions = GetDriveOptions(allDrives);

        int selectedDrive = ArrowMenu.ShowMenu(driveOptions);
        string selectedDriveName = allDrives[selectedDrive].Name;

        ExploreFolders(selectedDriveName);
    }

    private static string[] GetDriveOptions(DriveInfo[] drives)
    {
        string[] options = new string[drives.Length];

        for (int i = 0; i < drives.Length; i++)
        {
            options[i] = $"{drives[i].Name} " +
                         $"{drives[i].AvailableFreeSpace / (1024 * 1024 * 1024):N2} GB свободно из " +
                         $"{drives[i].TotalSize / (1024 * 1024 * 1024):N2} GB";
        }

        return options;
    }

    private static void ExploreFolders(string path)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);

        DirectoryInfo[] directories = dirInfo.GetDirectories();
        FileInfo[] files = dirInfo.GetFiles();

        string[] options = GetFolderFileOptions(directories, files);

        int selectedOption = ArrowMenu.ShowMenu(options);

        if (selectedOption == -1)
        {
            if (dirInfo.Parent != null)
            {
                ExploreFolders(dirInfo.Parent.FullName);
            }
            else
            {
                Main();
            }
        }
        else if (selectedOption < directories.Length)
        {
            ExploreFolders(directories[selectedOption].FullName);
        }
        else
        {
            string selectedFilePath = files[selectedOption - directories.Length].FullName;
            OpenFile(selectedFilePath);
        }
    }

    private static string[] GetFolderFileOptions(DirectoryInfo[] directories, FileInfo[] files)
    {
        string[] options = new string[directories.Length + files.Length];

        int i = 0;
        foreach (DirectoryInfo directory in directories)
        {
            options[i++] = $"{directory.Name,-45} {directory.CreationTime,-25:dd.MM.yyyy HH:mm}";
        }

        foreach (FileInfo file in files)
        {
            options[i++] = $"{file.Name,-45} {file.CreationTime,-25:dd.MM.yyyy HH:mm} {file.Extension,-10}";
        }

        return options;
    }

    private static void OpenFile(string filePath)
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };

            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Oшибка открытия {ex.Message}");
        }
    }
}
