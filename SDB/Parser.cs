using SDB.Misc;
using System;
using System.Collections.Generic;

namespace SDB
{
    public class Parser
    {
        public static Query Parse(IEnumerable<string> lines)
        {
            var query = Benumerator.AsLongString(lines);

            var save = query.Save();
            return Query.Parse(save);
        }

        public static string ParseWord(Benumerator<char> en, bool possiblyAtEnd = false)
        {
            Parser.SkipWhitespace(en);

            List<char> work = new List<char>();
            do
            {
                work.Add(en.Current);
            }
            while (en.MoveNext() && en.Current != ' ' && en.Current != ',' && en.Current != '(' && en.Current != ')');


            return new String(work.ToArray());
        }

        public static void SkipWhitespace(Benumerator<char> en)
        {
            if (en.Current == ' ')
                en.MoveNext();
        }

        public static bool HasValue(Benumerator<char> en, string value)
        {
            foreach(Char c in value)
            {
                if (en.Current != c)
                    return false;
                en.MoveNext();
            }
            return true;
        }
    }

    public class Compressor
    {
        public char Largest => chars.Max;
        public char Smallest => chars.Min;
        public int Range => chars.Max - chars.Min;
        public List<string> input = new List<string>();
        public SortedSet<char> chars = new SortedSet<char>();


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
            foreach (string s in input)
            {
                chars.UnionWith(s);
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
    }
}
