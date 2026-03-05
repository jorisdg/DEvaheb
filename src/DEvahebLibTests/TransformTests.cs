using System;
using System.Linq;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    public class TransformTests
    {
        [TestMethod]
        public void TestTransformLoopFloatToInt()
        {
            var loop = new DEvahebLib.Nodes.Loop(new DEvahebLib.Nodes.FloatValue(10));
            Assert.IsInstanceOfType(loop.Count, typeof(DEvahebLib.Nodes.FloatValue), "Before transform, count should be FloatValue");

            TransformNodes.Transform(loop);

            Assert.IsInstanceOfType(loop.Count, typeof(DEvahebLib.Nodes.IntegerValue), "After transform, count should be IntegerValue");
            Assert.AreEqual(10, ((DEvahebLib.Nodes.IntegerValue)loop.Count).Integer, "After transform, value should still be 10");
        }

        [TestMethod]
        public void TestTransformCameraFloatToInt()
        {
            // Camera zoom: command, fov, duration
            var cmdValue = DEvahebLib.Nodes.EnumValue.CreateOrPassThrough(
                new DEvahebLib.Nodes.FloatValue((float)DEvahebLib.Enums.CAMERA_COMMANDS.ZOOM),
                typeof(DEvahebLib.Enums.CAMERA_COMMANDS));

            var camera = new DEvahebLib.Nodes.Camera(
                cmdValue,
                new DEvahebLib.Nodes.FloatValue(80.0f),
                new DEvahebLib.Nodes.FloatValue(2000));

            Assert.IsInstanceOfType(camera.Arguments.ElementAt(2), typeof(DEvahebLib.Nodes.FloatValue), "Before transform, duration should be FloatValue");

            TransformNodes.Transform(camera);

            Assert.IsInstanceOfType(camera.Arguments.ElementAt(2), typeof(DEvahebLib.Nodes.IntegerValue), "After transform, duration should be IntegerValue");
            Assert.AreEqual(2000, ((DEvahebLib.Nodes.IntegerValue)camera.Arguments.ElementAt(2)).Integer, "After transform, duration should still be 2000");

            Assert.IsInstanceOfType(camera.Arguments.ElementAt(1), typeof(DEvahebLib.Nodes.FloatValue), "FOV should remain FloatValue");
            Assert.AreEqual(80.0f, ((DEvahebLib.Nodes.FloatValue)camera.Arguments.ElementAt(1)).Float, "After transform, FOV should still be 80.0");
        }

        [TestMethod]
        [DynamicData(nameof(Helper.IBITestFiles), typeof(Helper))]
        public void TransformIsIdempotent_IBI(string filename, bool jediAcademyFlag)
        {
            var version = Helper.ReadIBIVersion(filename);
            var nodes = Helper.ReadIBI(filename); // ReadIBI does transform already

            var bytesAfterFirstTransform = Helper.GenerateIBI(nodes, version, jediAcademyFlag);

            TransformNodes.Transform(nodes);

            var bytesAfterSecondTransform = Helper.GenerateIBI(nodes, version, jediAcademyFlag);

            int difference = Helper.FindIBIByteDifference(bytesAfterFirstTransform, bytesAfterSecondTransform);

            Assert.AreEqual(-1, difference, $"Found a difference at byte index {difference}");
        }

        [TestMethod]
        [DynamicData(nameof(Helper.IcarusTestFiles), typeof(Helper))]
        public void TransformIsIdempotent_Icarus(string filename)
        {
            var nodesOnceTransformed = Helper.ReadSourceFromFile(filename); // ReadSource does transform already
            var nodesTwiceTransformed = Helper.ReadSourceFromFile(filename); // ReadSource does transform already

            TransformNodes.Transform(nodesTwiceTransformed);

            string differences = Helper.CompareASTs(nodesTwiceTransformed, nodesOnceTransformed, stopOnFirst: false);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }
    }
}
