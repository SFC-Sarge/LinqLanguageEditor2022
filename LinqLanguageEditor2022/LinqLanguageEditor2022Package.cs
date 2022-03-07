global using System;

global using Community.VisualStudio.Toolkit;

global using Microsoft.VisualStudio.Shell;

global using Task = System.Threading.Tasks.Task;

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Media;

using LinqLanguageEditor2022.LinqEditor;
using LinqLanguageEditor2022.Options;
using LinqLanguageEditor2022.ToolWindows;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;
namespace LinqLanguageEditor2022
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideToolWindow(typeof(LinqToolWindow.Pane), Style = VsDockStyle.Tabbed, Window = WindowGuids.SolutionExplorer)]
    [ProvideToolWindowVisibility(typeof(LinqToolWindow.Pane), VSConstants.UICONTEXT.SolutionHasSingleProject_string)]
    [ProvideToolWindowVisibility(typeof(LinqToolWindow.Pane), VSConstants.UICONTEXT.SolutionHasMultipleProjects_string)]
    [ProvideToolWindowVisibility(typeof(LinqToolWindow.Pane), VSConstants.UICONTEXT.NoSolution_string)]
    [ProvideToolWindowVisibility(typeof(LinqToolWindow.Pane), VSConstants.UICONTEXT.EmptySolution_string)]
    [ProvideFileIcon(Constants.LinqExt, Constants.ProvideFileIcon)]
    [ProvideMenuResource(Constants.ProvideMenuResource, 1)]
    [Guid(PackageGuids.LinqLanguageEditor2022String)]

    [ProvideLanguageService(typeof(LinqLanguageFactory), Constants.LinqLanguageName, 0, ShowHotURLs = false, DefaultToNonHotURLs = true, EnableLineNumbers = true, EnableAsyncCompletion = true, EnableCommenting = true, ShowCompletion = true, AutoOutlining = true, CodeSense = true)]
    [ProvideLanguageEditorOptionPage(typeof(LinqAdvancedOptionPage), Constants.LinqLanguageName, "", Constants.LinqAdvancedOptionPage, null, 0)]
    [ProvideProfile(typeof(LinqAdvancedOptionPage), Constants.LinqLanguageName, Constants.LinqAdvancedOptionPage, 0, 0, true)]

    [ProvideLanguageExtension(typeof(LinqLanguageFactory), Constants.LinqExt)]
    [ProvideEditorFactory(typeof(LinqLanguageFactory), 740, CommonPhysicalViewAttributes = (int)__VSPHYSICALVIEWATTRIBUTES.PVA_SupportsPreview, TrustLevel = __VSEDITORTRUSTLEVEL.ETL_AlwaysTrusted)]
    [ProvideEditorExtension(typeof(LinqLanguageFactory), Constants.LinqExt, 65536, NameResourceID = 740)]
    [ProvideEditorLogicalView(typeof(LinqLanguageFactory), VSConstants.LOGVIEWID.TextView_string, IsTrusted = true)]

    public sealed class LinqLanguageEditor2022Package : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            LinqLanguageFactory LinqLanguageEditor2022 = new(this);
            RegisterEditorFactory(LinqLanguageEditor2022);

            AddService(typeof(LinqToolWindowMessenger), (_, _, _) => Task.FromResult<object>(new LinqToolWindowMessenger()));
            ((IServiceContainer)this).AddService(typeof(LinqLanguageFactory), LinqLanguageEditor2022, true);

            LinqAdvancedOptions linqAdvancedOptions = await LinqAdvancedOptions.GetLiveInstanceAsync();

            await linqAdvancedOptions.LoadAsync();
            //Console.WriteLine(linqAdvancedOptions.LinqResultsColor.ToString());
            if (linqAdvancedOptions.LinqResultsColor.ToString() != "")
            {
                //Settings Store Values to load.
                LinqAdvancedOptions.Instance.OpenInVSPreviewTab = linqAdvancedOptions.OpenInVSPreviewTab;
                LinqAdvancedOptions.Instance.EnableToolWindowResults = linqAdvancedOptions.EnableToolWindowResults;
                LinqAdvancedOptions.Instance.LinqCodeResultsColor = linqAdvancedOptions.LinqCodeResultsColor;
                LinqAdvancedOptions.Instance.LinqResultsColor = linqAdvancedOptions.LinqResultsColor;
                LinqAdvancedOptions.Instance.LinqResultsEqualMsgColor = linqAdvancedOptions.LinqResultsEqualMsgColor;
                LinqAdvancedOptions.Instance.LinqRunningSelectQueryMsgColor = linqAdvancedOptions.LinqRunningSelectQueryMsgColor;
                LinqAdvancedOptions.Instance.LinqExceptionAdditionMsgColor = linqAdvancedOptions.LinqExceptionAdditionMsgColor;
                await LinqAdvancedOptions.Instance.SaveAsync();
            }
            else
            {
                //Default Values to save to Settings Store.
                linqAdvancedOptions.EnableToolWindowResults = true;
                linqAdvancedOptions.OpenInVSPreviewTab = true;
                linqAdvancedOptions.LinqRunningSelectQueryMsgColor = "LightBlue";
                linqAdvancedOptions.LinqResultsColor = "Yellow";
                linqAdvancedOptions.LinqCodeResultsColor = "LightGreen";
                linqAdvancedOptions.LinqResultsEqualMsgColor = "LightBlue";
                linqAdvancedOptions.LinqExceptionAdditionMsgColor = "Red";
                await linqAdvancedOptions.SaveAsync();
            }

            await this.RegisterCommandsAsync();

            this.RegisterToolWindows();
        }
    }
}
