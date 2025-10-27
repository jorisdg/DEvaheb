using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvahebLib;

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
    }
}