namespace LinqLanguageEditor2022.Commands
{
    [Command(PackageIds.DisplayLinqPadStatementsResults)]
    internal sealed class LinqPadStatements : BaseCommand<LinqPadStatements>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                LinqToolWindowMessenger messenger = await Package.GetServiceAsync<LinqToolWindowMessenger, LinqToolWindowMessenger>();
                messenger.Send(Constants.RunSelectedLinqStatement);
            }).FireAndForget();
        }
    }
}
