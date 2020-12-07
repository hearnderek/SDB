using SDB.Misc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SDB.Parts
{
    public class CreateTable : Query
    {
        public string tableName;
        public ColumnDefinition[] columns;
        public string Definition => "CREATE TABLE " + tableName + " ( " + string.Join(", ", columns.Select(c => c.Definition)) + " )";

        public static new CreateTable Parse(Benumerator<char> en)
        {
            if (!en.MoveNext() || en.Current == ' ' && !en.MoveNext())
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

            if (en.Current != ')')
                throw new Exception("failed to parse CREATE. Ending ')' is missing");

            return new CreateTable
            {
                tableName = new string(tableName.ToArray()).Trim(),
                columns = colDefs.ToArray()
            };
        }

        

        public override void Run()
        {
            /* The plan */
            // Need to come up with a storage system
            //
            // Need to store tables
            //
            // Store everything in a single file?
            //
            // Starting with a single plain text file

            //using (FileStream f = File.OpenWrite(Config.fileLocation))
            //{
            //    f.
            //}


            throw new NotImplementedException();


        }
    }
}
