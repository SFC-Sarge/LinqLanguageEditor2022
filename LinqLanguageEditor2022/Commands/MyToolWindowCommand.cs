using LinqLanguageEditor2022.ToolWindows;

namespace LinqLanguageEditor2022.Commands
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyToolWindowCommand : BaseCommand<MyToolWindowCommand>
    {
        protected override Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            return MyToolWindow.ShowAsync();
        }
    }
}
