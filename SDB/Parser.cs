using System;
using System.Collections.Generic;
using System.Linq;

namespace SDB
{
    public class Parser
    {
        public static Query Parse(IEnumerable<string> lines)
        {
            var query = Helpers.AsLongString(lines);


            // Skipping first whitespace and failing if empty
            if (!query.MoveNext() || (query.Current == ' ' && !query.MoveNext()))
                throw new Exception("failed to parse. No Input");

            var from = From.Parse(query);
            var cols = Select.Parse(query).ToArray();

            return new Get {
                from = from,
                columns = cols
            };
        }
    }

    public class Query
    {
        
        
    }

    public class Get : Query
    {
        public string from;
        public SelectColumn[] columns;


    }

    public class Helpers
    {
        public static IEnumerator<Char> AsLongString(IEnumerable<string> lines)
        {
            char? last = null;
            char cur;
            foreach (string line in lines) 
            { 
                foreach (char c in line)
                {
                    if (c == '\t')
                        cur = ' ';
                    else
                        cur = c;

                    if (cur == ' ' && last.HasValue && last.Value == ' ')
                        continue;

                    yield return cur;
                    last = cur;
                }

                // replacing line endings with standard spaces
                if(! (last.HasValue && last.Value == ' '))
                    yield return ' ';
            }
        }
    }

    public class Select
    {
        public static IEnumerable<SelectColumn> Parse(IEnumerator<Char> en)
        {
            // Next value MUST be "SELECT " 
            if(en.Current != 'S' || !en.MoveNext()
                || en.Current != 'E' || !en.MoveNext()
                || en.Current != 'L' || !en.MoveNext()
                || en.Current != 'E' || !en.MoveNext()
                || en.Current != 'C' || !en.MoveNext()
                || en.Current != 'T' || !en.MoveNext()
                || en.Current != ' ' || !en.MoveNext()
                )
                throw new Exception("failed to parse SELECT. Expecting SELECT keyword");


            // I am very lazily forcing the use of * for now
            if (en.Current != '*')
                throw new Exception("failed to parse SELECT. only accepts * in selection for now");
            yield return new SelectColumn();
        }
    }

    public class SelectColumn
    {
        public string columnName = "*";
    }

    public class From
    {
        public static string Parse(IEnumerator<Char> en)
        {

            if (en.Current != 'F' || !en.MoveNext()
                || en.Current != 'R' || !en.MoveNext()
                || en.Current != 'O' || !en.MoveNext()
                || en.Current != 'M' || !en.MoveNext()
                || en.Current != ' '
                )
                throw new Exception("failed to parse FROM. Expected FROM keyword");


            List<char> tableName = new List<char>();
            while (en.MoveNext() && en.Current != ' ')
            {
                tableName.Add(en.Current);
            }

            if (!en.MoveNext())
                throw new Exception("failed to parse FROM. Unexpected end up input");

            return new String(tableName.ToArray());
        }

    }

}
