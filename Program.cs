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
                Console.WriteLine("Usege: ZipBrute.exe -f <fileName> -t <threadsCount> -l <maxPasswordLenght> -d <passwordDictionaryDirectory>");
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
                        return;
                    }
                }
                else if (args[i] == "-l" && i + 1 < args.Length)
                {
                    if (!int.TryParse(args[++i], out maxLength))
                    {
                        Console.WriteLine("Incorrect number of maximum password length.");
                        return;
                    }
                }
                if (args[i] == "-d" && i + 1 < args.Length)
                {
                    secList = args[++i];
                }
            }

            if (file == null)
            {
                Console.WriteLine("File name is missing -f <fileName>");
                return;
            }

            if (!File.Exists(file))
            {
                Console.WriteLine($"File {file} does not exists.");
                return;
            }

            PrintVersion();
            Console.WriteLine($"File: {file}");
            Console.WriteLine($"Threads: {threads}");
            Console.WriteLine($"MaxLength: {maxLength}");
            Console.WriteLine($"SecList: {secList}");

            Worker.WorkerClass.Run(file, threads, maxLength, secList);
        }

        static void PrintVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var name = assembly.GetName().Name ?? "Unknown";

            var version =
                assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion
                ?? assembly.GetName().Version?.ToString()
                ?? "0.0.0.0";

            Console.WriteLine($"{name} v{version}");
        }
    }
}
