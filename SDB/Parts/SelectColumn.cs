using System;
using System.Collections.Generic;

namespace SDB.Parts
{
    public class SelectColumn
    {
        public string columnName = "*";

        public static IEnumerable<SelectColumn> Parse(Benumerator<char> en, bool possiblyAtEnd = false)
        {
            do
            {
                Parser.SkipWhitespace(en);

                string columnName = Parser.ParseWord(en, possiblyAtEnd);
                if (!string.IsNullOrWhiteSpace(columnName))
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
}
