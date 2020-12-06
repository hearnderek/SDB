using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDB;
using System;

namespace TestSDB
{
    [TestClass]
    public class TestSelect
    {

        [TestMethod]
        public void ParseSelect()
        {
            var a = SDB.Parser.Parse( new[] { "CREATE TABLE Taku ( one int, two int, three int )" } );
            var b = SDB.Parser.Parse( new[] { "FROM Taku SELECT *" } ); 
        }
    }
}
