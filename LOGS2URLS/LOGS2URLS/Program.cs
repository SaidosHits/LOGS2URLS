using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        banner();
        Console.Write(" [>] Enter Path root Folder: ");
        string input_path = Console.ReadLine();
        if (!Directory.Exists(input_path))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(" Sorry the root Path is not found !");
            Console.ReadKey();
            return;
        }
        var search = Directory.GetFiles(input_path, "Passwords.txt", SearchOption.AllDirectories);
        if (search.Length <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($@" Sorry the Files Passwords.txt is not found in this Directory {input_path}");
            Console.ReadKey();
            Environment.Exit(0);
        }
        List<string> Urls = new List<string>();
        List<string> usernames = new List<string>();
        List<string> passwords = new List<string>();

        foreach (string file in search)
        {
            using (StreamReader read_file = File.OpenText(file))
            {
                string line;
                string currentUrl = "";
                string currentUsername = "";
                while ((line = read_file.ReadLine()) != null)
                {
                    if (line.Contains("URL: https"))
                    {
                        currentUrl = line.Substring(5);
                    }
                    if (line.Contains("Username: "))
                    {
                        currentUsername = line.Substring(10);
                    }
                    if (line.Contains("Password: "))
                    {
                        string currentPassword = line.Substring(10);
                        // Only save if both username and password are found
                        if (!string.IsNullOrEmpty(currentUrl) && !string.IsNullOrEmpty(currentUsername) && !string.IsNullOrEmpty(currentPassword))
                        {
                            Urls.Add(currentUrl);
                            usernames.Add(currentUsername);
                            passwords.Add(currentPassword);
                        }
                        currentUrl = "";
                        currentUsername = "";
                        currentPassword = "";
                    }
                }
            }
        }

        // Combine lists and save to a single file
        string current_location = AppDomain.CurrentDomain.BaseDirectory;
        var current_date = DateTime.Now;
        string folder_name = string.Format("Result {0:[dd.mm.yyyy] [hh.mm.ss]}", current_date);
        Directory.CreateDirectory(folder_name);
        string save_file = Path.Combine(folder_name, "Result.txt");

        using (StreamWriter write = new StreamWriter(save_file))
        {
            foreach (var combinedData in Urls.Zip(usernames, (url, username) => new { Url = url, Username = username })
                                            .Zip(passwords, (data, password) => new { data.Url, data.Username, Password = password }))
            {
                write.WriteLine($"{combinedData.Url}:{combinedData.Username}:{combinedData.Password}");
            }
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("");
        Console.WriteLine(" [>] Done !");
        Console.ReadKey();
    }

    static void banner()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@"");
        Console.WriteLine(@"             _     ____  _____ ____  _____  ____  _     ____  _     ____ ");
        Console.WriteLine(@"             / \   /  _ \/  __// ___\/__ __\/  _ \/ \ /\/  __\/ \   / ___\");
        Console.WriteLine(@"             | |   | / \|| |  _|    \  / \  | / \|| | |||  \/|| |   |    \");
        Console.WriteLine(@"             | |_/\| \_/|| |_//\___ |  | |  | \_/|| \_/||    /| |_/\\___ |");
        Console.WriteLine(@"             \____/\____/\____\\____/  \_/  \____/\____/\_/\_\\____/\____/");
        Console.WriteLine(@"                                    @SaidosHits                            ");
        Console.WriteLine(@"");
        Console.WriteLine(@"");

    }
}
