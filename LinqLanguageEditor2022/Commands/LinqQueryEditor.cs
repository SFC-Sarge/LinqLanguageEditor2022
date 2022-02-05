
namespace LinqLanguageEditor2022.Commands
{
    [Command(PackageIds.EditorLinqPad)]
    internal sealed class LinqQueryEditor : BaseCommand<LinqQueryEditor>
    {
        private const string _runEditorLinqQuery = "Run Editor Linq Query.";

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                ToolWindowMessenger messenger = await Package.GetServiceAsync<ToolWindowMessenger, ToolWindowMessenger>();
                messenger.Send(_runEditorLinqQuery);
            }).FireAndForget();
        }
    }
}
