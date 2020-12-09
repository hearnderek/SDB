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
}
