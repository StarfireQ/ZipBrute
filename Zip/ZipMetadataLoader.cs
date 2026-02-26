using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipBrute.Zip
{
    public class ZipEntryMetadata
    {
        public string? Name { get; set; }
        public long Size { get; set; }
        public long CompressedSize { get; set; }
        public int CompressionMethod { get; set; }
        public long Crc { get; set; }
        public bool IsEncrypted { get; set; }
        public int AESKeySize { get; set; }
    }

    internal class ZipMetadataLoader
    {
        public static List<ZipEntryMetadata> Load(string path)
        {
            var result = new List<ZipEntryMetadata>();

            using (FileStream fs = File.OpenRead(path))
            using (ZipFile zipFile = new ZipFile(fs))
            {
                foreach (ZipEntry entry in zipFile)
                {
                    if (!entry.IsFile)
                        continue;

                    result.Add(new ZipEntryMetadata
                    {
                        Name = entry.Name,
                        Size = entry.Size,
                        CompressedSize = entry.CompressedSize,
                        CompressionMethod = (int)entry.CompressionMethod,
                        Crc = entry.Crc,
                        IsEncrypted = entry.IsCrypted,
                        AESKeySize = entry.AESKeySize
                    });
                }
            }

            return result;
        }

        public static List<ZipEntryMetadata> Load(byte[] zipBytes)
        {
            var result = new List<ZipEntryMetadata>();

            using MemoryStream ms = new MemoryStream(zipBytes, writable: false);
            using (ZipFile zipFile = new ZipFile(ms))
            {
                foreach (ZipEntry entry in zipFile)
                {
                    if (!entry.IsFile)
                        continue;

                    result.Add(new ZipEntryMetadata
                    {
                        Name = entry.Name,
                        Size = entry.Size,
                        CompressedSize = entry.CompressedSize,
                        CompressionMethod = (int)entry.CompressionMethod,
                        Crc = entry.Crc,
                        IsEncrypted = entry.IsCrypted,
                        AESKeySize = entry.AESKeySize
                    });
                }
            }

            return result;
        }
    }
}
