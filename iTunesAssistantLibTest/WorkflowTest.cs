using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WorkflowTest
    {
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Test(string name)
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Workflow(name: name));
        }
    }
}
