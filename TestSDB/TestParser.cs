using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace TestSDB
{
    [TestClass]
    public class TestParser
    {

        [TestMethod]
        public void TestSelect()
        {
            var a = SDB.Parser.Parse(
               new[] { "FROM Taku SELECT *" }
               );
            Assert.AreEqual("*", ((SDB.Get)a).columns[0].columnName);

            string[] equivalents = new[]
            {
                "FROM Taku SELECT one, two, three, ",
                "FROM Taku SELECT one ,two ,three",
                "FROM Taku SELECT one,two,three",
                "FROM Taku SELECT one, two, three",
                "FROM Taku SELECT one, two, three ",
                "FROM Taku SELECT one, two, three,",
            };

            foreach(var input in equivalents)
            {

                var b = SDB.Parser.Parse(
                    new[] { input }
                    );

                Assert.AreEqual(3, ((SDB.Get)b).columns.Length);
                Assert.AreEqual("one", ((SDB.Get)b).columns[0].columnName );
                Assert.AreEqual("two", ((SDB.Get)b).columns[1].columnName);
                Assert.AreEqual("three", ((SDB.Get)b).columns[2].columnName);
            }
        }

        [TestMethod]
        public void TestCreate()
        {
            var a = SDB.Parser.Parse(
               new[] { "CREATE TABLE Taku ( one int, two int, three int )" }
               );
            Assert.AreEqual("Taku", ((SDB.CreateTable)a).tableName);
            Assert.AreEqual("one", ((SDB.CreateTable)a).columns[0].name);
            Assert.AreEqual("int", ((SDB.CreateTable)a).columns[0].type);
            Assert.AreEqual("three", ((SDB.CreateTable)a).columns[2].name);
            Assert.AreEqual("int", ((SDB.CreateTable)a).columns[2].type);

            
        }

        [TestMethod]
        public void TestInsertValues()
        {
            var a = SDB.Parser.Parse(
               new[] { "INSERT INTO Taku (one, two, three) VALUES ( 1, 2, 3 ), ( 1, 2, 3 )" }
               );
            Assert.AreEqual("Taku", ((SDB.Insert)a).tableName);
            Assert.AreEqual("one", ((SDB.Insert)a).columns[0].columnName);
            Assert.AreEqual("two", ((SDB.Insert)a).columns[1].columnName);
            Assert.AreEqual("three", ((SDB.Insert)a).columns[2].columnName);


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


        [TestMethod]
        public void TestParseWord()
        {
            var en = SDB.Benumerator.AsLongString(new[] { "one ,two ,three" });
            en.MoveNext();

            Assert.AreEqual("one", SDB.Parser.ParseWord(en));
            SDB.Parser.SkipWhitespace(en);
            if (en.Current == ',')
                en.MoveNext();

            Assert.AreEqual("two", SDB.Parser.ParseWord(en));
            SDB.Parser.SkipWhitespace(en);
            if (en.Current == ',')
                en.MoveNext();

            Assert.AreEqual("three", SDB.Parser.ParseWord(en, true));



        }
    }

}
