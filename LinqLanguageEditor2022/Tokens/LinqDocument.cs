using Community.VisualStudio.Toolkit;

using LinqLanguageEditor2022.Commands;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LinqLanguageEditor2022.Tokens
{
    public partial class LinqDocument : IDisposable
    {
        private string[] _lines;
        private readonly ITextBuffer _buffer;
        private bool _isDisposed;

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
            ProcessAsync().FireAndForget();

            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                Project project = await VS.Solutions.GetActiveProjectAsync();
                ProjectName = project?.Name;

            });
        }


        public bool IsParsing { get; private set; }

        public string ProjectName { get; protected set; }

        public string FileName { get; protected set; }

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
            LinqDocument doc = new(lines);
            return doc;
        }

        public async Task ProcessAsync()
        {
            IsParsing = true;
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
                IsParsing = false;

                if (success)
                {
                    Parsed?.Invoke(this);
                }
            }
        }

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

        public event Action<LinqDocument> Parsed;

    }
}
