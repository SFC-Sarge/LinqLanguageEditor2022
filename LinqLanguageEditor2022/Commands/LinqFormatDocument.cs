using LinqLanguageEditor2022.Parse;

using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;

using System.Collections.Generic;
using System.Linq;
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
                        LinqFormat(doc.TextBuffer);
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
            foreach (LinqParseItem item in doc.Items)
            {
                string trimmedLine = item.Text.Trim();
                List<LinqParseItem> items = new();
                string[] myTokens = trimmedLine.Split(new[] { ' ' });
                foreach (string myToken in myTokens)
                {
                    if (LinqNamespaceKeywords.NamespaceKeywords.Any(myToken.Contains))
                    {
                        tokenInfo.Color = TokenColor.Keyword;
                        tokenInfo.Type = TokenType.Keyword;
                    }
                    else if (LinqOperatorKeywords.OperatorKeywords.Any(myToken.Contains))
                    {
                        tokenInfo.Color = TokenColor.Keyword;
                        tokenInfo.Type = TokenType.Text;
                    }
                    else if (LinqModifierKeywords.ModifierKeywords.Any(myToken.Contains))
                    {
                        tokenInfo.Color = TokenColor.Keyword;
                        tokenInfo.Type = TokenType.Keyword;
                    }
                    else if (LinqOperators.Operators.Any(myToken.Contains))
                    {
                        tokenInfo.Color = TokenColor.Keyword;
                        tokenInfo.Type = TokenType.Text;
                    }
                    else if (LinqSeparators.Separators.Any(myToken.Contains))
                    {
                        tokenInfo.Color = TokenColor.Keyword;
                        tokenInfo.Type = TokenType.Text;
                    }
                    else if (LinqStatementModifierKeywords.StatementModifierKeywords.Any(myToken.Contains))
                    {
                        tokenInfo.Color = TokenColor.Keyword;
                        tokenInfo.Type = TokenType.Keyword;
                    }
                    else if (LinqSpecialCharacters.SpecialCharacters.Any(myToken.Contains))
                    {
                        tokenInfo.Color = TokenColor.Keyword;
                        tokenInfo.Type = TokenType.String;
                    }
                    else if (int.TryParse(myToken, out _))
                    {
                        tokenInfo.Color = TokenColor.Keyword;
                        tokenInfo.Type = TokenType.Text;
                    }

                    sb.AppendLine(myToken.Trim());
                }
            }

            Span wholeDocSpan = new Span(0, buffer.CurrentSnapshot.Length);
            buffer.Replace(wholeDocSpan, sb.ToString());
        }

        public int SetHost(IVsContainedLanguageHost pHost)
        {
            throw new NotImplementedException();
        }

        public int GetColorizer(out IVsColorizer ppColorizer)
        {
            throw new NotImplementedException();
        }

        public int GetTextViewFilter(IVsIntellisenseHost pISenseHost, Microsoft.VisualStudio.OLE.Interop.IOleCommandTarget pNextCmdTarget, out IVsTextViewFilter pTextViewFilter)
        {
            throw new NotImplementedException();
        }

        public int GetLanguageServiceID(out Guid pguidLangService)
        {
            throw new NotImplementedException();
        }

        public int SetBufferCoordinator(IVsTextBufferCoordinator pBC)
        {
            throw new NotImplementedException();
        }

        public int Refresh(uint dwRefreshMode)
        {
            throw new NotImplementedException();
        }

        public int WaitForReadyState()
        {
            throw new NotImplementedException();
        }

        public void SetSource(string source, int offset)
        {
            throw new NotImplementedException();
        }

        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            throw new NotImplementedException();
        }
    }
}