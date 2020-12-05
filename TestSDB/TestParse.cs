using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TestSDB
{
    [TestClass]
    public class TestParse
    {

        [TestMethod]
        public void Parse()
        {
            // skip all start select block
            //
            var x = SDB.Parser.Parse(
                new[] { "FROM Taku SELECT *" }
                );
            
        }
    }
}
