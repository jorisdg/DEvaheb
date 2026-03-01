using System;
using System.IO;
using System.Linq;

namespace DEvahebLibTests
{
    [TestClass]
    public class SpecialIcarusParserTests
    {
        [TestMethod]
        [DataRow(@"IcarusParserTests\comments.txt")]
        public void TestIgnoreComments(string filename)
        {
            string source = File.ReadAllText(filename); ;
            var parser = new DEvahebLib.Parser.IcarusParser();

            var nodesWithout = parser.Parse(source, convertComments: false);

            Assert.AreEqual(1, nodesWithout.Count, "Without conversion, only the use statement should be parsed");
        }

        [TestMethod]
        [DataRow(@"IcarusParserTests\comments.txt")]
        public void TestConvertCommentsToRem(string filename)
        {
            string source = File.ReadAllText(filename);
            var parser = new DEvahebLib.Parser.IcarusParser();

            var nodesWith = parser.Parse(source, convertComments: true);

            Assert.AreEqual(3, nodesWith.Count, "With conversion 3 nodes total");
            Assert.AreEqual(1, nodesWith.Count(n => n.GetType() == typeof(DEvahebLib.Nodes.Use)), "With conversion 1 use statement");

            var remNodes = nodesWith.Where(n => n.GetType() == typeof(DEvahebLib.Nodes.Rem)).ToList();
            Assert.AreEqual(2, remNodes.Count, "With conversion 2 rem statements");
     
            var rem = (DEvahebLib.Nodes.Rem)remNodes[0];
            Assert.AreEqual("This is a comment", ((DEvahebLib.Nodes.StringValue)rem.Comment).String);

            rem = (DEvahebLib.Nodes.Rem)remNodes[1];
            Assert.AreEqual("Another comment", ((DEvahebLib.Nodes.StringValue)rem.Comment).String);
        }

        [TestMethod]
        [DataRow(@"IcarusParserTests\singleline")]
        public void TestSingleLineStatements(string filenameBase)
        {
            var singleLineNodes = Helper.ReadSourceFromFile(filenameBase + ".txt");
            var expectedNodes = Helper.ReadSourceFromFile(filenameBase + "_expected.txt");

            string differences = Helper.CompareASTs(expectedNodes, singleLineNodes);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }
    }
}
