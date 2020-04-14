using Ganss.XSS;
using Markdig;

namespace DevOpsLab.Client.Helpers
{
    public static class HtmlHelper
    {
        private static HtmlSanitizer HtmlSanitizer = new HtmlSanitizer();

        public static string Sanitize(string html) => HtmlSanitizer.Sanitize(html);

        public static string MarkdownToHtml(string markdown) => Sanitize(Markdown.ToHtml(markdown));
    }
}
