using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class RomanNumeralFixerTests
    {
        [DataTestMethod]
        [DataRow("i ii iii iv v vi vii viii ix x xx xxx xl l lx lxx lxxx xc c", "I II III IV V VI VII VIII IX X XX XXX XL L LX LXX LXXX XC C")]
        [DataRow("i.", "I.")]
        [DataRow("iallegro", "iallegro")]
        [DataRow("i allegro", "I allegro")]
        [DataRow("i.allegro", "I.allegro")]
        [DataRow("i. allegro", "I. allegro")]
        public void ReturnsExpectedString(string before, string after)
        {
            RomanNumeralFixer
                .FixRomanNumerals(before)
                .Should()
                .Be(after);
        }
    }
}
