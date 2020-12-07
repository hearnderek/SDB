using System;
using System.Collections.Generic;

namespace SDB.Parts
{
    public class From
    {
        public static string Parse(Benumerator<char> en)
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

            return new string(tableName.ToArray());
        }

    }
}
