﻿global using System;

global using Community.VisualStudio.Toolkit;

global using Microsoft.VisualStudio.Shell;

global using Task = System.Threading.Tasks.Task;

using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;

using LinqLanguageEditor2022.LinqEditor;
using LinqLanguageEditor2022.Options;
using LinqLanguageEditor2022.ToolWindows;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;


namespace LinqLanguageEditor2022
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideToolWindow(typeof(LinqToolWindow.Pane), Style = VsDockStyle.Tabbed, Window = WindowGuids.SolutionExplorer)]
    [ProvideToolWindowVisibility(typeof(LinqToolWindow.Pane), VSConstants.UICONTEXT.SolutionHasSingleProject_string)]
    [ProvideToolWindowVisibility(typeof(LinqToolWindow.Pane), VSConstants.UICONTEXT.SolutionHasMultipleProjects_string)]
    [ProvideToolWindowVisibility(typeof(LinqToolWindow.Pane), VSConstants.UICONTEXT.NoSolution_string)]
    [ProvideToolWindowVisibility(typeof(LinqToolWindow.Pane), VSConstants.UICONTEXT.EmptySolution_string)]
    [ProvideFileIcon(Constants.LinqExt, "KnownMonikers.RegistrationScript")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.LinqLanguageEditor2022String)]

    [ProvideLanguageService(typeof(LinqLanguageFactory), Constants.LinqLanguageName, 0, ShowHotURLs = false, DefaultToNonHotURLs = true, EnableLineNumbers = true, EnableAsyncCompletion = true, EnableCommenting = true, ShowCompletion = true, AutoOutlining = true, CodeSense = true)]
    [ProvideLanguageEditorOptionPage(typeof(LinqAdvancedOptionPage), Constants.LinqLanguageName, "", "Advanced", null, 0)]
    [ProvideLanguageEditorOptionPage(typeof(CodeStyleGeneralOptionPage), Constants.LinqLanguageName, "", @"Code Style\General", null, 0)]
    [ProvideLanguageEditorOptionPage(typeof(CodeStyleIndentationOptionPage), Constants.LinqLanguageName, "", @"Code Style\Formatting\Indentation", null, 0)]
    [ProvideLanguageEditorOptionPage(typeof(CodeStyleNewLineOptionPage), Constants.LinqLanguageName, "", @"Code Style\Formatting\New lines", null, 0)]
    [ProvideLanguageEditorOptionPage(typeof(CodeStyleSpacingOptionPage), Constants.LinqLanguageName, "", @"Code Style\Formatting\Spacing", null, 0)]
    [ProvideLanguageEditorOptionPage(typeof(CodeStyleWrappingOptionPage), Constants.LinqLanguageName, "", @"Code Style\Formatting\Wrapping", null, 0)]
    [ProvideLanguageEditorOptionPage(typeof(IntelliSenseOptionPage), Constants.LinqLanguageName, "", "IntelliSense", null, 0)]
    [ProvideLanguageExtension(typeof(LinqLanguageFactory), Constants.LinqExt)]
    [ProvideOptionPage(typeof(GeneralOptionPage), "MyExtension", "General", 0, 0, true)]
    [ProvideProfile(typeof(GeneralOptionPage), "MyExtension", "General", 0, 0, true)]
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

            await this.RegisterCommandsAsync();

            this.RegisterToolWindows();
        }
    }
}
