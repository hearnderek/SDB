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
            else if (en.Current == 'I')
            {
                return Insert.Parse(s);
            }
            else
            {
                throw new Exception("Unrecognized first command");
            }
        }

    }

    public class Insert : Query
    {
        public string tableName;
        public SelectColumn[] columns;
        public List<SelectColumn[]> rows;

        public static new Insert Parse(Benumerator<char> en)
        {
            if (!en.MoveNext() || (en.Current == ' ' && !en.MoveNext()))
                throw new Exception("failed to parse. No Input");


            if (!Parser.HasValue(en, "INSERT INTO "))
                throw new Exception("failed to parse INSERT. Expecting 'INSERT INTO '");


            var tableName = Parser.ParseWord(en);
            Parser.SkipWhitespace(en);

            if (! Parser.HasValue(en, "("))
                throw new Exception("failed to parse INSERT. Expecting '('");

            var columns = SelectColumn.Parse(en).ToArray();
            Parser.SkipWhitespace(en);

            if (!Parser.HasValue(en, ")"))
                throw new Exception("failed to parse INSERT. Expecting ')'");
            Parser.SkipWhitespace(en);

            if (!Parser.HasValue(en, "VALUES"))
                throw new Exception("failed to parse INSERT. Expecting ')'");

            List<SelectColumn[]> rows = new List<SelectColumn[]>();
            do
            {
                Parser.SkipWhitespace(en);

                if (!Parser.HasValue(en, "("))
                    throw new Exception("failed to parse INSERT. Expecting ')'");

                Parser.SkipWhitespace(en);

                var values = SelectColumn.Parse(en).ToArray();
                if (values.Length != columns.Length)
                    throw new Exception("failed to parse INSERT. Number of values do not match number of columns");
                rows.Add(values);

                Parser.SkipWhitespace(en);

                if (!Parser.HasValue(en, ")"))
                    throw new Exception("failed to parse INSERT. Expecting ')'");

                Parser.SkipWhitespace(en);

            } while (Parser.HasValue(en, ","));

            return new Insert
            {
                tableName = tableName,
                columns = columns,
                rows = rows
            };
        }
    }

    public class DirectValue
    {
        public static new Get Parse(Benumerator<char> en)
        {
            throw new NotImplementedException();
        }
    }

    public class InsertValueRow
    {
        public static DirectValue[] values;

        public static new Get Parse(Benumerator<char> en)
        {
            throw new NotImplementedException();
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
            foreach(var col in SelectColumn.Parse(en, true))
            {
                yield return col;
            }
        }
    }

    public class SelectColumn
    {
        public string columnName = "*";

        public static IEnumerable<SelectColumn> Parse(Benumerator<Char> en, bool possiblyAtEnd = false)
        {
            do
            {
                Parser.SkipWhitespace(en);

                string columnName = Parser.ParseWord(en, possiblyAtEnd);
                if(!String.IsNullOrWhiteSpace(columnName))
                {
                    yield return new SelectColumn
                    {
                        columnName = columnName
                    };
                }

                Parser.SkipWhitespace(en);
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
