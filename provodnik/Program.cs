using System;
using System.IO;
using System.Diagnostics;

public static class Explorer
{
    private static void Main()
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();
        string[] options = new string[allDrives.Length];

        for (int i = 0; i < allDrives.Length; i++)
        {
            options[i] = $"{allDrives[i].Name} " +
                         $"{allDrives[i].AvailableFreeSpace / (1024 * 1024 * 1024):N2} ГБ свободно из " +
                         $"{allDrives[i].TotalSize / (1024 * 1024 * 1024):N2} ГБ";
        }

        int selectedDrive = ArrowMenu.ShowMenu(options);
        string selectedDriveName = allDrives[selectedDrive].Name;

        folders(selectedDriveName);
    }

    private static void folders(string path)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(path);

        DirectoryInfo[] directories = dirInfo.GetDirectories();
        FileInfo[] files = dirInfo.GetFiles();

        string[] options = new string[directories.Length + files.Length];

        int i = 0;
        foreach (DirectoryInfo directory in directories)
        {
            options[i++] = String.Format("{0,-45} {1,25}", directory.Name, directory.CreationTime.ToString("dd.MM.yyyy HH:mm"));
        }

        foreach (FileInfo file in files)
        {
            options[i++] = String.Format("{0,-45} {1,25} {2,10}", file.Name, file.CreationTime.ToString("dd.MM.yyyy HH:mm"), file.Extension);
        }

        int selectedOption = ArrowMenu.ShowMenu(options);

        if (selectedOption == -1)
        {
            if (dirInfo.Parent != null)
            {
                folders(dirInfo.Parent.FullName);
            }
            else
            {
                Main();
            }
        }
        else if (selectedOption < directories.Length)
        {
            folders(directories[selectedOption].FullName);
        }
        else
        {
            string selectedFilePath = files[selectedOption - directories.Length].FullName;
            openFiles(selectedFilePath);
        }
    }
    private static void openFiles(string filePath)
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
            Console.WriteLine($"Что-то вы не то открыли: {ex.Message}");
        }
    }
}