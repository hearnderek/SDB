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
            return Query.Parse(save);
        }

        
    }

    public class Query
    {
        public static Query Parse(Benumerator<char> en)
        {
            var s = en.Save();

            en.MoveNext();
            if (en.Current == ' ')
                en.MoveNext();

            if (en.Current == 'C')
            {
                return CreateTable.Parse(s);
            }
            else if (en.Current == 'F')
            {
                return Get.Parse(s);
            }
            else
            {
                throw new Exception("Unrecognized first command");
            }
        }

    }

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


    public class CreateTable : Query
    {
        public string tableName;
        public ColumnDefinition[] columns;

        public static new CreateTable Parse(Benumerator<Char> en)
        {
            if (!en.MoveNext() || (en.Current == ' ' && !en.MoveNext()))
                throw new Exception("failed to parse. No Input");

            if (en.Current != 'C' || !en.MoveNext()
                || en.Current != 'R' || !en.MoveNext()
                || en.Current != 'E' || !en.MoveNext()
                || en.Current != 'A' || !en.MoveNext()
                || en.Current != 'T' || !en.MoveNext()
                || en.Current != 'E' || !en.MoveNext()
                || en.Current != ' ' || !en.MoveNext()
                || en.Current != 'T' || !en.MoveNext()
                || en.Current != 'A' || !en.MoveNext()
                || en.Current != 'B' || !en.MoveNext()
                || en.Current != 'L' || !en.MoveNext()
                || en.Current != 'E' || !en.MoveNext()
                || en.Current != ' '
                )
                throw new Exception("failed to parse CREATE. Expected 'CREATE TABLE '");

            List<char> tableName = new List<char>();
            while (en.MoveNext() && en.Current != ' ')
            {
                tableName.Add(en.Current);
            }

            if (!en.MoveNext())
                throw new Exception("failed to parse CREATE. Unexpected end up input");

            if (en.Current != '(' || !en.MoveNext())
                throw new Exception("failed to parse CREATE. Ending ')' is missing");

            var colDefs = new List<ColumnDefinition>();
            do
            {
                colDefs.Add(ColumnDefinition.Parse(en));
            } while (en.Current == ',');


            if (en.Current == ' ')
                en.MoveNext();

            if (en.Current != ')' )
                throw new Exception("failed to parse CREATE. Ending ')' is missing");

            return new CreateTable
            {
                tableName = new String(tableName.ToArray()).Trim(),
                columns = colDefs.ToArray()
            };
        }
    }

    public class ColumnDefinition
    {
        public string type;
        public string name;
        public bool nullable = true;

        public static ColumnDefinition Parse(Benumerator<Char> en)
        {
            ColumnDefinition colDef = new ColumnDefinition();
            List<char> col;
            while (en.Current == ' ' || en.Current == ',')
                en.MoveNext();

            // get name
            col = new List<char>();
            while (en.Current != ' ' && en.Current != ',')
            {
                col.Add(en.Current);
                en.MoveNext();
            }
            colDef.name = new String(col.ToArray()).Trim();

            if (en.Current == ',')
                throw new Exception("Failed parsing Column Definition. Missing type");
            en.MoveNext();


            col = new List<char>();
            while (en.Current != ' ' && en.Current != ',')
            {
                col.Add(en.Current);
                en.MoveNext();
            }
            colDef.type = new String(col.ToArray()).Trim();

            return colDef;
        }
    }
}
