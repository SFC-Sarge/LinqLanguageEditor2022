using LinqLanguageEditor2022.Parse;

using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Text;

using System.Text;

namespace LinqLanguageEditor2022.Commands
{
    public class LinqFormatDocument
    {
        public static async Task InitializeAsync()
        {
            // We need to manually intercept the FormatDocument command, because language services swallow formatting commands.
            await VS.Commands.InterceptAsync(Microsoft.VisualStudio.VSConstants.VSStd2KCmdID.FORMATDOCUMENT, () =>
            {
                return ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    DocumentView doc = await VS.Documents.GetActiveDocumentViewAsync();

                    if (doc?.TextBuffer != null && doc.TextBuffer.ContentType.IsOfType(Constants.LinqBaselanguageName))
                    {
                        // LinqFormat(doc.TextBuffer);
                        return CommandProgression.Stop;
                    }

                    return CommandProgression.Continue;
                });
            });
        }

        private static void LinqFormat(ITextBuffer buffer)
        {
            LinqDocument doc = buffer.GetDocument();
            StringBuilder sb = new();
            TokenInfo tokenInfo = new();

            Span wholeDocSpan = new Span(0, buffer.CurrentSnapshot.Length);
            buffer.Replace(wholeDocSpan, sb.ToString());
        }
    }
}