using DEvahebLib;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    public class ValidateTests
    {
        [TestMethod]
        [DataRow(@"IcarusParserTests\errors\err001.txt", DiagnosticLevel.Error, 1)]
        [DataRow(@"IcarusParserTests\errors\err002.txt", DiagnosticLevel.Error, 2)]
        [DataRow(@"IcarusParserTests\errors\err003.txt", DiagnosticLevel.Error, 3)]
        //[DataRow(@"IcarusParserTests\errors\war001.txt", DiagnosticLevel.Warning, 1)]
        //[DataRow(@"IcarusParserTests\errors\war002.txt", DiagnosticLevel.Warning, 2)]
        [DataRow(@"IcarusParserTests\errors\war003.txt", DiagnosticLevel.Warning, 3)]
        public void TestValidations(string filename, DiagnosticLevel diagnosticLevel, int diagnosticCode)
        {
            var parser = new DEvahebLib.Parser.IcarusParser();

            var nodes = parser.ParseSourceFile(filename);

            parser.Diagnostics.AddRange(ValidateNodes.Validate(nodes));

            Assert.AreEqual(1, parser.Diagnostics.Count, $"One {diagnosticLevel} expected");
            Assert.AreEqual(diagnosticLevel, parser.Diagnostics[0].Level, $"{diagnosticLevel} level expected");
            Assert.AreEqual(diagnosticCode, parser.Diagnostics[0].DiagnosticCode, $"Error code {diagnosticCode} expected");
        }
    }
}
