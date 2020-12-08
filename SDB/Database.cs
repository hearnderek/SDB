using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SDB
{
    public class Database
    {
        public List<string> _inMemory = new List<string>();

        public void Init()
        {
            _inMemory.Add("SDB");

            foreach(var script in initScripts)
            {
                _inMemory.Add(script);
            }

        }

        private static Parts.CreateTable tables = new Parts.CreateTable
        {
            tableName = "|tables|",
            columns = new Parts.ColumnDefinition[]
            {
                new Parts.ColumnDefinition
                {
                    name = "id",
                    type = "int"
                },
                new Parts.ColumnDefinition
                {
                    name = "tablename",
                    type = "string"
                },
                new Parts.ColumnDefinition
                {
                    name = "location",
                    type = "int"
                }
            }
        };

        private static string[] initScripts = new[] { 
            // I will need to update this jump location to a bigger datastruct at some point
            tables.Definition,
            ""
        };


        public void CreateTable(Parts.CreateTable table)
        {
            int insertAt = -1;
            if (_inMemory[2] == "")
            {
                // Empty Database
                _inMemory[2] = "1, " + table.tableName + ", 5";
                _inMemory.Add("");
                insertAt = 4;
            }
            else
            {
                insertAt = _inMemory.Count();
            }

            if (_inMemory.Count == insertAt)
            {
                _inMemory.Add(table.Definition);
            }
            else
            {
                _inMemory[insertAt] = table.Definition;
            }

            // Add buffer row
            if (_inMemory.Count == insertAt+1)
            {
                _inMemory.Add("");
            }
            else
            {
                _inMemory[insertAt+1] = "";
            }
        }

        public void Insert(Parts.Insert insert)
        {
            throw new NotImplementedException();
        }

        public int Count(string table)
        {
            throw new NotImplementedException();
        }

        public int TableLocation(string table)
        {
            throw new NotImplementedException();
        }

        public List<Object> Select(Parts.Get get)
        {
            var table = get.from.table;

            // finding location of table in memory
            for(int i = 2; i < _inMemory.Count && _inMemory[i] != ""; i++)
            {

            }

            throw new NotImplementedException();
        }
    }
}
