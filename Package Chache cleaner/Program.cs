// See https://aka.ms/new-console-template for more information
using Microsoft.Win32;
using System.IO;

if (OperatingSystem.IsWindows())
{
    Console.WriteLine("Directory Cleanup Checker:");
    Console.WriteLine();
    using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
    var subkeynames = key?.GetSubKeyNames();
    var directoriesChache = Directory.GetDirectories(@"C:\ProgramData\Package Cache").ToList();
    // משיג תיקיות לא בשימוש
    foreach (var key1 in subkeynames)
    {
        using var key2 = key?.OpenSubKey(key1);
        if (key2 != null)
        {
            var source = (string?)key2.GetValue("InstallSource");
            if (source != null)
            {
                directoriesChache.RemoveAll(x => source.Contains(x));
                if (!Directory.Exists(source))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Non exsist Directory:");
                    Console.WriteLine(source);

                    Console.WriteLine();
                    Console.WriteLine("Application Info:");
                    Console.WriteLine();
                    foreach (var item in key2.GetValueNames())
                    {
                        var value = key2.GetValue(item);
                        if (value != null)
                        {
                            if (value is string && string.IsNullOrEmpty((string)value))
                                continue;
                            Console.WriteLine("{0}: {1}", item, value);
                        }
                    }
                    Console.WriteLine();
                }
            }
        }
    }
    Console.ResetColor();
    Console.WriteLine("Directories safe to clean:");
    foreach (var source in directoriesChache)
    {
        Console.WriteLine(source);
    }
    Console.WriteLine("Do You Wanna start clean?[Y/N]");
    var keyc = Console.ReadKey();
    if (keyc.Key == ConsoleKey.Y)
    {
        foreach (var source in directoriesChache)
        {
            Directory.Delete(source,true);
            Console.WriteLine("Directory Delete:");
            Console.WriteLine(source);
        }
    }
}

