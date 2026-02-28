using System.Collections.Generic;
using System.IO;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    public class TransformIdempotenceTests
    {
        [TestMethod]
        [DynamicData(nameof(Helper.IBITestFiles), typeof(Helper))]
        public void TransformIsIdempotent_IBI(string filename)
        {
            var version = Helper.ReadIBIVersion(filename);
            var nodes = Helper.ReadIBI(filename);

            var bytesAfterFirstTransform = Helper.GenerateIBI(nodes, version);

            TransformNodes.Transform(nodes);

            var bytesAfterSecondTransform = Helper.GenerateIBI(nodes, version);

            int difference = Helper.FindIBIByteDifference(bytesAfterFirstTransform, bytesAfterSecondTransform);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }

        [TestMethod]
        [DynamicData(nameof(Helper.IcarusTestFiles), typeof(Helper))]
        public void TransformIsIdempotent_Icarus(string filename)
        {
            var nodes = Helper.ReadSourceFromFile(filename);

            var bytesAfterFirstTransform = Helper.GenerateIBI(nodes);

            TransformNodes.Transform(nodes);

            var bytesAfterSecondTransform = Helper.GenerateIBI(nodes);

            int difference = Helper.FindIBIByteDifference(bytesAfterFirstTransform, bytesAfterSecondTransform);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }
    }
}
