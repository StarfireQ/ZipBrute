using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace ZipBrute.Zip
{
    internal class ZipFilePass
    {
        public static bool TryPass(byte[] zipBytes, string entryName, List<string> passDict, int threadsCount)
        {
            bool ret = false;

            CancellationTokenSource cts = new CancellationTokenSource();
            ParallelOptions options = new ParallelOptions
            {
                MaxDegreeOfParallelism = threadsCount,
                CancellationToken = cts.Token
            };

            long processed = 0;
            Timer timer = new Timer(_ =>
            {
                Console.WriteLine($"Processed: {Interlocked.Read(ref processed)} / {passDict.Count}");
            }, null, 0, 1000);

            try
            {
                Parallel.ForEach(passDict, options, (candidate, state) =>
                {
                    options.CancellationToken.ThrowIfCancellationRequested();

                    using MemoryStream ms = new MemoryStream(zipBytes, writable: false);
                    using ZipFile zipFile = new ZipFile(ms);
                    zipFile.Password = candidate;

                    ZipEntry? entry = zipFile.GetEntry(entryName);
                    if (entry == null) return;

                    try
                    {
                        using var stream = zipFile.GetInputStream(entry);
                        byte[] buffer = new byte[64];
                        int read = stream.Read(buffer, 0, buffer.Length);

                        Console.WriteLine($">>> Password is {candidate} <<<");

                        cts.Cancel();
                        state.Stop();
                        ret = true;
                    }
                    catch
                    {
                        Interlocked.Increment(ref processed);
                    }
                });
            }
            catch (OperationCanceledException)
            {
                // standard cancelation
            }

            return ret;
        }
    }
}
