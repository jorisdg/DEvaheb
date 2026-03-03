using System;
using System.Linq;
using DEvahebLib;
using DEvahebLib.Visitors;

namespace DEvahebLibTests
{
    [TestClass]
    [TestCategory("LocalTest")]
    public class ValidateStandardScripts
    {
        [TestMethod]
        [DynamicData(nameof(Helper.IBITestFiles), typeof(Helper))]
        public void ValidateIBI(string file)
        {
            var nodes = Helper.ReadIBI(file);

            var errors = ValidateNodes.Validate(nodes).Where(d => d.Level == DiagnosticLevel.Error);
            if (errors.Count() > 0)
            {
                Console.WriteLine(string.Join(Environment.NewLine, errors));
            }

            Assert.AreEqual(0, errors.Count(), "Standard IBI files are expected to have no errors");
        }
    }
}
