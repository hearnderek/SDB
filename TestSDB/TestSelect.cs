using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDB;
using System;

namespace TestSDB
{
    [TestClass]
    public class TestSelect
    {

        //[TestMethod]
        //public void ParseSelect()
        //{
        //    Assert.IsNotNull(SDB.Select.definition);
        //    // skip all start select block
        //    //
        //    SDB.Select.Parse(
        //        Helpers.AsLongString(new[] { "SELECT * FROM Taku" })
        //        );

        //    SDB.Select.Parse(
        //        Helpers.AsLongString(new[] { "  SELECT    *    FROM Taku" })
        //        );

        //    Assert.ThrowsException<Exception>(() =>
        //    {
        //        SDB.Select.Parse(
        //            Helpers.AsLongString(new[] { "FROM [Taku]  SELECT    [ID]    " })
        //            );
        //    });
        //}
    }
}
