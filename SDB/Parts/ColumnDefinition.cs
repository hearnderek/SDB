using SDB.Misc;
using System;
using System.Collections.Generic;

namespace SDB.Parts
{
    public class ColumnDefinition
    {
        public string type;
        public string name;
        public bool nullable = true;

        public static ColumnDefinition Parse(Benumerator<char> en)
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
            colDef.name = new string(col.ToArray()).Trim();

            if (en.Current == ',')
                throw new Exception("Failed parsing Column Definition. Missing type");
            en.MoveNext();


            col = new List<char>();
            while (en.Current != ' ' && en.Current != ',')
            {
                col.Add(en.Current);
                en.MoveNext();
            }
            colDef.type = new string(col.ToArray()).Trim();

            return colDef;
        }
    }
}
