using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipBrute.Worker
{
    internal class AsciiGen
    {
        public static IEnumerable<string> GenerateAsciiCombinations(int minLength, int maxLength)
        {
            char[] chars = Enumerable.Range(32, 95)
                                     .Select(i => (char)i)
                                     .ToArray();

            for (int length = minLength; length <= maxLength; length++)
            {
                int[] indices = new int[length];

                while (true)
                {
                    yield return new string(indices.Select(i => chars[i]).ToArray());

                    int position = length - 1;
                    while (position >= 0)
                    {
                        indices[position]++;
                        if (indices[position] < chars.Length)
                            break;

                        indices[position] = 0;
                        position--;
                    }

                    if (position < 0)
                        break;
                }
            }
        }
    }
}
