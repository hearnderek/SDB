﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SDB
{
    public class Compressor
    {
        public char Largest => chars.Max;
        public char Smallest => chars.Min;
        public int Range => chars.Max - chars.Min;
        public int CharSize => chars.Count;

        public List<string> input = new List<string>();
        public SortedSet<char> chars = new SortedSet<char>();

        public Dictionary<Char, Char> encodeMapping = new Dictionary<char, char>();
        public Dictionary<Char, Char> decodeMapping = new Dictionary<char, char>();
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
            Char i = (Char)0;
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
                foreach (Byte[] batch in Compressor.Batch(stream, bufferSize))
                {
                    bw.Write(batch, 0, batch.Length);
                };
            }
        }

        public static List<UInt32> Exponents(BigInteger x, int exponent = 3)
        {
            var l = new List<UInt32>();
            do
            {
                UInt16 exp = (UInt16)BigInteger.Log(x, exponent);
                BigInteger best = BigInteger.Pow(exponent, exp); ;
                UInt32 mul = 1;
                for(int i = 0; i < exp; i++)
                {
                    UInt16 exp2 = (UInt16)(BigInteger.Log(x, exponent) - i);
                    var powed2 = BigInteger.Pow(exponent, exp2);
                    var bigMul2 = BigInteger.Divide(x, powed2);
                    UInt32 mul2 = UInt32.MaxValue;

                    if (bigMul2 <= UInt32.MaxValue)
                    {
                        mul2 = (UInt32)bigMul2;
                    }
                    else
                    {
                        break;
                    }
                    
                    
                    BigInteger next = powed2 * new BigInteger(mul2);


                    if (best < next)
                    {
                        exp = exp2;
                        mul = mul2;
                        best = next;
                    }
                }
                l.Add((UInt32)exp);
                l.Add(mul);
                x -= best;

            } while (x > UInt32.MaxValue);
            UInt32 remainder = (UInt32)x;
            l.Add(remainder);
            return l;
        }

        public static IEnumerable<Byte[]> Batch(IEnumerable<Byte> collection, int batchSize)
        {
            var bsm1 = batchSize - 1;
            int i = 0;
            Byte[] nextbatch = new byte[batchSize];
            foreach (Byte item in collection)
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
                Byte[] miniBatch = new byte[i];
                Array.Copy(nextbatch, miniBatch, i);
                yield return miniBatch;
            }
        }

        public Byte[] FileHeaderSimple()
        {
            var magicNumber = BitConverter.GetBytes(0xDEADBEEF);
            List<Byte> bs = new List<byte>(magicNumber);

            Byte[] last = new Byte[] { magicNumber[magicNumber.Length - 2], magicNumber[magicNumber.Length - 1] };
            foreach(Char c in chars)
            {
                last = BitConverter.GetBytes(c);
                bs.AddRange(last);
            }
            bs.AddRange(last);

            return bs.ToArray();
        }

        public Byte[] FileHeader()
        {
            var magicNumber = BitConverter.GetBytes(0xDEADBEEF);
            List<Byte> bs = new List<byte>(magicNumber);

            bool skipping = false;
            Byte[] tbb = new Byte[] { magicNumber[magicNumber.Length - 2], magicNumber[magicNumber.Length - 1] };
            Char? last = null;
            foreach (Char c in chars)
            {
                // two bit buffer
                tbb = BitConverter.GetBytes(c);
                if(last.HasValue && c - last == 1)
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

        public static Compressor FromHeaderSimple(Byte[] headerBytes)
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
            var chars = new SortedSet<Char>();
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

        public static Compressor FromHeader(Byte[] headerBytes)
        {
            if(headerBytes.Length < 6)
            {
                // something is wrong
                return null;
            }
            if(headerBytes.Length == 6)
            {
                // empty
                // 0xDEADBEEF
                return new Compressor();
            }
            var chars = new SortedSet<Char>();
            Char? doubled = null;
            for (int i = 4; i+1 < headerBytes.Length; i+=2)
            {

                Char c = BitConverter.ToChar(headerBytes, i);
                if (chars.Add(c))
                {
                    if (doubled.HasValue)
                    {
                        for (Char nextC = (Char)(doubled.Value + 1); nextC < chars.Max; nextC++)
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


        public IEnumerable<Byte> StreamCompress()
        {
            foreach(Byte b in FileHeader())
            {
                yield return b;
            }

            bool first = false;
            Byte x = 0x00;
            var size = this.CharSize;
            foreach (string s in input)
            {
                foreach (Char c in s + Environment.NewLine)
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
                            Byte t = (Byte)(bits[0] << 4);
                            x |= t;
                            yield return x;
                            x = 0x00;
                        }

                        first = !first;
                    }

                    else if (size <= Byte.MaxValue + 1)
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

        public List<Byte> Compress()
        {
            List<Char> encoded = new List<char>();
            foreach(string s in input)
            {
                foreach(Char c in s)
                {
                    encoded.Add(encodeMapping[c]);
                    
                }

                // lazy method of adding a newline after every "line" of input
                foreach (Char c in Environment.NewLine)
                {
                    encoded.Add(encodeMapping[c]);
                }
            }

            List<Byte> bytes = new List<byte>(FileHeader());
            bool first = false;
            Byte x = 0x00;
            var size = this.CharSize;


            foreach (Char c in encoded)
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
                        Byte t = (Byte)(bits[0] << 4);
                        x |= t;
                        bytes.Add(x);
                        x = 0x00;
                    }
                    
                    first = !first;
                }
                
                else if (size <= Byte.MaxValue + 1)
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
            return (int)Math.Pow(2,  Math.Ceiling(Math.Log(CharSize, 2)));
        }

    }
}
