using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

using LinqLanguageEditor2022.Options;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

using OutputWindowPane = Community.VisualStudio.Toolkit.OutputWindowPane;
using Path = System.IO.Path;
using Project = Community.VisualStudio.Toolkit.Project;

namespace LinqLanguageEditor2022.ToolWindows
{
    public partial class LinqToolWindowControl : UserControl
    {
        //OutputWindowPane _pane = null;
        public LinqToolWindowMessenger ToolWindowMessenger = null;
        public Project _activeProject;
        public string _activeFile;
        public string _myNamespace = null;
        public string queryResult = null;
        public string dirLPRun7 = null;
        public string fileLPRun7 = null;
        public LinqType CurrentLinqMode = 0;
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
            fileLPRun7 = Path.Combine(dirLPRun7, Constants.SolutionToolWindowsFolderName, Constants.LPRun7Executable);

            //ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            //{
            //    await DoOutputWindowsAsync();
            //}).FireAndForget();
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
                    case Constants.RunSelectedLinqStatement:
                        await RunEditorLinqQueryAsync(LinqType.Statement);
                        break;
                    case Constants.RunSelectedLinqMethod:
                        await RunEditorLinqQueryAsync(LinqType.Method);
                        break;
                    case Constants.RunEditorLinqQuery:
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
                //await _pane.ClearAsync();
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
                //await _pane.ClearAsync();
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
                    NothingSelectedResult = new() { Text = Constants.NoActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(NothingSelectedResult);
                    //await _pane.WriteLineAsync(Constants.NoActiveDocument);
                    return;
                }
                if (docView.TextView.Selection != null && !docView.TextView.Selection.IsEmpty)
                {
                    //await _pane.WriteLineAsync(Constants.RunningSelectQuery);
                    runningQueryResult = new() { Text = Constants.RunningSelectQuery, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
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
                                CurrentLinqMode = LinqType.Statement;
                                tempQueryPath = $"{Path.GetTempFileName()}{Constants.LinqExt}";
                                queryString = $"{Constants.QueryKindStatement}\r\n{currentSelection}\r\n{Constants.ResultDump};".Trim();
                                File.WriteAllText(tempQueryPath, $"{queryString}");

                                break;
                            case LinqType.Method:
                                CurrentLinqMode = LinqType.Method;
                                tempQueryPath = $"{Path.GetTempFileName()}{Constants.LinqExt}";
                                methodName = currentSelection.Substring(0, currentSelection.IndexOf("\r"));
                                methodNameComplete = methodName.Substring(methodName.LastIndexOf(" ") + 1, methodName.LastIndexOf(")") - methodName.LastIndexOf(" "));
                                methodCallLine = "{\r\n" + $"{methodNameComplete}" + ";\r\n}";
                                queryString = $"{Constants.QueryKindMethod}\r\n{Constants.VoidMain}\r\n{methodCallLine}\r\n{currentSelection}".Trim();
                                File.WriteAllText(tempQueryPath, $"{queryString}");
                                break;
                            case LinqType.File:
                                CurrentLinqMode = LinqType.File;
                                tempQueryPath = $"{Path.GetTempFileName()}{Constants.LinqExt}";
                                if (!currentSelection.StartsWith(Constants.QueryStartsWith))
                                {
                                    methodName = currentSelection.Substring(0, currentSelection.IndexOf("\r"));
                                    methodNameComplete = methodName.Substring(methodName.LastIndexOf(" ") + 1, methodName.LastIndexOf(")") - methodName.LastIndexOf(" "));
                                    methodCallLine = "{\r\n" + $"{methodNameComplete}" + ";\r\n}";
                                    queryString = $"{Constants.QueryKindMethod}\r\n{Constants.VoidMain}\r\n{methodCallLine}\r\n{currentSelection}".Trim();
                                    File.WriteAllText(tempQueryPath, $"{queryString}");

                                }
                                else if (currentSelection.StartsWith(Constants.QueryStartsWith))
                                {
                                    File.WriteAllText(tempQueryPath, $"{currentSelection}");
                                }
                                else
                                {
                                    queryString = $"{Constants.QueryKindStatement}\r\n{currentSelection}\r\n{Constants.ResultDump};";
                                    File.WriteAllText(tempQueryPath, $"{queryString}");
                                }
                                break;
                            case LinqType.None:
                                CurrentLinqMode = LinqType.None;
                                NothingSelectedResult = new() { Text = Constants.NoActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                LinqPadResults.Children.Add(NothingSelectedResult);
                                //await _pane.WriteLineAsync(Constants.NoActiveDocument);
                                return;
                            default:
                                NothingSelectedResult = new() { Text = Constants.NoActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                LinqPadResults.Children.Add(NothingSelectedResult);
                                //await _pane.WriteLineAsync(Constants.NoActiveDocument);
                                return;
                        }
                        var systemLinqEnumerable = typeof(System.Linq.Enumerable).Assembly;
                        var systemLinqQueryable = typeof(System.Linq.Queryable).Assembly;
                        var systemDiagnostics = typeof(System.Diagnostics.Debug).Assembly;

                        Script script = CSharpScript.Create(currentSelection, ScriptOptions.Default
                                .AddImports("System")
                                .AddImports("System.Linq")
                                .AddImports("System.Collections")
                                .AddImports("System.Collections.Generic")
                                .AddImports("System.Diagnostics")
                                .AddReferences(systemLinqEnumerable)
                                .AddReferences(systemLinqQueryable)
                                .AddReferences(systemDiagnostics));
                        var result = await script.RunAsync();
                        queryResult = $"{result.GetVariable("result").Value}";
                        //await _pane.ClearAsync();
                        LinqPadResults.Children.Clear();
                        if (LinqAdvancedOptions.Instance.EnableToolWindowResults == true)
                        {
                            selectedQueryResult = new() { Text = $"{currentSelection}\r\n\r\nresult = {queryResult}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                            //selectedQueryResult = new() { Text = $"{currentSelection} \r\n\r\n{Constants.CurrentSelectionQuery} = \r\n\r\n{queryResult}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                            //selectedQueryResult = new() { Text = $"{Constants.CurrentSelectionQuery} = \r\n\r\n{queryResult}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
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
                        //await _pane.WriteLineAsync($"{Constants.ExceptionIn} {fileLPRun7} {Constants.ExceptionCall} {exceptionResult.Text}");
                    }
                }
                else
                {
                    NothingSelectedResult = new() { Text = Constants.NoActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(NothingSelectedResult);
                    //await _pane.WriteLineAsync(Constants.NoActiveDocument);
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
            string templateFile = String.Empty;
            switch (CurrentLinqMode)
            {
                case LinqType.None:

                    break;
                case LinqType.Statement:
                    templateFile = Constants.LinqStatementTemplate;
                    break;
                case LinqType.Method:
                    templateFile = Constants.LinqMethodTemplate;

                    break;
                case LinqType.File:

                    break;
            }

            var template = await ReplaceTokensAsync(project, file, currentSelection, templateFile);
            return NormalizeLineEndings(template);
        }

        private async Task<string> ReplaceTokensAsync(Project project, string file, string currentSelection, string templateFile)
        {
            if (string.IsNullOrEmpty(templateFile))
            {
                return templateFile;
            }
            var rootNs = project.Name;
            var ns = string.IsNullOrEmpty(rootNs) ? "MyLinq" : rootNs;
            string className = Path.GetFileNameWithoutExtension(file);
            if (className.EndsWith(Constants.LinqTmpExt))
            {
                className = className.Substring(0, className.Length - 4);
            }
            string titleCase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(className.ToLower());
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                //await _pane.ClearAsync();
                //LinqPadResults.Children.Clear();
                currentSelection = currentSelection.Replace("\r\n{", "\r\n\t\t{")
                    .Replace("\r\n//", "\r\n\t\t\t//")
                    .Replace("\r\nvar", "\r\n\t\t\tvar")
                    .Replace("\r\nnew", "\r\n\t\t\tnew")
                    .Replace("\r\nConsole.WriteLine", "\r\n\t\t\tConsole.WriteLine")
                    .Replace("\r\n\t\tDebug.WriteLine", "\r\n\t\t\t\tDebug.WriteLine")
                    .Replace("\r\nDebug.WriteLine", "\r\n\t\t\tDebug.WriteLine")
                    .Replace("\r\nforeach", "\r\n\t\t\tforeach")
                    .Replace("\r\nif", "\r\n\t\t\tif")
                    .Replace("\r\nelse", "\r\n\t\t\telse")
                    .Replace("\r\ndouble", "\r\n\t\t\tdouble")
                    .Replace("\r\nint", "\r\n\t\t\tint")
                    .Replace("\r\nstring", "\r\n\t\t\tstring")
                    .Replace("\r\nreturn", "\r\n\t\t\treturn")
                    .Replace("\r\nList", "\r\n\t\t\tList")
                    .Replace("\r\n};", "\r\n\t\t\t};")
                    .Replace("\r\ntry\r\n{", "\r\n\t\t\ttry\r\n\t\t\t")
                    .Replace("\r\ncatch", "\r\n\t\t\ttryatch")
                    .Replace("\r\n}", "\r\n\t\t}");
                switch (CurrentLinqMode)
                {
                    case LinqType.None:
                        break;
                    case LinqType.Statement:
                        templateFile = templateFile.Replace("{namespace}", ns)
                                      .Replace("{itemname}", titleCase)
                                      .Replace("{methodname}", $"{titleCase}_Method")
                                      .Replace("{$}", currentSelection);
                        break;
                    case LinqType.Method:
                        templateFile = templateFile.Replace("{namespace}", ns)
                                      .Replace("{itemname}", titleCase)
                                      .Replace("{$}", currentSelection);
                        break;
                    case LinqType.File:
                        //using (var reader = new StreamReader(file))
                        //{
                        //    string content = await reader.ReadToEndAsync();
                        //    if (content.StartsWith("//<Query Kind="))
                        //    {
                        //        currentSelection = content;
                        //    }
                        //}
                        if (currentSelection.StartsWith(Constants.QueryStartsWith))
                        {
                            currentSelection = $"//{currentSelection}";
                        }
                        break;
                }
            }).FireAndForget();
            return templateFile;

        }
        private string NormalizeLineEndings(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }

            return Regex.Replace(content, @"\r\n|\n\r|\n|\r", "\r\n");
        }

        //private async Task DoOutputWindowsAsync()
        //{
        //    //_pane = await VS.Windows.CreateOutputWindowPaneAsync(Constants.LinpPadDump);
        //    return;
        //}
    }
}
