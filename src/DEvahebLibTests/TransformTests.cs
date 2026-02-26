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
            Assert.AreEqual(10, ((DEvahebLib.Nodes.IntegerValue)loop.Count).Integer);
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
            Assert.AreEqual(2000, ((DEvahebLib.Nodes.IntegerValue)camera.Arguments.ElementAt(2)).Integer);

            // Non-duration arg should stay as FloatValue
            Assert.IsInstanceOfType(camera.Arguments.ElementAt(1), typeof(DEvahebLib.Nodes.FloatValue), "FOV should remain FloatValue");
        }
    }
}
