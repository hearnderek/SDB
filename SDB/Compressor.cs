using System;
using System.Collections.Generic;

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

        public List<Compressor> Mitosis()
        {
            var range = Range;
            if (range > byte.MaxValue * 2)
            {
                List<char> cs = new List<char>();
                List<int> distance = new List<int>();
                char last = chars.Min;
                int largestDist = 0;
                int largestDiffIndex = 0;
                int i = 0;
                foreach (char c in chars)
                {
                    cs.Add(c);
                    var dist = c - last;
                    distance.Add(dist);
                    if(dist > largestDist)
                    {
                        largestDist = dist;
                        largestDiffIndex = i;
                    }
                    last = c;
                    i++;
                }

                if(largestDist > range/4)
                {
                    var small = new Compressor();
                    small.SetInput(new String(cs.GetRange(0, largestDiffIndex).ToArray()));
                    small.Process();
                    var smalls = small.Mitosis();
                    
                    var big = new Compressor();
                    big.SetInput(new String(cs.GetRange(largestDiffIndex, cs.Count - largestDiffIndex).ToArray()));
                    big.Process();
                    var bigs = big.Mitosis();

                    smalls.AddRange(bigs);
                    return smalls;
                }
                else
                {
                    return new List<Compressor>
                    {
                        this
                    };
                }

                var x = cs[cs.Count / 2];
            }
            else
            {
                return new List<Compressor>
                {
                    this
                };
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

            List<Byte> bytes = new List<byte>();
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
