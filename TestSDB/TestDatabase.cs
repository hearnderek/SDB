using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestSDB
{
    [TestClass]
    public class TestDatabase
    {
        [TestMethod]
        public void TestInit()
        {
            var db = new SDB.Database();
            db.Init();

            Assert.AreEqual(3, db._inMemory.Count);
        }

        [TestMethod]
        public void TestCreate()
        {
            var db = new SDB.Database();
            db.Init();

            db.CreateTable(new SDB.Parts.CreateTable
            {
                tableName = "MyFirstTable",
                columns = new SDB.Parts.ColumnDefinition[]
                {
                    new SDB.Parts.ColumnDefinition
                    {
                        name = "MyFirstColumn",
                        type = "int"
                    }
                }
            });

            Assert.AreEqual(6, db._inMemory.Count);

            db.CreateTable(new SDB.Parts.CreateTable
            {
                tableName = "MySecondTable",
                columns = new SDB.Parts.ColumnDefinition[]
                {
                    new SDB.Parts.ColumnDefinition
                    {
                        name = "MyFirstColumn",
                        type = "int"
                    }
                }
            });

            Assert.AreEqual(9, db._inMemory.Count);
        }
    }


}