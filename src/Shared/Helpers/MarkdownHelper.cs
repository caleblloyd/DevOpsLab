using Markdig;

namespace DevOpsLab.Shared.Helpers
{
    public static class MarkdownHelper
    {
        public static string ToHtml(string markdown) => Markdown.ToHtml(markdown);
    }
}
