using System;
using System.Linq;

namespace SDB
{
    public class Get : Query
    {
        public string from;
        public SelectColumn[] columns;

        public static new Get Parse(Benumerator<char> en)
        {
            // Skipping first whitespace and failing if empty
            if (!en.MoveNext() || (en.Current == ' ' && !en.MoveNext()))
                throw new Exception("failed to parse. No Input");

            var from = From.Parse(en);
            var cols = Select.Parse(en).ToArray();

            if(from != null && cols.Length > 0)
            {
                return new Get
                {
                    from = from,
                    columns = cols
                };
            }
            else
            {
                return null;
            }

        }
    }
}
