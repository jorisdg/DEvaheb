using System.IO;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        [DataRow(@"BasicTests\camera")] // Need function signature definitions... IBI float is INT text
        [DataRow(@"BasicTests\declare")]
        [DataRow(@"BasicTests\do")]
        [DataRow(@"BasicTests\dowait")] // TODO aliases Visitor
        [DataRow(@"BasicTests\flush")]
        [DataRow(@"BasicTests\free")]
        [DataRow(@"BasicTests\get")]
        [DataRow(@"BasicTests\kill")]
        [DataRow(@"BasicTests\move")]
        [DataRow(@"BasicTests\play")]
        [DataRow(@"BasicTests\print")]
        [DataRow(@"BasicTests\remove")]
        [DataRow(@"BasicTests\rotate")]
        [DataRow(@"BasicTests\run")]
        [DataRow(@"BasicTests\set")] // Need enum definition files, especially for SET_TYPES
        [DataRow(@"BasicTests\signal")]
        [DataRow(@"BasicTests\sound")]
        [DataRow(@"BasicTests\tag")]
        [DataRow(@"BasicTests\use")]
        [DataRow(@"BasicTests\wait")]
        [DataRow(@"BasicTests\waitsignal")]
        public void TestFunctions(string filenameBase)
        {
            Helper.GenerateSourceFromIBIAndCompareOriginal(filenameBase);
        }


        [TestMethod]
        [DataRow(@"BasicTests\affect")]
        [DataRow(@"BasicTests\else")]
        [DataRow(@"BasicTests\if")]
        [DataRow(@"BasicTests\loop")] // Need function signature definitions... IBI float is INT text
        [DataRow(@"BasicTests\task")]
        public void TestBlocks(string filenameBase)
        {
            Helper.GenerateSourceFromIBIAndCompareOriginal(filenameBase);
        }
    }
}