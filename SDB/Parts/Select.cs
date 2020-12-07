using SDB.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SDB.Parts
{
    public class Select
    {
        public SelectColumn[] columns;
        public string Definition => "SELECT " + string.Join(", ", columns.Select(c => c.columnName));

        public static Select Parse(Benumerator<char> en)
        {
            var columns = _Parse(en);
            return new Select
            {
                columns = columns.ToArray()
            };
        }

        private static IEnumerable<SelectColumn> _Parse(Benumerator<char> en)
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
            foreach (var col in SelectColumn.Parse(en, true))
            {
                yield return col;
            }
        }
    }
}
