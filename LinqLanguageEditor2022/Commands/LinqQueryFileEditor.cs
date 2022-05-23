namespace LinqLanguageEditor2022.Commands
{
    [Command(PackageIds.LinqEditorLinqPad)]
    internal sealed class LinqQueryFileEditor : BaseCommand<LinqQueryFileEditor>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                //await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                LinqToolWindowMessenger messenger = await Package.GetServiceAsync<LinqToolWindowMessenger, LinqToolWindowMessenger>();
                messenger.Send(Constants.RunEditorLinqQuery);
            }).FireAndForget();
        }
    }
}
