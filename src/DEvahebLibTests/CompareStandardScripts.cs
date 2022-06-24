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

            Console.WriteLine($"{files.Count()} files found");

            int differentFiles = 0;
            foreach (var file in files)
            {
                Console.WriteLine($"{file}");

                string differences = Helper.GenerateSourceFromIBIAndCompareOriginal(Path.ChangeExtension(file, null), variables, originalExtension: ".icarus", ignoreSetTypes: false);
                if (!string.IsNullOrWhiteSpace(differences))
                {
                    differentFiles++;
                    Console.WriteLine(differences);
                }
            }

            Assert.AreEqual(expected: 0, actual: differentFiles);
        }
    }
}
