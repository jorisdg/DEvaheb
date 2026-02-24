using System;
using System.Collections.Generic;
using System.IO;
using DEvahebLib.Nodes;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    public class TransformIdempotenceTests
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
        public void TransformIsIdempotent_IBI(string filenameBase)
        {
            var ibiFile = filenameBase + ".IBI";
            var version = Helper.ReadIBIVersion(ibiFile);
            var nodes = Helper.ReadIBI(ibiFile);

            var bytesAfterFirstTransform = Helper.WriteIBI(nodes, version);

            TransformNodes.Transform(nodes);

            var bytesAfterSecondTransform = Helper.WriteIBI(nodes, version);

            string differences = Helper.CompareIBIBytes(bytesAfterFirstTransform, bytesAfterSecondTransform);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences, "Second transform changed the IBI output");
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
        public void TransformIsIdempotent_Icarus(string filenameBase)
        {
            var ibiFile = filenameBase + ".IBI";
            var sourceFile = filenameBase + ".txt";
            var version = Helper.ReadIBIVersion(ibiFile);
            var nodes = Helper.ReadSource(sourceFile);

            var bytesAfterFirstTransform = Helper.WriteIBI(nodes, version);

            TransformNodes.Transform(nodes);

            var bytesAfterSecondTransform = Helper.WriteIBI(nodes, version);

            string differences = Helper.CompareIBIBytes(bytesAfterFirstTransform, bytesAfterSecondTransform);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences, "Second transform changed the Icarus output");
        }
    }
}
