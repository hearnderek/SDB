using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;


namespace TestSDB
{
    [TestClass]
    public class TestRun
    {

        [TestMethod]
        public void TestSelect()
        {

            var queries = new[] {
                "CREATE TABLE Taku ( one int, two int, three int )",
                "INSERT INTO Taku (one, two, three) VALUES ( 1, 2, 3 ), ( 1, 2, 3 )",
                "FROM Taku SELECT *" }
                .Select(q => new string[] { q })
                .Select(SDB.Parser.Parse);

            foreach(var query in queries)
            {
                query.Run();
            }
        }

        [TestMethod]
        public void TestCreate()
        {

            var queries = new[] {
                "CREATE TABLE Taku ( one int, two int, three int )"
            }
                .Select(q => new string[] { q })
                .Select(SDB.Parser.Parse);

            foreach (var query in queries)
            {
                query.Run();
            }
        }

        [TestMethod]
        public void TestInsert()
        {

            var queries = new[] {
                "CREATE TABLE Taku ( one int, two int, three int )",
                "INSERT INTO Taku (one, two, three) VALUES ( 1, 2, 3 ), ( 1, 2, 3 )"}
                .Select(q => new string[] { q })
                .Select(SDB.Parser.Parse);

            foreach (var query in queries)
            {
                query.Run();
            }
        }

        [TestMethod]
        public void TestDelete()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestDrop()
        {
            throw new NotImplementedException();
        }
    }

}
