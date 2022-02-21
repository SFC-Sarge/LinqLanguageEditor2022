
namespace LinqLanguageEditor2022.Commands
{
    [Command(PackageIds.LinqEditorLinqPad)]
    internal sealed class LinqQueryEditor : BaseCommand<LinqQueryEditor>
    {
        private const string _runEditorLinqQuery = "Run Editor Linq Query.";

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                //await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                LinqToolWindowMessenger messenger = await Package.GetServiceAsync<LinqToolWindowMessenger, LinqToolWindowMessenger>();
                messenger.Send(_runEditorLinqQuery);
            }).FireAndForget();
        }
    }
}
