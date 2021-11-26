using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace iTunesAssistantLib
{
    public static class RegionAbbreviationFixer
    {
        private static readonly Regex RegionAbbreviationRegex = new Regex(@"^\s*(A[BKLRSZ]|BC|C[AOT]|D[CE]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ABDEHINOPST]|N[BCDEHJLMSTUVY]|O[HKNR]|P[AERW]|QC|RI|S[CDK]|T[NX]|UT|V[AIT]|W[AIVY]|YT)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string FixRegionAbbreviation(this string s)
        {
            var builder = new StringBuilder();

            var parts = s
                .Split(",")
                .ToArray();

            // Require a minimum format of "venue, city, region" to proceed
            if (parts.Length < 3)
                return s;

            for (int i = 0; i < parts.Length; i++)
            {
                var curr = parts[i];
                var last = i == parts.Length - 1;
                if (last)
                {
                    curr = RegionAbbreviationRegex.Replace(curr.TrimEnd(), match => match.Value.ToUpperInvariant());
                    builder.Append($"{curr}");
                }
                else
                {
                    builder.Append($"{curr},");
                }
            }

            return builder.ToString();
        }
    }
}
