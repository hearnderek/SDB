using SDB.Misc;
using System;
using System.Linq;

namespace SDB.Parts
{
    public class Get : Query
    {
        public From from;
        public SelectColumn[] columns;
        public Select select;

        public string Definition => from.Definition + " " + select.Definition;

        public static new Get Parse(Benumerator<char> en)
        {
            // Skipping first whitespace and failing if empty
            if (!en.MoveNext() || en.Current == ' ' && !en.MoveNext())
                throw new Exception("failed to parse. No Input");

            var from = From.Parse(en);
            var select = Select.Parse(en);

            if (from != null && select.columns.Length > 0)
            {
                return new Get
                {
                    from = from,
                    columns = select.columns,
                    select = select
                };
            }
            else
            {
                return null;
            }

        }

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
