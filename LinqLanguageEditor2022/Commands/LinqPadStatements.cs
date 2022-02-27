namespace LinqLanguageEditor2022.Commands
{
    [Command(PackageIds.DisplayLinqPadStatementsResults)]
    internal sealed class LinqPadStatements : BaseCommand<LinqPadStatements>
    {
        private const string runSelectedLinqStatement = Constants.RunSelectedLinqStatement;

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                //await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                LinqToolWindowMessenger messenger = await Package.GetServiceAsync<LinqToolWindowMessenger, LinqToolWindowMessenger>();
                messenger.Send(runSelectedLinqStatement);
            }).FireAndForget();
        }
    }
}
