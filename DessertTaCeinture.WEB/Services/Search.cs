using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DessertTaCeinture.WEB.Services
{
    public class Search
    {
        #region Instance
        private static Search _Instance;
        public static Search Instance
        {
            get { return _Instance = _Instance ?? new Search(); }
        }
        private Search() { }
        #endregion

        public string ConvertSearchString(string text)
        {
            text = Regex.Replace(text, "[^\\w\\._]", "").ToLowerInvariant();
            text = RemoveDiacritics(text);

            return text;
        }

        public string RemoveDiacritics(string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }
    }
}