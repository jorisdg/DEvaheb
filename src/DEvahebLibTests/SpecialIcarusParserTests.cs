using System;
using System.IO;
using System.Linq;
using DEvahebLib.Nodes;

namespace DEvahebLibTests
{
    [TestClass]
    public class SpecialIcarusParserTests
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        [TestMethod]
        [DataRow(@"IcarusParserTests\if_syntax.txt")]
        public void TestIfSyntaxes(string filename)
        {
            string source = File.ReadAllText(filename);
            var parser = new DEvahebLib.Parser.IcarusParser();

            var ifNodes = parser.Parse(source);

            Assert.AreEqual(4, ifNodes.Count, "Four valid nodes");
            Assert.AreEqual(4, ifNodes.Count(n => n.GetType() == typeof(If)), "Four valid IF syntaxes");


            Assert.IsTrue((ifNodes[3] as If).Expr1 is VectorValue, "Vector expected");
            Assert.IsTrue((ifNodes[3] as If).Expr2 is VectorValue, "Vector expected");


            Assert.IsTrue(((ifNodes[3] as If).Expr1 as VectorValue).Values[0] is FloatValue, "Vectors are made of float values");
            Assert.IsTrue(((ifNodes[3] as If).Expr1 as VectorValue).Values[1] is FloatValue, "Vectors are made of float values");
            Assert.IsTrue(((ifNodes[3] as If).Expr1 as VectorValue).Values[2] is FloatValue, "Vectors are made of float values");
                                                    
            Assert.IsTrue(((ifNodes[3] as If).Expr2 as VectorValue).Values[0] is FloatValue, "Vectors are made of float values");
            Assert.IsTrue(((ifNodes[3] as If).Expr2 as VectorValue).Values[1] is FloatValue, "Vectors are made of float values");
            Assert.IsTrue(((ifNodes[3] as If).Expr2 as VectorValue).Values[2] is FloatValue, "Vectors are made of float values");


            Assert.AreEqual(1.0f, (((ifNodes[3] as If).Expr1 as VectorValue).Values[0] as FloatValue).Value);
            Assert.AreEqual(2.0f, (((ifNodes[3] as If).Expr1 as VectorValue).Values[1] as FloatValue).Value);
            Assert.AreEqual(3.0f, (((ifNodes[3] as If).Expr1 as VectorValue).Values[2] as FloatValue).Value);
                                                                                                     
            Assert.AreEqual(4.0f, (((ifNodes[3] as If).Expr2 as VectorValue).Values[0] as FloatValue).Value);
            Assert.AreEqual(5.0f, (((ifNodes[3] as If).Expr2 as VectorValue).Values[1] as FloatValue).Value);
            Assert.AreEqual(6.0f, (((ifNodes[3] as If).Expr2 as VectorValue).Values[2] as FloatValue).Value);
        }

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
            Assert.AreEqual(1, nodesWith.Count(n => n.GetType() == typeof(Use)), "With conversion 1 use statement");

            var remNodes = nodesWith.Where(n => n.GetType() == typeof(Rem)).ToList();
            Assert.AreEqual(2, remNodes.Count, "With conversion 2 rem statements");

            var rem = (Rem)remNodes[0];
            Assert.AreEqual("This is a comment", ((StringValue)rem.Comment).String);

            rem = (Rem)remNodes[1];
            Assert.AreEqual("Another comment", ((StringValue)rem.Comment).String);
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
