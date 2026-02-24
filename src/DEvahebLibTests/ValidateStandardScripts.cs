using System;
using System.Collections.Generic;
using System.IO;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    [TestCategory("LocalTest")]
    public class ValidateStandardScripts
    {
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

        public static IEnumerable<object[]> IcarusFiles
        {
            get
            {
                var files = Directory.EnumerateFiles(testFilesDirectory, "*.icarus", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    yield return new object[] { file };
                }
            }
        }

        [TestMethod]
        [DynamicData(nameof(IBIFiles))]
        public void ValidateIBI(string file)
        {
            var nodes = Helper.ReadIBI(file);

            var errors = ValidateNodes.Validate(nodes);

            if (errors.Count > 0)
            {
                Console.WriteLine(string.Join(Environment.NewLine, errors));
            }

            Assert.AreEqual(0, errors.Count, string.Join("; ", errors));
        }

        [TestMethod]
        [DynamicData(nameof(IcarusFiles))]
        public void ValidateIcarus(string file)
        {
            var sourceFile = file;
            List<DEvahebLib.Nodes.Node> nodes;
            try
            {
                nodes = Helper.ReadSource(sourceFile);
            }
            catch (Exception ex)
            {
                Assert.Inconclusive($"Source parse failed: {ex.Message}");
                return;
            }

            var errors = ValidateNodes.Validate(nodes);

            if (errors.Count > 0)
            {
                Console.WriteLine(string.Join(Environment.NewLine, errors));
            }

            Assert.AreEqual(0, errors.Count, string.Join("; ", errors));
        }
    }
}
