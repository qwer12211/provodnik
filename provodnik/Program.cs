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
            string driveName = drives[i].Name;
            long availableSpace = drives[i].AvailableFreeSpace;
            long totalSize = drives[i].TotalSize;

            string formattedAvailableSpace = FormatSize(availableSpace);
            string formattedTotalSize = FormatSize(totalSize);

            options[i] = $"{driveName} {formattedAvailableSpace} свободно из {formattedTotalSize}";
        }

        return options;
    }

    private static string FormatSize(long sizeInBytes)
    {
        const double GB = 1024 * 1024 * 1024;

        double sizeInGB = (double)sizeInBytes / GB;
        string formattedSize = $"{sizeInGB:N2} GB";

        return formattedSize;
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
