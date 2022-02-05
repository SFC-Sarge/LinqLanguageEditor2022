global using Community.VisualStudio.Toolkit;

global using Microsoft.VisualStudio.Shell;

global using System;

global using Task = System.Threading.Tasks.Task;

using LinqLanguageEditor2022.Commands;
using LinqLanguageEditor2022.ToolWindows;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;

using VSIXLinqLanguageEditor2022.LinqEditor;

namespace LinqLanguageEditor2022
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideToolWindow(typeof(MyToolWindow.Pane), Style = VsDockStyle.Tabbed, Window = WindowGuids.SolutionExplorer)]
    [ProvideToolWindowVisibility(typeof(MyToolWindow.Pane), VSConstants.UICONTEXT.SolutionHasSingleProject_string)]
    [ProvideToolWindowVisibility(typeof(MyToolWindow.Pane), VSConstants.UICONTEXT.SolutionHasMultipleProjects_string)]
    [ProvideToolWindowVisibility(typeof(MyToolWindow.Pane), VSConstants.UICONTEXT.NoSolution_string)]
    [ProvideToolWindowVisibility(typeof(MyToolWindow.Pane), VSConstants.UICONTEXT.EmptySolution_string)]
    [ProvideFileIcon(Constants.LinqExt, "KnownMonikers.RegistrationScript")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.LinqLanguageEditor2022String)]

    [ProvideLanguageService(typeof(LinqLanguageFactory), Constants.LinqLanguageName, 0, ShowHotURLs = false, DefaultToNonHotURLs = true, EnableLineNumbers = true, EnableAsyncCompletion = true, EnableCommenting = true, ShowCompletion = true, AutoOutlining = true, CodeSense = true)]
    [ProvideLanguageEditorOptionPage(typeof(Options.OptionsProvider.AdvancedOptions), Constants.LinqLanguageName, "", "Advanced", null, 0)]
    [ProvideLanguageExtension(typeof(LinqLanguageFactory), Constants.LinqExt)]

    [ProvideEditorFactory(typeof(LinqLanguageFactory), 739, CommonPhysicalViewAttributes = (int)__VSPHYSICALVIEWATTRIBUTES.PVA_SupportsPreview, TrustLevel = __VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted)]
    [ProvideEditorExtension(typeof(LinqLanguageFactory), Constants.LinqExt, 65536, NameResourceID = 739)]
    [ProvideEditorLogicalView(typeof(LinqLanguageFactory), VSConstants.LOGVIEWID.TextView_string, IsTrusted = true)]

    public sealed class LinqLanguageEditor2022Package : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            LinqLanguageFactory Linqlanguage = new(this);
            RegisterEditorFactory(Linqlanguage);

            AddService(typeof(ToolWindowMessenger), (_, _, _) => Task.FromResult<object>(new ToolWindowMessenger()));
            ((IServiceContainer)this).AddService(typeof(LinqLanguageFactory), Linqlanguage, true);

            await this.RegisterCommandsAsync();
            await LinqFormatDocument.InitializeAsync();
            await LinqCommentDocument.InitializeAsync();

            this.RegisterToolWindows();
        }
    }
}