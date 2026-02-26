using System;
using System.Collections.Generic;
using System.IO;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    [TestCategory("LocalTest")]
    public class CompareStandardScripts
    {
        /// <summary>
        /// <RunSettings>
        ///   <TestRunParameters>
        ///     <Parameter name = "TestFilesDirectory" value="C:\temp\JEDI_Academy_SDK\Tools\JAscripts" />
        ///   </TestRunParameters>
        /// </RunSettings>
        /// </summary>
        public static string testFilesDirectory = @"C:\temp\JEDI_Academy_SDK\Tools\JAscripts";

        [ClassInitialize]
        public static void TestClassInitialize(TestContext context)
        {
            string testFilesDirFromContext = context.Properties["TestFilesDirectory"]?.ToString();
            if (!string.IsNullOrEmpty(testFilesDirFromContext))
            {
                testFilesDirectory = testFilesDirFromContext;
            }
        }

        public static IEnumerable<object[]> IBIFiles
        {
            get
            {
                var files = Directory.EnumerateFiles(testFilesDirectory, "*.IBI", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    yield return new object[] { file };
                }
            }
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestFilesWithInlineComments(string file)
        {
            string differences = Helper.GenerateSourceFromIBIAndCompareOriginal(Path.ChangeExtension(file, null), BasicTests.VariableList, originalExtension: ".icarus", parity: DEvahebLib.Visitors.SourceCodeParity.BehavED, ignoreSetTypes: false);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(expected: string.Empty, actual: differences);
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestFilesWithoutInlineComments(string file)
        {
            string differences = Helper.GenerateSourceFromIBIAndCompareOriginal(Path.ChangeExtension(file, null), BasicTests.VariableList, originalExtension: ".icarus", parity: DEvahebLib.Visitors.SourceCodeParity.BehavED, ignoreSetTypes: true);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.IsTrue(condition: string.IsNullOrEmpty(differences), message: differences);
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestFilesWithoutBehavedCompatibility(string file)
        {
            string differences = Helper.GenerateSourceFromIBIAndCompareOriginal(Path.ChangeExtension(file, null), BasicTests.VariableList, originalExtension: ".icarus", parity: DEvahebLib.Visitors.SourceCodeParity.BareExpressions, ignoreSetTypes: true);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.IsTrue(condition: string.IsNullOrEmpty(differences), message: differences);
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestRoundTripIBIBinary(string file)
        {
            var originalBytes = File.ReadAllBytes(file);
            var version = Helper.ReadIBIVersion(file);
            var nodes = Helper.ReadIBI(file);
            var generatedBytes = Helper.WriteIBI(nodes, version);
            string differences = Helper.CompareIBIBytes(originalBytes, generatedBytes);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestRoundTripIBIAST(string file)
        {
            var originalBytes = File.ReadAllBytes(file);
            var version = Helper.ReadIBIVersion(file);
            var nodes = Helper.ReadIBI(file);
            var generatedBytes = Helper.WriteIBI(nodes, version);
            string differences = Helper.CompareASTs(Helper.ReadIBI(file), Helper.ReadIBI(generatedBytes));

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestSourceParseMatchesIBI(string file)
        {
            var sourceFile = Path.ChangeExtension(file, ".icarus");
            if (!File.Exists(sourceFile))
            {
                Assert.Inconclusive($"Source file not found: {sourceFile}");
                return;
            }

            var ibiNodes = Helper.ReadIBI(file);
            var sourceNodes = Helper.ReadSource(sourceFile, includeRem: false);

            string differences = Helper.CompareASTs(ibiNodes, sourceNodes, stopOnFirst: false);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }
    }
}