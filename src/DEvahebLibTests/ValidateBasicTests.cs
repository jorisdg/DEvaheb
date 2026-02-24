using System;
using System.IO;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    public class ValidateBasicTests
    {
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
        [DataRow(@"BasicTests\affect")]
        [DataRow(@"BasicTests\else")]
        [DataRow(@"BasicTests\if")]
        [DataRow(@"BasicTests\loop")]
        [DataRow(@"BasicTests\task")]
        public void ValidateIBI(string filenameBase)
        {
            var ibiFile = filenameBase + ".IBI";
            var nodes = Helper.ReadIBI(ibiFile);

            var errors = ValidateNodes.Validate(nodes);

            if (errors.Count > 0)
            {
                Console.WriteLine(string.Join(Environment.NewLine, errors));
            }

            Assert.AreEqual(0, errors.Count, string.Join("; ", errors));
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
        [DataRow(@"BasicTests\affect")]
        [DataRow(@"BasicTests\else")]
        [DataRow(@"BasicTests\if")]
        [DataRow(@"BasicTests\loop")]
        [DataRow(@"BasicTests\task")]
        public void ValidateIcarus(string filenameBase)
        {
            var sourceFile = filenameBase + ".txt";
            var nodes = Helper.ReadSource(sourceFile);

            var errors = ValidateNodes.Validate(nodes);

            if (errors.Count > 0)
            {
                Console.WriteLine(string.Join(Environment.NewLine, errors));
            }

            Assert.AreEqual(0, errors.Count, string.Join("; ", errors));
        }
    }
}
