using DEvahebLib;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    public class ValidateTests
    {
        [TestMethod]
        public void TestValidateValidNodes()
        {
            var set = new DEvahebLib.Nodes.Set(
                new DEvahebLib.Nodes.StringValue("SET_BEHAVIOR_STATE"),
                new DEvahebLib.Nodes.StringValue("BS_CINEMATIC"));
            Assert.AreEqual(0, ValidateNodes.Validate(set).Count, "Valid set() should have no errors");

            var wait = new DEvahebLib.Nodes.Wait(new DEvahebLib.Nodes.FloatValue(1000));
            Assert.AreEqual(0, ValidateNodes.Validate(wait).Count, "Valid wait() should have no errors");

            var ifNode = new DEvahebLib.Nodes.If(
                new DEvahebLib.Nodes.FloatValue(1),
                new DEvahebLib.Nodes.OperatorNode(DEvahebLib.Enums.Operator.Gt),
                new DEvahebLib.Nodes.FloatValue(0));
            Assert.AreEqual(0, ValidateNodes.Validate(ifNode).Count, "Valid if() should have no errors");

            var loop = new DEvahebLib.Nodes.Loop(new DEvahebLib.Nodes.FloatValue(5));
            Assert.AreEqual(0, ValidateNodes.Validate(loop).Count, "Valid loop() should have no errors");

            var flush = new DEvahebLib.Nodes.Flush();
            Assert.AreEqual(0, ValidateNodes.Validate(flush).Count, "Valid flush() should have no errors");
        }

        [TestMethod]
        public void TestValidateInvalidNodes()
        {
            // Wrong argument count
            var set = new DEvahebLib.Nodes.Set(new DEvahebLib.Nodes.StringValue("only_one"));
            var setErrors = ValidateNodes.Validate(set);
            Assert.IsTrue(setErrors.Count > 0, "set() with 1 arg should have errors");
            Assert.IsTrue(setErrors[0] == Diagnostic.ERR001_InvalidArgumentCount(null, 0, 0));

            var wait = new DEvahebLib.Nodes.Wait();
            var waitErrors = ValidateNodes.Validate(wait);
            Assert.IsTrue(waitErrors.Count > 0, "wait() with 0 args should have errors");

            // if() with wrong second argument type
            var ifNode = new DEvahebLib.Nodes.If(
                new DEvahebLib.Nodes.FloatValue(1),
                new DEvahebLib.Nodes.StringValue("not_operator"),
                new DEvahebLib.Nodes.FloatValue(0));
            var ifErrors = ValidateNodes.Validate(ifNode);
            Assert.IsTrue(ifErrors.Count > 0, "if() with non-operator second arg should have errors");

            var flush = new DEvahebLib.Nodes.Flush(new DEvahebLib.Nodes.StringValue("extra"));
            var flushErrors = ValidateNodes.Validate(flush);
            Assert.IsTrue(flushErrors.Count > 0, "flush() with args should have errors");
        }

        [TestMethod]
        public void TestValidateCamera()
        {
            // Valid camera disable (1 arg)
            var disableCmd = DEvahebLib.Nodes.EnumValue.CreateOrPassThrough(
                new DEvahebLib.Nodes.FloatValue((float)DEvahebLib.Enums.CAMERA_COMMANDS.DISABLE),
                typeof(DEvahebLib.Enums.CAMERA_COMMANDS));
            var disable = new DEvahebLib.Nodes.Camera(disableCmd);
            Assert.AreEqual(0, ValidateNodes.Validate(disable).Count, "Valid camera disable should have no errors");

            // Invalid camera disable (too many args)
            var disableCmd2 = DEvahebLib.Nodes.EnumValue.CreateOrPassThrough(
                new DEvahebLib.Nodes.FloatValue((float)DEvahebLib.Enums.CAMERA_COMMANDS.DISABLE),
                typeof(DEvahebLib.Enums.CAMERA_COMMANDS));
            var disableExtra = new DEvahebLib.Nodes.Camera(
                disableCmd2,
                new DEvahebLib.Nodes.FloatValue(100));
            var errors = ValidateNodes.Validate(disableExtra);
            Assert.IsTrue(errors.Count > 0, "Camera disable with extra args should have errors");
        }
    }
}
