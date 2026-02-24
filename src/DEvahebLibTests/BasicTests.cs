using System;
using System.Globalization;
using System.IO;
using System.Linq;
using DEvahebLib;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    public class BasicTests
    {
        /// <summary>
        /// <RunSettings>
        ///   <TestRunParameters>
        ///     <Parameter name = "VariableTypesFile" value="..\..\..\..\DEvaheb\variable_types.csv" />
        ///   </TestRunParameters>
        /// </RunSettings>
        /// </summary>
        public static string variablesFile = @"..\..\..\..\DEvaheb\variable_types.csv";
        private static Variables variables = null;

        public static Variables VariableList
        {
            get
            {
                if (variables == null)
                {
                    variables = Variables.FromCsv(variablesFile);
                }

                return variables;
            }
        }

        [ClassInitialize]
        public static void TestClassInitialize(TestContext testContext)
        {
            if (variables == null)
            {
                string variableTypesFile = testContext?.Properties["VariableTypesFile"]?.ToString();

                if (!string.IsNullOrWhiteSpace(variableTypesFile))
                {
                    variablesFile = variableTypesFile;
                }
            }
        }

        [TestMethod]
        [DataRow(@"BasicTests\camera", "en-US")]
        [DataRow(@"BasicTests\declare", "en-US")]
        [DataRow(@"BasicTests\do", "en-US")]
        [DataRow(@"BasicTests\dowait", "en-US")]
        [DataRow(@"BasicTests\flush", "en-US")]
        [DataRow(@"BasicTests\free", "en-US")]
        [DataRow(@"BasicTests\get", "en-US")]
        [DataRow(@"BasicTests\kill", "en-US")]
        [DataRow(@"BasicTests\move", "en-US")]
        [DataRow(@"BasicTests\play", "en-US")]
        [DataRow(@"BasicTests\print", "en-US")]
        [DataRow(@"BasicTests\random", "en-US")]
        [DataRow(@"BasicTests\remove", "en-US")]
        [DataRow(@"BasicTests\rotate", "en-US")]
        [DataRow(@"BasicTests\run", "en-US")]
        [DataRow(@"BasicTests\set", "en-US")]
        [DataRow(@"BasicTests\signal", "en-US")]
        [DataRow(@"BasicTests\sound", "en-US")]
        [DataRow(@"BasicTests\tag", "en-US")]
        [DataRow(@"BasicTests\use", "en-US")]
        [DataRow(@"BasicTests\wait", "en-US")]
        [DataRow(@"BasicTests\waitsignal", "en-US")]

        [DataRow(@"BasicTests\camera", "nl-BE")]
        [DataRow(@"BasicTests\declare", "nl-BE")]
        [DataRow(@"BasicTests\do", "nl-BE")]
        [DataRow(@"BasicTests\dowait", "nl-BE")]
        [DataRow(@"BasicTests\flush", "nl-BE")]
        [DataRow(@"BasicTests\free", "nl-BE")]
        [DataRow(@"BasicTests\get", "nl-BE")]
        [DataRow(@"BasicTests\kill", "nl-BE")]
        [DataRow(@"BasicTests\move", "nl-BE")]
        [DataRow(@"BasicTests\play", "nl-BE")]
        [DataRow(@"BasicTests\print", "nl-BE")]
        [DataRow(@"BasicTests\random", "nl-BE")]
        [DataRow(@"BasicTests\remove", "nl-BE")]
        [DataRow(@"BasicTests\rotate", "nl-BE")]
        [DataRow(@"BasicTests\run", "nl-BE")]
        [DataRow(@"BasicTests\set", "nl-BE")]
        [DataRow(@"BasicTests\signal", "nl-BE")]
        [DataRow(@"BasicTests\sound", "nl-BE")]
        [DataRow(@"BasicTests\tag", "nl-BE")]
        [DataRow(@"BasicTests\use", "nl-BE")]
        [DataRow(@"BasicTests\wait", "nl-BE")]
        [DataRow(@"BasicTests\waitsignal", "nl-BE")]
        public void TestFunctions(string filenameBase, string cultureName)
        {
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(cultureName);
            Assert.AreEqual(string.Empty, Helper.GenerateSourceFromIBIAndCompareOriginal(filenameBase, variables: VariableList, parity: DEvahebLib.Visitors.SourceCodeParity.BehavED));
        }


        [TestMethod]
        [DataRow(@"BasicTests\affect", "en-US")]
        [DataRow(@"BasicTests\else", "en-US")]
        [DataRow(@"BasicTests\if", "en-US")]
        [DataRow(@"BasicTests\loop", "en-US")]
        [DataRow(@"BasicTests\task", "en-US")]

        [DataRow(@"BasicTests\affect", "nl-BE")]
        [DataRow(@"BasicTests\else", "nl-BE")]
        [DataRow(@"BasicTests\if", "nl-BE")]
        [DataRow(@"BasicTests\loop", "nl-BE")]
        [DataRow(@"BasicTests\task", "nl-BE")]
        public void TestBlocks(string filenameBase, string cultureName)
        {
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(cultureName);
            Assert.AreEqual(string.Empty, Helper.GenerateSourceFromIBIAndCompareOriginal(filenameBase, variables: VariableList, parity: DEvahebLib.Visitors.SourceCodeParity.BehavED));
        }

        [TestMethod]
        [DataRow(@"BasicTests\camera")]
        [DataRow(@"BasicTests\declare")]
        [DataRow(@"BasicTests\do")]
        [DataRow(@"BasicTests\dowait")]
        [DataRow(@"BasicTests\flush")]
        [DataRow(@"BasicTests\free")]
        [DataRow(@"BasicTests\get")]
        [DataRow(@"BasicTests\kill")]
        [DataRow(@"BasicTests\move")]
        [DataRow(@"BasicTests\play")]
        [DataRow(@"BasicTests\print")]
        [DataRow(@"BasicTests\random")]
        [DataRow(@"BasicTests\remove")]
        [DataRow(@"BasicTests\rotate")]
        [DataRow(@"BasicTests\run")]
        [DataRow(@"BasicTests\set")]
        [DataRow(@"BasicTests\signal")]
        [DataRow(@"BasicTests\sound")]
        [DataRow(@"BasicTests\tag")]
        [DataRow(@"BasicTests\use")]
        [DataRow(@"BasicTests\wait")]
        [DataRow(@"BasicTests\waitsignal")]
        [DataRow(@"BasicTests\affect")]
        [DataRow(@"BasicTests\else")]
        [DataRow(@"BasicTests\if")]
        [DataRow(@"BasicTests\loop")]
        [DataRow(@"BasicTests\task")]
        public void TestRoundTripIBI(string filenameBase)
        {
            var ibiFile = filenameBase + ".IBI";
            var originalBytes = File.ReadAllBytes(ibiFile);
            var version = Helper.ReadIBIVersion(ibiFile);
            var nodes = Helper.ReadIBI(ibiFile);
            var generatedBytes = Helper.WriteIBI(nodes, version);
            string differences = Helper.CompareIBIBytes(originalBytes, generatedBytes);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }

        [TestMethod]
        [DataRow(@"BasicTests\camera", "en-US")]
        [DataRow(@"BasicTests\declare", "en-US")]
        [DataRow(@"BasicTests\do", "en-US")]
        [DataRow(@"BasicTests\dowait", "en-US")]
        [DataRow(@"BasicTests\flush", "en-US")]
        [DataRow(@"BasicTests\free", "en-US")]
        [DataRow(@"BasicTests\get", "en-US")]
        [DataRow(@"BasicTests\kill", "en-US")]
        [DataRow(@"BasicTests\move", "en-US")]
        [DataRow(@"BasicTests\play", "en-US")]
        [DataRow(@"BasicTests\print", "en-US")]
        [DataRow(@"BasicTests\random", "en-US")]
        [DataRow(@"BasicTests\remove", "en-US")]
        [DataRow(@"BasicTests\rotate", "en-US")]
        [DataRow(@"BasicTests\run", "en-US")]
        [DataRow(@"BasicTests\set", "en-US")]
        [DataRow(@"BasicTests\signal", "en-US")]
        [DataRow(@"BasicTests\sound", "en-US")]
        [DataRow(@"BasicTests\tag", "en-US")]
        [DataRow(@"BasicTests\use", "en-US")]
        [DataRow(@"BasicTests\wait", "en-US")]
        [DataRow(@"BasicTests\waitsignal", "en-US")]
        [DataRow(@"BasicTests\affect", "en-US")]
        [DataRow(@"BasicTests\else", "en-US")]
        [DataRow(@"BasicTests\if", "en-US")]
        [DataRow(@"BasicTests\loop", "en-US")]
        [DataRow(@"BasicTests\task", "en-US")]
        [DataRow(@"BasicTests\camera", "nl-BE")]
        [DataRow(@"BasicTests\declare", "nl-BE")]
        [DataRow(@"BasicTests\do", "nl-BE")]
        [DataRow(@"BasicTests\dowait", "nl-BE")]
        [DataRow(@"BasicTests\flush", "nl-BE")]
        [DataRow(@"BasicTests\free", "nl-BE")]
        [DataRow(@"BasicTests\get", "nl-BE")]
        [DataRow(@"BasicTests\kill", "nl-BE")]
        [DataRow(@"BasicTests\move", "nl-BE")]
        [DataRow(@"BasicTests\play", "nl-BE")]
        [DataRow(@"BasicTests\print", "nl-BE")]
        [DataRow(@"BasicTests\random", "nl-BE")]
        [DataRow(@"BasicTests\remove", "nl-BE")]
        [DataRow(@"BasicTests\rotate", "nl-BE")]
        [DataRow(@"BasicTests\run", "nl-BE")]
        [DataRow(@"BasicTests\set", "nl-BE")]
        [DataRow(@"BasicTests\signal", "nl-BE")]
        [DataRow(@"BasicTests\sound", "nl-BE")]
        [DataRow(@"BasicTests\tag", "nl-BE")]
        [DataRow(@"BasicTests\use", "nl-BE")]
        [DataRow(@"BasicTests\wait", "nl-BE")]
        [DataRow(@"BasicTests\waitsignal", "nl-BE")]
        [DataRow(@"BasicTests\affect", "nl-BE")]
        [DataRow(@"BasicTests\else", "nl-BE")]
        [DataRow(@"BasicTests\if", "nl-BE")]
        [DataRow(@"BasicTests\loop", "nl-BE")]
        [DataRow(@"BasicTests\task", "nl-BE")]
        public void TestSourceParseMatchesIBI(string filenameBase, string cultureName)
        {
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(cultureName);

            var ibiNodes = Helper.ReadIBI(filenameBase + ".IBI");
            var sourceNodes = Helper.ReadSource(filenameBase + ".txt");

            string differences = Helper.CompareASTs(ibiNodes, sourceNodes);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }
        [TestMethod]
        public void TestConvertCommentsToRem()
        {
            string source = "// This is a comment\nuse ( \"hello\" );\n// Another comment\n";
            var parser = new DEvahebLib.Parser.IcarusParser();

            var nodesWithout = parser.Parse(source, convertComments: false);
            Assert.AreEqual(1, nodesWithout.Count, "Without conversion, only the use statement should be parsed");

            var nodesWith = parser.Parse(source, convertComments: true);
            Assert.AreEqual(3, nodesWith.Count, "With conversion, 2 rem nodes + 1 use statement");

            Assert.IsInstanceOfType(nodesWith[0], typeof(DEvahebLib.Nodes.Rem));
            var rem1 = (DEvahebLib.Nodes.Rem)nodesWith[0];
            Assert.AreEqual("rem", rem1.Name);
            Assert.AreEqual("This is a comment", ((DEvahebLib.Nodes.StringValue)rem1.Comment).String);

            Assert.IsInstanceOfType(nodesWith[2], typeof(DEvahebLib.Nodes.Rem));
            var rem2 = (DEvahebLib.Nodes.Rem)nodesWith[2];
            Assert.AreEqual("rem", rem2.Name);
            Assert.AreEqual("Another comment", ((DEvahebLib.Nodes.StringValue)rem2.Comment).String);
        }

        [TestMethod]
        [DataRow(@"BasicTests\singleline")]
        public void TestSingleLineStatements(string filenameBase)
        {
            var singleLineNodes = Helper.ReadSource(filenameBase + ".txt");
            var expectedNodes = Helper.ReadSource(filenameBase + "_expected.txt");

            string differences = Helper.CompareASTs(expectedNodes, singleLineNodes);

            if (!string.IsNullOrWhiteSpace(differences))
            {
                Console.WriteLine(differences);
            }

            Assert.AreEqual(string.Empty, differences);
        }

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
            Assert.IsTrue(setErrors[0].Contains("2 argument"), setErrors[0]);

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