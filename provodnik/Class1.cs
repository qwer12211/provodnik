using System;
public static class ArrowMenu
{
    public static int ShowMenu(string[] items)
    {
        int currentChoice = 0;

        while (true)
        {
            Console.Clear();
            PrintMenu(items, currentChoice);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                currentChoice = (currentChoice - 1 + items.Length) % items.Length;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                currentChoice = (currentChoice + 1) % items.Length;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                return currentChoice;
            }
            else if (keyInfo.Key == ConsoleKey.Escape)
            {
                return -1;
            }
        }
    }

    private static void PrintMenu(string[] items, int currentChoice)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (i == currentChoice)
            {
                Console.Write("-> ");
            }
            else
            {
                Console.Write("   ");
            }

            Console.WriteLine(items[i]);
        }
    }
}
