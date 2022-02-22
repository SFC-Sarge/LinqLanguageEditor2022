using LinqLanguageEditor2022.Extensions;
using LinqLanguageEditor2022.Options;

using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

using MSXML;

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly string _folder;
        private readonly List<string> _templateFiles = new List<string>();
        private const string _defaultExt = Constants.LinqExt;
        private const string _templateDir = ".templates";

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
            var assembly = Assembly.GetExecutingAssembly().Location;
            _folder = Path.Combine(Path.GetDirectoryName(assembly), "Templates");
            _templateFiles.AddRange(Directory.GetFiles(_folder, "*" + _defaultExt, SearchOption.AllDirectories));

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
                        int position = 0;
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
                            await _pane.WriteLineAsync($"{currentSelection} \r\n\r\n{Constants.currentSelectionQuery} = {queryResult}");
                        }
                        if (LinqAdvancedOptions.Instance.EnableToolWindowResults == true)
                        {
                            selectedQueryResult = new() { Text = $"{Constants.currentSelectionQuery} = {queryResult}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                            LinqPadResults.Children.Add(selectedQueryResult);
                            Line line = new() { Margin = new Thickness(0, 0, 0, 20) };
                            LinqPadResults.Children.Add(line);
                        }
                        tempQueryPath = $"{Path.GetTempFileName()}{Constants.LinqExt}";
                        position = await WriteFileAsync(_activeProject, tempQueryPath, currentSelection);

                        if (LinqAdvancedOptions.Instance.OpenInVSPreviewTab == true)
                        {
                            await VS.Documents.OpenInPreviewTabAsync(tempQueryPath);
                        }
                        else
                        {
                            await VS.Documents.OpenAsync(tempQueryPath);
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
        private async Task<int> WriteFileAsync(Project project, string file, string currentSelection)
        {
            string template = await GetTemplateFilePathAsync(project, file, currentSelection);

            if (!string.IsNullOrEmpty(template))
            {
                int index = template.IndexOf('$');

                await WriteToDiskAsync(file, template);
                return index;
            }

            await WriteToDiskAsync(file, string.Empty);

            return 0;
        }
        private async Task WriteToDiskAsync(string file, string content)
        {
            using (StreamWriter writer = new(file, false, GetFileEncoding(file)))
            {
                await writer.WriteAsync(content);
            }
        }
        private Encoding GetFileEncoding(string file)
        {
            string[] noBom = { ".cmd", ".bat", ".json" };
            string ext = Path.GetExtension(file).ToLowerInvariant();

            if (noBom.Contains(ext))
            {
                return new UTF8Encoding(false);
            }

            return new UTF8Encoding(true);
        }

        public async Task<string> GetTemplateFilePathAsync(Project project, string file, string currentSelection)
        {
            var templateFile = Constants.linqTemplate;

            var template = await ReplaceTokensAsync(project, file, currentSelection, templateFile);
            return NormalizeLineEndings(template);
        }

        private static async Task<string> ReplaceTokensAsync(Project project, string file, string currentSelection, string templateFile)
        {
            if (string.IsNullOrEmpty(templateFile))
            {
                return templateFile;
            }
            var rootNs = project.Name;
            var ns = string.IsNullOrEmpty(rootNs) ? "MyNamespace" : rootNs;
            string titleCase = String.Empty;

            string className = Path.GetFileNameWithoutExtension(file);
            if (className.EndsWith(".tmp"))
            {
                className = className.Substring(0, className.Length - 4);
                titleCase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(className.ToLower());
            }
            //using (var reader = new StreamReader(file))
            //{
            //    string content = await reader.ReadToEndAsync();
            //    if (content.StartsWith("//<Query Kind="))
            //    {
            //        currentSelection = content;
            //    }
            //}
            if (currentSelection.StartsWith("<Query Kind="))
            {
                currentSelection = $"//{currentSelection}";
            }
            return templateFile.Replace("{namespace}", ns)
                          .Replace("{itemname}", titleCase)
                          .Replace("{$}", currentSelection);
        }
        private string NormalizeLineEndings(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }

            return Regex.Replace(content, @"\r\n|\n\r|\n|\r", "\r\n");
        }

        private async Task DoOutputWindowsAsync()
        {
            _pane = await VS.Windows.CreateOutputWindowPaneAsync(Constants.linpPadDump);
            return;
        }


    }
}