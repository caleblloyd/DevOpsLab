using System.Text.RegularExpressions;

namespace DevOpsLab.Server.Helpers.Data
{
    public static class AlphaNumericHelper
    {
        private const string AlphaNumericPattern = @"[^a-z0-9]+";

        public static string AlphaNumericUnderscore(string s)
        {
            return Regex.Replace(s, AlphaNumericPattern, "_", RegexOptions.IgnoreCase).Trim('_');
        }

        public static string AlphaNumericDash(string s)
        {
            return Regex.Replace(s, AlphaNumericPattern, "-", RegexOptions.IgnoreCase).Trim('-');
        }
    }
}