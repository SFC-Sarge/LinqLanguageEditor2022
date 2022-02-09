using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

using System.Linq;
using System.Threading.Tasks;


namespace LinqLanguageEditor2022.Tokens
{
    public partial class LinqDocument : IDisposable
    {
        private string[] _lines;
        private bool _isDisposed;
        private readonly ITextBuffer _buffer;

        protected LinqDocument(string[] lines)
        {
            _lines = lines;
            ProcessAsync().FireAndForget();
        }

        public LinqDocument(ITextBuffer buffer) : this(buffer.CurrentSnapshot.Lines.Select(line => line.GetTextIncludingLineBreak()).ToArray())
        {
            _buffer = buffer;
            _buffer.Changed += BufferChanged;
            FileName = buffer.GetFileName();

#pragma warning disable VSTHRD104 // Offer async methods
            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                Project project = await VS.Solutions.GetActiveProjectAsync();
                ProjectName = project?.Name;

            });
#pragma warning restore VSTHRD104 // Offer async methods
        }


        public bool IsProcessing { get; private set; }

        public string ProjectName { get; protected set; }

        public string FileName { get; protected set; }

        //public List<LinqParseItem> Items { get; private set; } = new();

        public void UpdateLines(string[] lines)
        {
            _lines = lines;
        }

        private void BufferChanged(object sender, TextContentChangedEventArgs e)
        {
            UpdateLines(_buffer.CurrentSnapshot.Lines.Select(line => line.GetTextIncludingLineBreak()).ToArray());
            ProcessAsync().FireAndForget();
        }

        public static LinqDocument FromLines(params string[] lines)
        {
            LinqDocument doc = new LinqDocument(lines);
            return doc;
        }

        public async Task ProcessAsync()
        {
            IsProcessing = true;
            bool success = false;

            await TaskScheduler.Default;

            try
            {
                //Parse();
                success = true;
            }
            catch (Exception ex)
            {
                await ex.LogAsync();
            }
            finally
            {
                IsProcessing = false;

                if (success)
                {
                    Processed?.Invoke(this);
                }
            }
        }

        //public LinqParseItem FindItemFromPosition(int position)
        //{
        //    LinqParseItem item = Items.LastOrDefault(t => t.Span.Contains(position));
        //    LinqParseItem reference = item?.References.FirstOrDefault(v => v != null && v.Span.Contains(position - 1));

        //    // Return the reference if it exist; otherwise the item
        //    return reference ?? item;
        //}

        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_buffer != null)
                {
                    _buffer.Changed -= BufferChanged;
                }
            }

            _isDisposed = true;
        }

        public event Action<LinqDocument> Processed;
    }
}
