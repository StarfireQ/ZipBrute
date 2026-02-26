using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZipBrute.Zip;

namespace ZipBrute.Worker
{
    internal class WorkerClass
    {
        public static void Run(string filePath, int threadsCount, int maxPassLenght, string secsListDir)
        {
            Stopwatch sw = Stopwatch.StartNew();

            byte[] zipBytes = File.ReadAllBytes(filePath);

            Console.WriteLine("Generating password dictionary...");
            List<string> passDict = AsciiGen.GenerateAsciiCombinations(1, maxPassLenght).ToList();

            if (zipBytes != null && zipBytes.Length != 0)
            {
                Console.WriteLine("Zip file processing...");
                List<ZipEntryMetadata> rc = ZipMetadataLoader.Load(zipBytes);
                ZipEntryMetadata? entry = rc.FirstOrDefault();

                if (entry != null && (entry.IsEncrypted || entry.AESKeySize > 0))
                {
                    bool success = ZipFilePass.TryPass(zipBytes, entry.Name!, passDict, threadsCount);
                    string log = success ? "SUCCESS" : "FAIL";
                    sw.Stop();

                    Console.WriteLine($"Status: {log} Duration: {(int)sw.Elapsed.TotalHours}h {sw.Elapsed.Minutes}m {sw.Elapsed.Seconds}s");
                }
                else
                {
                    Console.WriteLine("Zip file is not encrypted.");
                }
            }
            else
            {
                Console.WriteLine("Zip file is corrupted.");
            }
        }
    }
}
