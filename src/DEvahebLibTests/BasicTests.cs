using System.IO;
using DEvahebLib;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    public class BasicTests
    {
        public static Variables variables = null;

        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            if (variables == null)
            {
                variables = Variables.FromCsv("variable_types.csv");
            }
        }

        [TestMethod]
        [DataRow(@"BasicTests\camera")]
        [DataRow(@"BasicTests\declare")]
        [DataRow(@"BasicTests\do")]
        [DataRow(@"BasicTests\dowait")]
        [DataRow(@"BasicTests\flush")]
        [DataRow(@"BasicTests\free")]
        [DataRow(@"BasicTests\get")]
        [DataRow(@"BasicTests\kill")]
        [DataRow(@"BasicTests\move")]
        [DataRow(@"BasicTests\play")]
        [DataRow(@"BasicTests\print")]
        [DataRow(@"BasicTests\random")]
        [DataRow(@"BasicTests\remove")]
        [DataRow(@"BasicTests\rotate")]
        [DataRow(@"BasicTests\run")]
        [DataRow(@"BasicTests\set")]
        [DataRow(@"BasicTests\signal")]
        [DataRow(@"BasicTests\sound")]
        [DataRow(@"BasicTests\tag")]
        [DataRow(@"BasicTests\use")]
        [DataRow(@"BasicTests\wait")]
        [DataRow(@"BasicTests\waitsignal")]
        public void TestFunctions(string filenameBase)
        {
            Assert.AreEqual(string.Empty, Helper.GenerateSourceFromIBIAndCompareOriginal(filenameBase, variables: variables));
        }


        [TestMethod]
        [DataRow(@"BasicTests\affect")]
        [DataRow(@"BasicTests\else")]
        [DataRow(@"BasicTests\if")]
        [DataRow(@"BasicTests\loop")]
        [DataRow(@"BasicTests\task")]
        public void TestBlocks(string filenameBase)
        {
            Assert.AreEqual(string.Empty, Helper.GenerateSourceFromIBIAndCompareOriginal(filenameBase, variables: variables));
        }
    }
}