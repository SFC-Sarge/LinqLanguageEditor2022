namespace LinqLanguageEditor2022.Commands
{
    [Command(PackageIds.DisplayLinqPadMethodResults)]
    internal sealed class LinqPadMethod : BaseCommand<LinqPadMethod>
    {
        private const string _runSelectedLinqMethod = Constants.RunSelectedLinqMethod;

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                //await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                LinqToolWindowMessenger messenger = await Package.GetServiceAsync<LinqToolWindowMessenger, LinqToolWindowMessenger>();
                messenger.Send(_runSelectedLinqMethod);
            }).FireAndForget();
        }
    }
}
