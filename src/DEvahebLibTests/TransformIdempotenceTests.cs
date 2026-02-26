using System.Collections.Generic;
using System.IO;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    public class TransformIdempotenceTests
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
        [DynamicData(nameof(IBIBasicTestsFiles))]
        public void TransformIsIdempotent_IBI(string filename)
        {
            var version = Helper.ReadIBIVersion(filename);
            var nodes = Helper.ReadIBI(filename);

            var bytesAfterFirstTransform = Helper.WriteIBI(nodes, version);

            TransformNodes.Transform(nodes);

            var bytesAfterSecondTransform = Helper.WriteIBI(nodes, version);

            int difference = Helper.FindIBIByteDifference(bytesAfterFirstTransform, bytesAfterSecondTransform);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }

        [TestMethod]
        [DynamicData(nameof(IBIBasicTestsFiles))]
        public void TransformIsIdempotent_Icarus(string filename)
        {
            var version = Helper.ReadIBIVersion(filename);
            var nodes = Helper.ReadSource(Path.ChangeExtension(filename, "txt"));

            var bytesAfterFirstTransform = Helper.WriteIBI(nodes, version);

            TransformNodes.Transform(nodes);

            var bytesAfterSecondTransform = Helper.WriteIBI(nodes, version);

            int difference = Helper.FindIBIByteDifference(bytesAfterFirstTransform, bytesAfterSecondTransform);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }
    }
}
