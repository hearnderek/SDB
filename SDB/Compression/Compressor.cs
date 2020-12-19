using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SDB.Compression
{
    public class Compressor
    {
        public char Largest => chars.Max;
        public char Smallest => chars.Min;
        public int Range => chars.Max - chars.Min;
        public int CharSize => chars.Count;

        public List<string> input = new List<string>();
        public SortedSet<char> chars = new SortedSet<char>();

        public Dictionary<char, char> encodeMapping = new Dictionary<char, char>();
        public Dictionary<char, char> decodeMapping = new Dictionary<char, char>();
        public void SetInput(string x)
        {
            SetInput(new List<string> { x });
        }

        public void SetInput(List<string> xs)
        {
            if (input.Count == 0)
                input = xs;
            else
                input.AddRange(xs);
        }

        public void Process()
        {
            chars.UnionWith(Environment.NewLine);
            foreach (string s in input)
            {
                chars.UnionWith(s);
            }

            SetMapping();
        }

        public void SetMapping()
        {
            encodeMapping.Clear();
            decodeMapping.Clear();
            char i = (char)0;
            foreach (char c in chars)
            {
                encodeMapping[c] = i;
                decodeMapping[i] = c;
                i++;
            }
        }

        public void WriteToFile(string path, int bufferSize = 10000)
        {
            using (var bw = System.IO.File.Create(path))
            {
                var stream = StreamCompress();
                foreach (byte[] batch in Batch(stream, bufferSize))
                {
                    bw.Write(batch, 0, batch.Length);
                };
            }
        }

        public static IEnumerable<byte[]> Batch(IEnumerable<byte> collection, int batchSize)
        {
            var bsm1 = batchSize - 1;
            int i = 0;
            byte[] nextbatch = new byte[batchSize];
            foreach (byte item in collection)
            {
                nextbatch[i++] = item;
                if (i == bsm1)
                {
                    yield return nextbatch;
                    nextbatch = new byte[batchSize];
                    i = 0;
                }
            }

            if (i > 0)
            {
                byte[] miniBatch = new byte[i];
                Array.Copy(nextbatch, miniBatch, i);
                yield return miniBatch;
            }
        }

        public byte[] FileHeaderSimple()
        {
            var magicNumber = BitConverter.GetBytes(0xDEADBEEF);
            List<byte> bs = new List<byte>(magicNumber);

            byte[] last = new byte[] { magicNumber[magicNumber.Length - 2], magicNumber[magicNumber.Length - 1] };
            foreach (char c in chars)
            {
                last = BitConverter.GetBytes(c);
                bs.AddRange(last);
            }
            bs.AddRange(last);

            return bs.ToArray();
        }

        public byte[] FileHeader()
        {
            var magicNumber = BitConverter.GetBytes(0xDEADBEEF);
            List<byte> bs = new List<byte>(magicNumber);

            bool skipping = false;
            byte[] tbb = new byte[] { magicNumber[magicNumber.Length - 2], magicNumber[magicNumber.Length - 1] };
            char? last = null;
            foreach (char c in chars)
            {
                // two bit buffer
                tbb = BitConverter.GetBytes(c);
                if (last.HasValue && c - last == 1)
                {
                    if (skipping)
                    {
                        // Write nothing!
                        // able to write one less char with no dataloss
                    }
                    else
                    {
                        // double up the last character to indicate a continuious jump
                        var count = bs.Count();
                        bs.Add(bs[count - 2]);
                        bs.Add(bs[count - 1]);

                        skipping = true;
                    }
                }
                else
                {
                    if (skipping)
                    {
                        // Last C should logically always have a value at this point
                        bs.AddRange(BitConverter.GetBytes(last.Value));
                    }
                    bs.AddRange(tbb);
                    skipping = false;
                }

                last = c;
            }

            // Final character should always be a tripplet
            if (skipping)
            {
                tbb = BitConverter.GetBytes(last.Value);
                bs.AddRange(tbb);
                bs.AddRange(tbb);
                bs.AddRange(tbb);
            }
            else
            {
                bs.AddRange(tbb);
                bs.AddRange(tbb);
            }

            return bs.ToArray();
        }

        public static Compressor FromHeaderSimple(byte[] headerBytes)
        {
            if (headerBytes.Length < 6)
            {
                // something is wrong
                return null;
            }
            if (headerBytes.Length == 6)
            {
                // empty
                // 0xDEADBEEF
                return new Compressor();
            }
            var chars = new SortedSet<char>();
            for (int i = 4; i + 1 < headerBytes.Length; i += 2)
            {
                if (!chars.Add(BitConverter.ToChar(headerBytes, i)))
                    // We repeat the last character as a sign of ending our header
                    break;
            }
            var comp = new Compressor { chars = chars };
            comp.SetMapping();
            return comp;

        }

        public static Compressor FromHeader(byte[] headerBytes)
        {
            if (headerBytes.Length < 6)
            {
                // something is wrong
                return null;
            }
            if (headerBytes.Length == 6)
            {
                // empty
                // 0xDEADBEEF
                return new Compressor();
            }
            var chars = new SortedSet<char>();
            char? doubled = null;
            for (int i = 4; i + 1 < headerBytes.Length; i += 2)
            {

                char c = BitConverter.ToChar(headerBytes, i);
                if (chars.Add(c))
                {
                    if (doubled.HasValue)
                    {
                        for (char nextC = (char)(doubled.Value + 1); nextC < chars.Max; nextC++)
                        {
                            chars.Add(nextC);
                        }
                    }
                    doubled = null;
                }
                else
                {
                    if (doubled.HasValue)
                    {
                        // We repeat the last character 3 times as a sign of ending our header
                        break;
                    }
                    else
                    {
                        // saw the same character twice
                        doubled = c;
                    }
                }
            }
            var comp = new Compressor { chars = chars };
            comp.SetMapping();
            return comp;

        }


        public IEnumerable<byte> StreamCompress()
        {
            foreach (byte b in FileHeader())
            {
                yield return b;
            }

            bool first = false;
            byte x = 0x00;
            var size = CharSize;
            foreach (string s in input)
            {
                foreach (char c in s + Environment.NewLine)
                {
                    var encoded = encodeMapping[c];
                    var bits = BitConverter.GetBytes(encoded);

                    if (size <= 16)
                    {
                        // compress to 1/4 size!

                        if (!first)
                        {
                            x |= bits[0];
                        }
                        else
                        {
                            byte t = (byte)(bits[0] << 4);
                            x |= t;
                            yield return x;
                            x = 0x00;
                        }

                        first = !first;
                    }

                    else if (size <= byte.MaxValue + 1)
                    {
                        // compress to 1/2 size!
                        yield return bits[0];
                    }
                    else
                    {
                        // no compression...
                        yield return bits[0];
                        yield return bits[1];
                    }
                }
            }

            // We MIGHT be half way through on the last byte
            if (first)
            {
                // This will cause problem...
                yield return x;
            }
        }

        public List<byte> Compress()
        {
            List<char> encoded = new List<char>();
            foreach (string s in input)
            {
                foreach (char c in s)
                {
                    encoded.Add(encodeMapping[c]);

                }

                // lazy method of adding a newline after every "line" of input
                foreach (char c in Environment.NewLine)
                {
                    encoded.Add(encodeMapping[c]);
                }
            }

            List<byte> bytes = new List<byte>(FileHeader());
            bool first = false;
            byte x = 0x00;
            var size = CharSize;


            foreach (char c in encoded)
            {
                var bits = BitConverter.GetBytes(c);

                if (size <= 16)
                {
                    // compress to 1/4 size!

                    if (!first)
                    {
                        x |= bits[0];
                    }
                    else
                    {
                        byte t = (byte)(bits[0] << 4);
                        x |= t;
                        bytes.Add(x);
                        x = 0x00;
                    }

                    first = !first;
                }

                else if (size <= byte.MaxValue + 1)
                {
                    // compress to 1/2 size!
                    bytes.Add(bits[0]);
                }
                else
                {
                    // no compression...
                    bytes.AddRange(bits);
                }

            }

            return bytes;
        }


        public int GetCompression()
        {
            return (int)Math.Pow(2, Math.Ceiling(Math.Log(CharSize, 2)));
        }

    }
}
