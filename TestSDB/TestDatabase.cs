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


    }


}