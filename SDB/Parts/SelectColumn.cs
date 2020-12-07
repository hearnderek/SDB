using System;
using System.Collections.Generic;

namespace SDB
{
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
}
