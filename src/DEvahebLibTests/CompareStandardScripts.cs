using System;
using System.Collections.Generic;
using System.IO;

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
        public void TestRoundTripIBI2Nodes2IBI_Binary(string file)
        {
            var originalBytes = File.ReadAllBytes(file);
            var version = Helper.ReadIBIVersion(file);
            var nodes = Helper.ReadIBI(file);
            var generatedBytes = Helper.GenerateIBI(nodes, version);

            int difference = Helper.FindIBIByteDifference(originalBytes, generatedBytes);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestRoundTripIBI2Nodes2IBI_AST(string file)
        {
            var nodes = Helper.ReadIBI(file);
            var generatedBytes = Helper.GenerateIBI(nodes);

            string differences = Helper.CompareASTs(Helper.ReadIBI(file), Helper.ReadIBI(generatedBytes));

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.IsTrue(condition: string.IsNullOrEmpty(differences), message: differences);
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestRoundTripIBI2Source2IBI_Binary(string file)
        {
            var originalBytes = File.ReadAllBytes(file);
            var version = Helper.ReadIBIVersion(file);
            var nodes = Helper.ReadIBI(file);
            
            var generatedSource = Helper.GenerateSource(Helper.VariableList, nodes);
            var newNodes = Helper.ReadSource(generatedSource);
            var generatedBytes = Helper.GenerateIBI(nodes, version);

            int difference = Helper.FindIBIByteDifference(originalBytes, generatedBytes);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestRoundTripIBI2Source2IBI_BareExpressions_Binary(string file)
        {
            var originalBytes = File.ReadAllBytes(file);
            var version = Helper.ReadIBIVersion(file);
            var nodes = Helper.ReadIBI(file);

            var generatedSource = Helper.GenerateSource(Helper.VariableList, nodes, DEvahebLib.Visitors.SourceCodeParity.BareExpressions);
            var newNodes = Helper.ReadSource(generatedSource);
            var generatedBytes = Helper.GenerateIBI(nodes, version);

            int difference = Helper.FindIBIByteDifference(originalBytes, generatedBytes);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestRoundTripIBI2Source2IBI_NoVariables_Binary(string file)
        {
            var originalBytes = File.ReadAllBytes(file);
            var version = Helper.ReadIBIVersion(file);
            var nodes = Helper.ReadIBI(file);

            var generatedSource = Helper.GenerateSource(variables: null, nodes);
            var newNodes = Helper.ReadSource(generatedSource);
            var generatedBytes = Helper.GenerateIBI(nodes, version);

            int difference = Helper.FindIBIByteDifference(originalBytes, generatedBytes);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestRoundTripIBI2Source2IBI_BareExpressionsNoVariables_Binary(string file)
        {
            var originalBytes = File.ReadAllBytes(file);
            var version = Helper.ReadIBIVersion(file);
            var nodes = Helper.ReadIBI(file);

            var generatedSource = Helper.GenerateSource(variables: null, nodes, DEvahebLib.Visitors.SourceCodeParity.BareExpressions);
            var newNodes = Helper.ReadSource(generatedSource);
            var generatedBytes = Helper.GenerateIBI(nodes, version);

            int difference = Helper.FindIBIByteDifference(originalBytes, generatedBytes);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void TestRoundTripIBI2Source2IBI_AST(string file)
        {
            var originalBytes = File.ReadAllBytes(file);
            var version = Helper.ReadIBIVersion(file);
            var nodes = Helper.ReadIBI(file);

            var generatedSource = Helper.GenerateSource(Helper.VariableList, nodes);
            var newNodes = Helper.ReadSource(generatedSource);
            var generatedBytes = Helper.GenerateIBI(nodes, version);

            string differences = Helper.CompareASTs(Helper.ReadIBI(file), Helper.ReadIBI(generatedBytes));

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.IsTrue(condition: string.IsNullOrEmpty(differences), message: differences);
        }
    }
}