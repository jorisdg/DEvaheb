using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DEvahebLibTests
{
    [TestClass]
    public class BasicTests
    {
        public static IEnumerable<object[]> IBIBasicTestsFiles
        {
            get
            {
                var files = Directory.EnumerateFiles(@".\BasicTests", "*.IBI", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    yield return new object[] { file };
                }
            }
        }

        public static IEnumerable<object[]> IBIBasicTestsFilesLanguages
        {
            get
            {
                var files = Directory.EnumerateFiles(@".\BasicTests", "*.IBI", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    yield return new object[] { file, "en-US" };
                    yield return new object[] { file, "nl-BE" };
                }
            }
        }

        [TestMethod]
        [DataRow(@"BasicTests\camera", "en-US")]
        [DataRow(@"BasicTests\declare", "en-US")]
        [DataRow(@"BasicTests\do", "en-US")]
        [DataRow(@"BasicTests\dowait", "en-US")]
        [DataRow(@"BasicTests\flush", "en-US")]
        [DataRow(@"BasicTests\free", "en-US")]
        [DataRow(@"BasicTests\get", "en-US")]
        [DataRow(@"BasicTests\kill", "en-US")]
        [DataRow(@"BasicTests\move", "en-US")]
        [DataRow(@"BasicTests\play", "en-US")]
        [DataRow(@"BasicTests\print", "en-US")]
        [DataRow(@"BasicTests\random", "en-US")]
        [DataRow(@"BasicTests\remove", "en-US")]
        [DataRow(@"BasicTests\rotate", "en-US")]
        [DataRow(@"BasicTests\run", "en-US")]
        [DataRow(@"BasicTests\set", "en-US")]
        [DataRow(@"BasicTests\signal", "en-US")]
        [DataRow(@"BasicTests\sound", "en-US")]
        [DataRow(@"BasicTests\tag", "en-US")]
        [DataRow(@"BasicTests\use", "en-US")]
        [DataRow(@"BasicTests\wait", "en-US")]
        [DataRow(@"BasicTests\waitsignal", "en-US")]

        [DataRow(@"BasicTests\camera", "nl-BE")]
        [DataRow(@"BasicTests\declare", "nl-BE")]
        [DataRow(@"BasicTests\do", "nl-BE")]
        [DataRow(@"BasicTests\dowait", "nl-BE")]
        [DataRow(@"BasicTests\flush", "nl-BE")]
        [DataRow(@"BasicTests\free", "nl-BE")]
        [DataRow(@"BasicTests\get", "nl-BE")]
        [DataRow(@"BasicTests\kill", "nl-BE")]
        [DataRow(@"BasicTests\move", "nl-BE")]
        [DataRow(@"BasicTests\play", "nl-BE")]
        [DataRow(@"BasicTests\print", "nl-BE")]
        [DataRow(@"BasicTests\random", "nl-BE")]
        [DataRow(@"BasicTests\remove", "nl-BE")]
        [DataRow(@"BasicTests\rotate", "nl-BE")]
        [DataRow(@"BasicTests\run", "nl-BE")]
        [DataRow(@"BasicTests\set", "nl-BE")]
        [DataRow(@"BasicTests\signal", "nl-BE")]
        [DataRow(@"BasicTests\sound", "nl-BE")]
        [DataRow(@"BasicTests\tag", "nl-BE")]
        [DataRow(@"BasicTests\use", "nl-BE")]
        [DataRow(@"BasicTests\wait", "nl-BE")]
        [DataRow(@"BasicTests\waitsignal", "nl-BE")]
        public void TestFunctions(string filenameBase, string cultureName)
        {
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(cultureName);
            Assert.AreEqual(string.Empty, Helper.GenerateSourceFromIBIAndCompareOriginal(filenameBase, variables: Helper.VariableList, parity: DEvahebLib.Visitors.SourceCodeParity.BehavED));
        }


        [TestMethod]
        [DataRow(@"BasicTests\affect", "en-US")]
        [DataRow(@"BasicTests\else", "en-US")]
        [DataRow(@"BasicTests\if", "en-US")]
        [DataRow(@"BasicTests\loop", "en-US")]
        [DataRow(@"BasicTests\task", "en-US")]

        [DataRow(@"BasicTests\affect", "nl-BE")]
        [DataRow(@"BasicTests\else", "nl-BE")]
        [DataRow(@"BasicTests\if", "nl-BE")]
        [DataRow(@"BasicTests\loop", "nl-BE")]
        [DataRow(@"BasicTests\task", "nl-BE")]
        public void TestBlocks(string filenameBase, string cultureName)
        {
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(cultureName);
            Assert.AreEqual(string.Empty, Helper.GenerateSourceFromIBIAndCompareOriginal(filenameBase, variables: Helper.VariableList, parity: DEvahebLib.Visitors.SourceCodeParity.BehavED));
        }

        [TestMethod]
        [DynamicData(nameof(IBIBasicTestsFiles))]
        public void TestRoundTripIBI(string filename)
        {
            var originalBytes = File.ReadAllBytes(filename);
            var nodes = Helper.ReadIBI(filename);
            var generatedBytes = Helper.WriteIBI(nodes);

            int difference = Helper.FindIBIByteDifference(originalBytes, generatedBytes);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }

        [TestMethod]
        [DynamicData(nameof(IBIBasicTestsFilesLanguages))]
        public void TestSourceParseMatchesIBI(string filename, string cultureName)
        {
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(cultureName);

            var ibiNodes = Helper.ReadIBI(filename);
            var sourceNodes = Helper.ReadSource(Path.ChangeExtension(filename, ".txt"));

            string differences = Helper.CompareASTs(ibiNodes, sourceNodes);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }

        [TestMethod]
        public void TestConvertCommentsToRem()
        {
            string source = "// This is a comment\nuse ( \"hello\" );\n// Another comment\n";
            var parser = new DEvahebLib.Parser.IcarusParser();

            var nodesWithout = parser.Parse(source, convertComments: false);
            Assert.AreEqual(1, nodesWithout.Count, "Without conversion, only the use statement should be parsed");

            var nodesWith = parser.Parse(source, convertComments: true);
            Assert.AreEqual(3, nodesWith.Count, "With conversion, 2 rem nodes + 1 use statement");

            Assert.IsInstanceOfType(nodesWith[0], typeof(DEvahebLib.Nodes.Rem));
            var rem1 = (DEvahebLib.Nodes.Rem)nodesWith[0];
            Assert.AreEqual("rem", rem1.Name);
            Assert.AreEqual("This is a comment", ((DEvahebLib.Nodes.StringValue)rem1.Comment).String);

            Assert.IsInstanceOfType(nodesWith[2], typeof(DEvahebLib.Nodes.Rem));
            var rem2 = (DEvahebLib.Nodes.Rem)nodesWith[2];
            Assert.AreEqual("rem", rem2.Name);
            Assert.AreEqual("Another comment", ((DEvahebLib.Nodes.StringValue)rem2.Comment).String);
        }

        [TestMethod]
        [DataRow(@"BasicTests\singleline")]
        public void TestSingleLineStatements(string filenameBase)
        {
            var singleLineNodes = Helper.ReadSource(filenameBase + ".txt");
            var expectedNodes = Helper.ReadSource(filenameBase + "_expected.txt");

            string differences = Helper.CompareASTs(expectedNodes, singleLineNodes);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }
    }
}