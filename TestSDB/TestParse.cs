using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TestSDB
{
    [TestClass]
    public class TestParse
    {

        [TestMethod]
        public void SelectParse()
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
        public void CreateParse()
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
    }
}
