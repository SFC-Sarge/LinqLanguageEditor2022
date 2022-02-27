using LinqLanguageEditor2022.ToolWindows;

namespace LinqLanguageEditor2022.Commands
{
    [Command(PackageIds.LinqToolWindowCommand)]
    internal sealed class LinqToolWindowCommand : BaseCommand<LinqToolWindowCommand>
    {
        protected override Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            return LinqToolWindow.ShowAsync();
        }
    }
}
