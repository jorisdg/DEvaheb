using DEvahebLib;

namespace DEvahebLibTests
{
    [TestClass]
    public class IcarusParserDiagnosticTests
    {
        [TestMethod]
        [DataRow(@"IcarusParserTests\errors\err004.txt", DiagnosticLevel.Error, 4)]
        [DataRow(@"IcarusParserTests\errors\err005.txt", DiagnosticLevel.Error, 5)]
        [DataRow(@"IcarusParserTests\errors\err006.txt", DiagnosticLevel.Error, 6)]
        public void TestNoSemiColons(string filename, DiagnosticLevel diagnosticLevel, int diagnosticCode)
        {
            var parser = new DEvahebLib.Parser.IcarusParser();

            var nodes = parser.ParseSourceFile(filename);

            Assert.AreEqual(1, parser.Diagnostics.Count, $"One {diagnosticLevel} expected");
            Assert.AreEqual(diagnosticLevel, parser.Diagnostics[0].Level, $"{diagnosticLevel} level expected");
            Assert.AreEqual(diagnosticCode, parser.Diagnostics[0].DiagnosticCode, $"Error code {diagnosticCode} expected");
        }
    }
}
