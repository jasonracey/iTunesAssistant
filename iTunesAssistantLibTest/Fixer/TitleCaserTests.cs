using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class TitleCaserTests
    {
        [DataTestMethod]
        [DataRow("i ii iii iv v vi vii viii ix x xx xxx xl l lx lxx lxxx xc c", "I II III IV V VI VII VIII IX X XX XXX XL L LX LXX LXXX XC C")]
        [DataRow("i.", "I.")]
        [DataRow("iallegro largo", "Iallegro Largo")]
        [DataRow("i allegro largo", "I Allegro Largo")]
        [DataRow("i.allegro largo", "I.Allegro Largo")]
        [DataRow("i. allegro largo", "I. Allegro Largo")]
        public void ReturnsExpectedString(string before, string after)
        {
            TitleCaser
                .ToTitleCase(before)
                .Should()
                .Be(after);
        }
    }
}
