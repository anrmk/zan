using System.Linq;

namespace Core.Extensions {
    /// <summary>
    /// Получение мета-тегов из содержания документа
    /// </summary>
    public class DocumentExtension {
        public static string GetTitle(string content) {
            if(string.IsNullOrEmpty(content))
                return "";
            var res = GetMetaTag(content, "-->", "\r\n    \r\n    \r\n    \r\n");
            return res;
        }

        public static string GetContent(string content) {
            if(string.IsNullOrEmpty(content))
                return "";
            var res = GetMetaTag(content, "\r\n    \r\n    \r\n    \r\n", "\r\n\r\n \r\n");

            var value = res.Replace("\r", string.Empty).Replace("  ", "&nbsp;");
            var arr = value.Split('\n').Where(a => a.Trim() != string.Empty);
            var htmlStr = "<p>" + string.Join("</p><p class='paragraph'>", arr) + "</p>";

            return htmlStr;

            //return res;
        }

        //public static string GetMetaString(string content) {
        //    if(string.IsNullOrEmpty(content))
        //        return "";
        //    Regex regex = new Regex(@"(?=<!--)([\s\S]*?)-->");
        //    MatchCollection matches = regex.Matches(content);
        //    var value = string.Concat(matches.Select(x => x.Value).ToArray());
        //    return value;
        //}

        public static string GetMetaTag(string html, string startSymbol, string lastSymbol) {
            string ret = "";
            int startIndex = html.IndexOf(startSymbol) + startSymbol.Length;
            if(startIndex == -1)
                return string.Empty;
            int lastIndex = html.IndexOf(lastSymbol);
            if(lastIndex == -1) {
                var newHtml = html.Substring(startIndex, html.Length - startIndex - 1);
                lastIndex = newHtml.IndexOf("\n") + startIndex;
            }
            ret = html.Substring(startIndex, lastIndex - startIndex);

            return string.IsNullOrEmpty(ret) ? string.Empty : ret.Trim().TrimStart('=').Trim();
        }
    }
}
