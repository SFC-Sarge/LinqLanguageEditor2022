using LinqLanguageEditor2022.Extensions;
using LinqLanguageEditor2022.Options;

using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

using MSXML;

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

using OutputWindowPane = Community.VisualStudio.Toolkit.OutputWindowPane;
using Path = System.IO.Path;
using Project = Community.VisualStudio.Toolkit.Project;

namespace LinqLanguageEditor2022.ToolWindows
{
    public partial class LinqToolWindowControl : UserControl
    {
        OutputWindowPane _pane = null;
        public LinqToolWindowMessenger ToolWindowMessenger = null;
        public Project _activeProject;
        public string _activeFile;
        public string _myNamespace = null;
        public string queryResult = null;
        public string dirLPRun7 = null;
        public string fileLPRun7 = null;

        public LinqToolWindowControl(Project activeProject, LinqToolWindowMessenger toolWindowMessenger)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            InitializeComponent();
            if (toolWindowMessenger == null)
            {
                toolWindowMessenger = new LinqToolWindowMessenger();
            }
            ToolWindowMessenger = toolWindowMessenger;
            toolWindowMessenger.MessageReceived += OnMessageReceived;
            VS.Events.SolutionEvents.OnAfterCloseSolution += OnAfterCloseSolution;

            dirLPRun7 = Path.GetDirectoryName(typeof(LinqToolWindow).Assembly.Location);
            fileLPRun7 = Path.Combine(dirLPRun7, Constants.solutionToolWindowsFolderName, Constants.lPRun7Executable);
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await DoOutputWindowsAsync();
            }).FireAndForget();
        }
        [Flags]
        public enum LinqType
        {
            None = 0,
            Statement = 1,
            Method = 2,
            File = 3
        }

        private void OnMessageReceived(object sender, string e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                switch (e)
                {
                    case Constants.runSelectedLinqStatement:
                        await RunEditorLinqQueryAsync(LinqType.Statement);
                        break;
                    case Constants.runSelectedLinqMethod:
                        await RunEditorLinqQueryAsync(LinqType.Method);
                        break;
                    case Constants.runEditorLinqQuery:
                        await RunEditorLinqQueryAsync(LinqType.File);
                        break;
                    default:
                        break;
                }
            }).FireAndForget();
        }
        private void OnAfterCloseSolution()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await _pane.ClearAsync();
                LinqPadResults.Children.Clear();
            }).FireAndForget();
        }

        public async Task<DocumentView> OpenDocumentWithSpecificEditorAsync(string file, Guid editorType, Guid LogicalView)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            VsShellUtilities.OpenDocumentWithSpecificEditor(ServiceProvider.GlobalProvider, file, editorType, LogicalView, out _, out _, out IVsWindowFrame frame);
            IVsTextView nativeView = VsShellUtilities.GetTextView(frame);
            return await nativeView.ToDocumentViewAsync();
        }

        public async Task RunEditorLinqQueryAsync(LinqType linqType)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await _pane.ClearAsync();
                LinqPadResults.Children.Clear();
                DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
                _activeFile = docView?.Document?.FilePath;
                _activeProject = await VS.Solutions.GetActiveProjectAsync();
                _myNamespace = _activeProject.Name;
                TextBlock runningQueryResult = null;
                TextBlock NothingSelectedResult = null;
                TextBlock selectedQueryResult = null;
                TextBlock exceptionResult = null;
                if (docView?.TextView == null)
                {
                    NothingSelectedResult = new() { Text = Constants.noActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(NothingSelectedResult);
                    await _pane.WriteLineAsync(Constants.noActiveDocument);
                    return;
                }
                if (docView.TextView.Selection != null && !docView.TextView.Selection.IsEmpty)
                {
                    await _pane.WriteLineAsync(Constants.runningSelectQuery);
                    runningQueryResult = new() { Text = Constants.runningSelectQuery, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(runningQueryResult);

                    string currentSelection = null;
                    string tempQueryPath = null;
                    string methodName = null;
                    string methodNameComplete = null;
                    string methodCallLine = null;
                    string queryString = null;
                    try
                    {
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        currentSelection = docView.TextView.Selection.StreamSelectionSpan.GetText().Trim().Replace("  ", "").Trim();

                        switch (linqType)
                        {
                            case LinqType.Statement:
                                tempQueryPath = $"{Path.GetTempFileName()}{Constants.LinqExt}";
                                queryString = $"{Constants.queryKindStatement}\r\n{currentSelection}\r\n{Constants.resultDump};".Trim();
                                File.WriteAllText(tempQueryPath, $"{queryString}");
                                break;
                            case LinqType.Method:
                                tempQueryPath = $"{Path.GetTempFileName()}{Constants.LinqExt}";
                                methodName = currentSelection.Substring(0, currentSelection.IndexOf("\r"));
                                methodNameComplete = methodName.Substring(methodName.LastIndexOf(" ") + 1, methodName.LastIndexOf(")") - methodName.LastIndexOf(" "));
                                methodCallLine = "{\r\n" + $"{methodNameComplete}" + ";\r\n}";
                                queryString = $"{Constants.queryKindMethod}\r\nvoid Main()\r\n{methodCallLine}\r\n{currentSelection}".Trim();

                                File.WriteAllText(tempQueryPath, $"{queryString}");
                                break;
                            case LinqType.File:

                                tempQueryPath = $"{Path.GetTempFileName()}{Constants.LinqExt}";
                                if (!currentSelection.StartsWith("<Query Kind="))
                                {
                                    methodName = currentSelection.Substring(0, currentSelection.IndexOf("\r"));
                                    methodNameComplete = methodName.Substring(methodName.LastIndexOf(" ") + 1, methodName.LastIndexOf(")") - methodName.LastIndexOf(" "));
                                    methodCallLine = "{\r\n" + $"{methodNameComplete}" + ";\r\n}";
                                    queryString = $"{Constants.queryKindMethod}\r\nvoid Main()\r\n{methodCallLine}\r\n{currentSelection}".Trim();
                                    File.WriteAllText(tempQueryPath, $"{queryString}");
                                }
                                else if (currentSelection.StartsWith("<Query Kind="))
                                {
                                    File.WriteAllText(tempQueryPath, $"{currentSelection}");
                                }
                                else
                                {
                                    queryString = $"{Constants.queryKindStatement}\r\n{currentSelection}\r\n{Constants.resultDump};";
                                    File.WriteAllText(tempQueryPath, $"{queryString}");
                                }
                                break;
                            case LinqType.None:
                                NothingSelectedResult = new() { Text = Constants.noActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                LinqPadResults.Children.Add(NothingSelectedResult);
                                await _pane.WriteLineAsync(Constants.noActiveDocument);
                                return;
                            default:
                                NothingSelectedResult = new() { Text = Constants.noActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                LinqPadResults.Children.Add(NothingSelectedResult);
                                await _pane.WriteLineAsync(Constants.noActiveDocument);
                                return;
                        }


                        using System.Diagnostics.Process process = new();
                        process.StartInfo = new ProcessStartInfo()
                        {
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = fileLPRun7,
                            Arguments = $"{Constants.fileLPRun7Args} {tempQueryPath}",
                            RedirectStandardError = true,
                            RedirectStandardOutput = true
                        };
                        process.Start();
                        queryResult = await process.StandardOutput.ReadToEndAsync();
                        process.WaitForExit();

                        await _pane.ClearAsync();
                        LinqPadResults.Children.Clear();
                        if (LinqAdvancedOptions.Instance.UseLinqPadDumpWindow == true)
                        {
                            //await _pane.WriteLineAsync($"{currentSelection} \r\n\r\n{Constants.currentSelectionQuery} = {queryResult}");
                            await _pane.WriteLineAsync($"{currentSelection} \r\n\r\n{Constants.currentSelectionQuery} = {queryResult}");
                        }
                        if (LinqAdvancedOptions.Instance.EnableToolWindowResults == true)
                        {
                            //TextBlock selectedQueryResult = new TextBlock { Text = $"{currentSelection} \r\n\r\n{Constants.currentSelectionQuery} = {queryResult}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                            selectedQueryResult = new() { Text = $"{Constants.currentSelectionQuery} = {queryResult}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                            LinqPadResults.Children.Add(selectedQueryResult);
                            Line line = new() { Margin = new Thickness(0, 0, 0, 20) };
                            LinqPadResults.Children.Add(line);
                        }
                        tempQueryPath = $"{Path.GetTempFileName()}{Constants.LinqExt}";
                        //System.IO.File.WriteAllText(tempQueryPath, $"{currentSelection} \r\n\r\n{Constants.currentSelectionQuery} = {queryResult}".Trim());
                        File.WriteAllText(tempQueryPath, $"{currentSelection}".Trim());

                        //await OpenDocumentWithSpecificEditorAsync(tempQueryPath, myEditor, myEditorView);
                        Project project = await VS.Solutions.GetActiveProjectAsync();
                        await project.AddExistingFilesAsync(tempQueryPath);
                        ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"AddExistingFilesAsync: {tempQueryPath}");

                        if (LinqAdvancedOptions.Instance.OpenInVSPreviewTab == true)
                        {
                            await VS.Documents.OpenInPreviewTabAsync(tempQueryPath);
                        }
                        else
                        {
                            await VS.Documents.OpenAsync(tempQueryPath);
                        }
                        try
                        {
                            var activeItem = await VS.Solutions.GetActiveItemAsync();
                            if (activeItem != null)
                            {
                                ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"GetActiveItemAsync: {activeItem.Name}");
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionResult = new() { Text = ex.Message, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                        LinqPadResults.Children.Add(exceptionResult);
                        await _pane.WriteLineAsync($"{Constants.exceptionIn} {fileLPRun7} {Constants.exceptionCall} {exceptionResult.Text}");
                    }
                }
                else
                {
                    NothingSelectedResult = new() { Text = Constants.noActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(NothingSelectedResult);
                    await _pane.WriteLineAsync(Constants.noActiveDocument);
                }
            }).FireAndForget();
        }

        private async Task DoOutputWindowsAsync()
        {
            _pane = await VS.Windows.CreateOutputWindowPaneAsync(Constants.linpPadDump);
            return;
        }


    }
}