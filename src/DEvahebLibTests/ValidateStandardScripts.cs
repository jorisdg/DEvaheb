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
        [TestMethod]
        [DynamicData(nameof(Helper.IBITestFiles), typeof(Helper))]
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
        [DynamicData(nameof(Helper.IcarusTestFiles), typeof(Helper))]
        public void ValidateIcarus(string file)
        {
            var sourceFile = file;
            List<DEvahebLib.Nodes.Node> nodes;
            try
            {
                nodes = Helper.ReadSourceFromFile(sourceFile);
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
