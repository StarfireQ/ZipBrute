using System.Reflection;
using System.Threading;

namespace ZipBrute
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  ZipBrute.exe -f <file> -t <threads> -l <max-length> -d <dictionary-dir>");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("  -f <file>            Path to ZIP file");
                Console.WriteLine("  -t <threads>         Number of threads");
                Console.WriteLine("  -l <max-length>      Maximum password length");
                Console.WriteLine("  -d <dictionary-dir>  Directory with password dictionaries");
                Console.WriteLine("  -h                   Show this help message");
                Console.WriteLine("  -v                   Show application version");
                return;
            }

            string? file = null;
            int threads = Environment.ProcessorCount;
            int maxLength = 4;
            string? secList = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-f" && i + 1 < args.Length)
                {
                    file = args[++i];
                }
                else if (args[i] == "-t" && i + 1 < args.Length)
                {
                    if (!int.TryParse(args[++i], out threads))
                    {
                        Console.WriteLine("Incorrect number of threads count.");
                        PrintHelp();
                        Environment.ExitCode = 1;
                        return;
                    }
                }
                else if (args[i] == "-l" && i + 1 < args.Length)
                {
                    if (!int.TryParse(args[++i], out maxLength))
                    {
                        Console.WriteLine("Incorrect number of maximum password length.");
                        PrintHelp();
                        Environment.ExitCode = 1;
                        return;
                    }
                }
                else if (args[i] == "-d" && i + 1 < args.Length)
                {
                    secList = args[++i];
                }
                else if (args[i] == "-h")
                {
                    PrintHelp();
                    return;
                }
                else if (args[i] == "-v")
                {
                    PrintVersion();
                    return;
                }
            }

            if (file == null)
            {
                Console.WriteLine("File name is missing -f <fileName>");
                PrintHelp();
                Environment.ExitCode = 1;
                return;
            }

            if (!File.Exists(file))
            {
                Console.WriteLine($"File {file} does not exists.");
                PrintHelp();
                Environment.ExitCode = 1;
                return;
            }

            PrintVersion();
            Console.WriteLine($"File: {file}");
            Console.WriteLine($"Threads: {threads}");
            Console.WriteLine($"MaxLength: {maxLength}");
            Console.WriteLine($"SecList: {secList}");

            Worker.WorkerClass.Run(file, threads, maxLength, secList);
        }

        private static void PrintVersion()
        {
            Assembly? assembly = Assembly.GetExecutingAssembly();
            string? name = assembly.GetName().Name ?? "Unknown";
            string? version =
                assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion
                ?? assembly.GetName().Version?.ToString()
                ?? "0.0.0.0";

            Console.WriteLine($"{name} {version}");
        }

        private static void PrintHelp()
        {
            Console.WriteLine("ZipBrute - ZIP password brute-force utility");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  ZipBrute.exe -f <file> -t <threads> -l <max-length> -d <dictionary-dir>");
            Console.WriteLine("  ZipBrute.exe -h");
            Console.WriteLine("  ZipBrute.exe -v");
            Console.WriteLine();
            Console.WriteLine("Required options:");
            Console.WriteLine("  -f <file>            Path to target ZIP file");
            Console.WriteLine("  -t <threads>         Number of worker threads");
            Console.WriteLine("  -l <max-length>      Maximum password length");
            Console.WriteLine("  -d <dictionary-dir>  Directory containing password dictionaries");
            Console.WriteLine();
            Console.WriteLine("Other options:");
            Console.WriteLine("  -h           Show this help message");
            Console.WriteLine("  -v           Show application version");
        }
    }
}
