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

            string generatedSource = Helper.GenerateSourceFromIBI(filenameBase + ".IBI", Helper.VariableList, DEvahebLib.Visitors.SourceCodeParity.BehavED);
            string differences = Helper.GetSourceFilesDifferences(filenameBase + ".txt", generatedSource);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
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

            string generatedSource = Helper.GenerateSourceFromIBI(filenameBase + ".IBI", Helper.VariableList, DEvahebLib.Visitors.SourceCodeParity.BehavED);
            string differences = Helper.GetSourceFilesDifferences(filenameBase + ".txt", generatedSource);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }

        [TestMethod]
        [DynamicData(nameof(IBIBasicTestsFiles))]
        public void TestRoundTripIBI(string filename)
        {
            var originalBytes = File.ReadAllBytes(filename);
            var ibiVersion = Helper.ReadIBIVersion(filename);
            var nodes = Helper.ReadIBI(filename);
            var generatedBytes = Helper.GenerateIBI(nodes, version: ibiVersion, jediAcademyFlag: true);

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
            var sourceNodes = Helper.ReadSourceFromFile(Path.ChangeExtension(filename, ".txt"));

            string differences = Helper.CompareASTs(ibiNodes, sourceNodes);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }
    }
}