using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEvahebLib;

namespace DEvahebLibTests
{
    [TestClass]
    [TestCategory("LocalTest")]
    public class CompareStandardScripts
    {
        [TestMethod]
        [DataRow(@"C:\GitHub\JEDI_Academy_SDK\Tools\JAscripts", "variable_types.csv")]
        public void TestFunctions(string folder, string variablesFile)
        {
            Variables variables = Variables.FromCsv(variablesFile);

            var files = Directory.EnumerateFiles(folder, "*.IBI", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Helper.GenerateSourceFromIBIAndCompareOriginal(Path.ChangeExtension(file, null), variables, originalExtension: ".icarus");
            }
        }
    }
}
