using System;
using System.Collections.Generic;
using System.Linq;

namespace SDB
{
    public class Parser
    {
        public static Query Parse(IEnumerable<string> lines)
        {
            var query = Benumerator.AsLongString(lines);

            var save = query.Save();

            // Skipping first whitespace and failing if empty
            if (!save.MoveNext() || (save.Current == ' ' && !save.MoveNext()))
                throw new Exception("failed to parse. No Input");

            var from = From.Parse(save);
            var cols = Select.Parse(save).ToArray();

            return new Get
            {
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

    public class Select
    {
        public static IEnumerable<SelectColumn> Parse(Benumerator<Char> en)
        {
            // Next value MUST be "SELECT " 
            if (en.Current != 'S' || !en.MoveNext()
                || en.Current != 'E' || !en.MoveNext()
                || en.Current != 'L' || !en.MoveNext()
                || en.Current != 'E' || !en.MoveNext()
                || en.Current != 'C' || !en.MoveNext()
                || en.Current != 'T' || !en.MoveNext()
                || en.Current != ' ' || !en.MoveNext()
                )
                throw new Exception("failed to parse SELECT. Expecting SELECT keyword");


            // I am very lazily forcing the use of * for now
            foreach(var col in SelectColumn.Parse(en))
            {
                yield return col;
            }
        }
    }

    public class SelectColumn
    {
        public string columnName = "*";

        public static IEnumerable<SelectColumn> Parse(Benumerator<Char> en)
        {
            List<char> columnName;
            do
            {
                if (en.Current == ' ')
                    en.MoveNext();

                columnName = new List<char>();
                while (en.Current != ' ' && en.Current != ',')
                {
                    columnName.Add(en.Current);
                    if (!en.MoveNext())
                    {
                        yield return new SelectColumn
                        {
                            columnName = new String(columnName.ToArray()).Trim()
                        };
                        yield break;
                    }
                }

                string lastColumn = new String(columnName.ToArray()).Trim();
                if (! String.IsNullOrWhiteSpace(lastColumn))
                {
                    yield return new SelectColumn
                    {
                        columnName = lastColumn
                    };
                }

                if (en.Current == ' ')
                    if (!en.MoveNext())
                        yield break;
            }
            while (en.Current == ',' && en.MoveNext());
        }
    }

    public class From
    {
        public static string Parse(Benumerator<Char> en)
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
