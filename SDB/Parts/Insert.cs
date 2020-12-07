using System;
using System.Collections.Generic;
using System.Linq;

namespace SDB.Parts
{
    public class Insert : Query
    {
        public string tableName;
        public SelectColumn[] columns;
        public List<SelectColumn[]> rows;

        public static new Insert Parse(Benumerator<char> en)
        {
            if (!en.MoveNext() || en.Current == ' ' && !en.MoveNext())
                throw new Exception("failed to parse. No Input");


            if (!Parser.HasValue(en, "INSERT INTO "))
                throw new Exception("failed to parse INSERT. Expecting 'INSERT INTO '");


            var tableName = Parser.ParseWord(en);
            Parser.SkipWhitespace(en);

            if (!Parser.HasValue(en, "("))
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

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
