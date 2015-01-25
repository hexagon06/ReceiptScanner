using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

namespace Receipt.Scanner
{
    public static class Scanner
    {
        public static string Scan(Stream imageStream)
        {
            string resultText;
            var uri = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "tessdata");

            bool result = Directory.Exists(uri.ToString());
            using (var engine = new TesseractEngine(uri.ToString(), "eng", EngineMode.Default))
            {
                // have to load Pix via a bitmap since Pix doesn't support loading a stream.
                using (var image = new System.Drawing.Bitmap(imageStream))
                {
                    using (var pix = PixConverter.ToPix(image))
                    {
                        using (var page = engine.Process(pix))
                        {
                            //finalLabel = String.Format("{0:P}", page.GetMeanConfidence());
                            resultText = page.GetText();
                        }
                    }
                }
            }

            return resultText;
        }

        private const string REGEX_LINE = "^.*( [\\d]{1,4})+([,. ]?[\\d]{1,2})+$";

        public static IDictionary<string, decimal> Convert(string ScanResult)
        {
            var options = RegexOptions.Multiline;
            var lines = Regex.Matches(ScanResult, REGEX_LINE, options);
            return ToDictionary(lines);
        }

        private static IDictionary<string, decimal> ToDictionary(MatchCollection lines)
        {
            Dictionary<string, decimal> values = new Dictionary<string, decimal>();
            foreach (var m in lines)
            {
                var match = m as Match;
                string line = match.ToString();
                Match valueMatch = GetValueMatch(match);
                decimal value = GetValue(valueMatch);
                string name = GetName(match, valueMatch);

                if (!string.IsNullOrEmpty(name))
                {
                    values.Add(name, value);
                }
            }
            return values;
        }

        private static decimal GetValue(Match valueMatch)
        {
            var valueString = valueMatch.ToString().Trim();
            valueString = valueString.Replace(' ', '.');
            valueString = valueString.Replace(',', '.');

            decimal value;
            decimal.TryParse(valueString, out value);
            return value;
        }

        private const string REGEX_VALUE = "([\\d]*)?([,. ]?[\\d]{1,2})?$";

        private static Match GetValueMatch(Match match)
        {
            var valueMatch = Regex.Match(match.ToString(), REGEX_VALUE);
            return valueMatch;
        }

        private static string GetName(Match match, Match valueMatch)
        {
            var matchString = match.ToString();
            var valueString = valueMatch.ToString();

            var index = matchString.IndexOf(valueString);
            string name = (index < 0)
                ? matchString
                : matchString.Remove(index, valueString.Length);
            return name.Trim();
        }

    }
}
