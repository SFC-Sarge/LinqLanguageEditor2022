﻿namespace LinqLanguageEditor2022.Commands
{
    [Command(PackageIds.DisplayLinqPadMethodResults)]
    internal sealed class LinqPadMethod : BaseCommand<LinqPadMethod>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                LinqToolWindowMessenger messenger = await Package.GetServiceAsync<LinqToolWindowMessenger, LinqToolWindowMessenger>();
                messenger.Send(Constants.RunSelectedLinqMethod);
            }).FireAndForget();
        }
    }
}
