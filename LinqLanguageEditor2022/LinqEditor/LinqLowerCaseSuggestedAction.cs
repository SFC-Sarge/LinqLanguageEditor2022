using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LinqLanguageEditor2022.LinqEditor
{
    internal class LinqLowerCaseSuggestedAction : ISuggestedAction
    {
        private ITrackingSpan m_span;
        private string m_lower;
        private string m_display;
        private ITextSnapshot m_snapshot;

        public LinqLowerCaseSuggestedAction(ITrackingSpan span)
        {
            m_span = span;
            m_snapshot = span.TextBuffer.CurrentSnapshot;
            m_lower = span.GetText(m_snapshot).ToLower();
            m_display = string.Format("Convert '{0}' to lower case", span.GetText(m_snapshot));
        }
        public Task<object> GetPreviewAsync(CancellationToken cancellationToken)
        {
            var textBlock = new TextBlock();
            textBlock.Padding = new Thickness(5);
            textBlock.Inlines.Add(new Run() { Text = m_lower });
            return Task.FromResult<object>(textBlock);
        }
        public Task<IEnumerable<SuggestedActionSet>> GetActionSetsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<SuggestedActionSet>>(null);
        }
        public bool HasActionSets
        {
            get { return false; }
        }
        public string DisplayText
        {
            get { return m_display; }
        }
        public ImageMoniker IconMoniker
        {
            get { return default(ImageMoniker); }
        }
        public string IconAutomationText
        {
            get
            {
                return null;
            }
        }
        public string InputGestureText
        {
            get
            {
                return null;
            }
        }
        public bool HasPreview
        {
            get { return true; }
        }
        public void Invoke(CancellationToken cancellationToken)
        {
            m_span.TextBuffer.Replace(m_span.GetSpan(m_snapshot), m_lower);
        }
        public void Dispose()
        {
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            // This is a sample action and doesn't participate in LightBulb telemetry
            telemetryId = Guid.Empty;
            return false;
        }
    }
}
