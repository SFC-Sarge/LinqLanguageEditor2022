
namespace LinqLanguageEditor2022.Commands
{
    [Command(PackageIds.LinqEditorLinqPad)]
    internal sealed class LinqQueryFileEditor : BaseCommand<LinqQueryFileEditor>
    {
        private const string _runEditorLinqQuery = Constants.RunEditorLinqQuery;

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
