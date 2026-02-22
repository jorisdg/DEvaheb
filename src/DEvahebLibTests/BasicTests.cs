using System;
using System.Globalization;
using System.IO;
using DEvahebLib;

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
    }
}