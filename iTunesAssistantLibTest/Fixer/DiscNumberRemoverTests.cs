using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
  [TestClass]
  public class DiscNumberRemoverTests
  {
    [DataTestMethod]
    [DataRow("In Utero1 disc1", "In Utero1")]
    [DataRow("In Utero1 disc01", "In Utero1")]
    [DataRow("In Utero1 disc 1", "In Utero1")]
    [DataRow("In Utero1 disc 01", "In Utero1")]
    [DataRow("In Utero1 [disc1]", "In Utero1")]
    [DataRow("In Utero1 [disc01]", "In Utero1")]
    [DataRow("In Utero1 [disc 1]", "In Utero1")]
    [DataRow("In Utero1 [disc 01]", "In Utero1")]
    [DataRow("In Utero1 (disc1)", "In Utero1")]
    [DataRow("In Utero1 (disc01)", "In Utero1")]
    [DataRow("In Utero1 (disc 1)", "In Utero1")]
    [DataRow("In Utero1 (disc 01)", "In Utero1")]
    public void RemovesDiscAndNumberFromName(string before, string after)
    {
        DiscNumberRemover
            .RemoveDiscNumber(before)
            .Should()
            .Be(after);
    }

    [DataTestMethod]
    [DataRow("In Utero1 cd1", "In Utero1")]
    [DataRow("In Utero1 cd01", "In Utero1")]
    [DataRow("In Utero1 cd 1", "In Utero1")]
    [DataRow("In Utero1 cd 01", "In Utero1")]
    [DataRow("In Utero1 [cd1]", "In Utero1")]
    [DataRow("In Utero1 [cd01]", "In Utero1")]
    [DataRow("In Utero1 [cd 1]", "In Utero1")]
    [DataRow("In Utero1 [cd 01]", "In Utero1")]
    [DataRow("In Utero1 (cd1)", "In Utero1")]
    [DataRow("In Utero1 (cd01)", "In Utero1")]
    [DataRow("In Utero1 (cd 1)", "In Utero1")]
    [DataRow("In Utero1 (cd 01)", "In Utero1")]
    public void RemovesCdAndNumberFromName(string before, string after)
    {
        DiscNumberRemover
            .RemoveDiscNumber(before)
            .Should()
            .Be(after);
    }
  }
}
