using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using LinqLanguageEditor2022.Options;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

using static LinqLanguageEditor2022.ToolWindows.LinqToolWindowControl;

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
        }
        [Flags]
        public enum LinqType
        {
            None = 0,
            Statement = 1,
            Method = 2,
        }

        private void OnMessageReceived(object sender, string e)
        {
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                switch (e)
                {
                    case Constants.RunSelectedLinqMethod:
                        await RunEditorLinqQueryAsync(LinqType.Method);
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
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
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
        public async Task RunLinqQueriesAsync(string modifiedSelection)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                LinqPadResults.Children.Clear();
                TextBlock runningQueryResult = null;
                TextBlock exceptionResult = null;
                TextBlock exceptionAdditionMsg = null;
                TextBlock queryResultMsg = null;
                TextBlock queryResults = null;
                TextBlock queryResultEquals = new() { Text = $"{Constants.LinqQueryEquals}", Foreground = Brushes.LightBlue, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                TextBlock queryCodeHeader = new() { Text = $"{Constants.LinqQueryTextHeader}", Foreground = Brushes.LightBlue, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };

                Line line = new() { Margin = new Thickness(0, 0, 0, 20) };
                runningQueryResult = new() { Text = $"{Constants.RunningSelectQuery}\r\n", Foreground = Brushes.GreenYellow, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                LinqPadResults.Children.Add(runningQueryResult);
                try
                {
                    var systemLinqEnumerable = typeof(System.Linq.Enumerable).Assembly;
                    var systemLinqQueryable = typeof(System.Linq.Queryable).Assembly;
                    var systemDiagnostics = typeof(System.Diagnostics.Debug).Assembly;

                    Script script = CSharpScript.Create(modifiedSelection, ScriptOptions.Default
                            .AddImports("System")
                            .AddImports("System.Linq")
                            .AddImports("System.Collections")
                            .AddImports("System.Collections.Generic")
                            .AddImports("System.Diagnostics")
                            .AddReferences(systemLinqEnumerable)
                            .AddReferences(systemLinqQueryable)
                            .AddReferences(systemDiagnostics));
                    var result = await script.RunAsync();
                    var allVariables = result.Variables;
                    var variable = allVariables.Where(n => n.Name == Constants.LinqResultText);
                    foreach (var variable1 in result.Variables)
                        Debug.WriteLine($"{variable1.Name} = {variable1.Value}\r\n of type {variable1.Type}");
                    string tempResults = String.Empty;

                    if (variable.First().Name == Constants.LinqResultText)
                    {
                        var returnValue = result.GetVariable(Constants.LinqResultText).Value;
                        var myType = returnValue.GetType();
                        queryResultMsg = new() { Text = $"{result.Script.Code}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                        LinqPadResults.Children.Add(queryCodeHeader);
                        LinqPadResults.Children.Add(line);
                        LinqPadResults.Children.Add(queryResultMsg);

                        if (myType == typeof(int) || myType == typeof(string) || myType == typeof(bool) || myType == typeof(float) || myType == typeof(double))
                        {
                            tempResults = $"{result.GetVariable(Constants.LinqResultText).Value}";
                        }
                        else if (myType == typeof(string[]))
                        {
                            var stringArrays = (string[])result.GetVariable(variable.First().Name).Value;
                            if (stringArrays.Length > 0)
                            {
                                foreach (var stringArray in stringArrays)
                                {
                                    tempResults += $"{stringArray}\r\n";
                                }
                            }
                        }
                        else if (myType == typeof(int[]))
                        {
                            var intArrays = (int[])result.GetVariable(variable.First().Name).Value;
                            if (intArrays.Length > 0)
                            {
                                foreach (var intArray in intArrays)
                                {
                                    tempResults += $"{intArray}\r\n";
                                }
                            }
                        }
                        else if (myType == typeof(List<string>))
                        {
                            var listStrings = (List<string>)result.GetVariable(variable.First().Name).Value;
                            if (listStrings.Count() > 0)
                            {
                                foreach (var listString in listStrings)
                                {
                                    tempResults += $"{listString}\r\n";
                                }
                            }
                        }
                        else if (myType == typeof(IEnumerable<double>) || (myType.FullName.Contains("OfTypeIterator") && myType.FullName.Contains("Double")))
                        {
                            IEnumerable<double> enumDoubles = (IEnumerable<double>)returnValue;
                            if (enumDoubles.Count() > 0)
                            {
                                foreach (var enumDouble in enumDoubles)
                                {
                                    tempResults += $"{enumDouble}\r\n";
                                }
                            }
                        }
                        else if (myType == typeof(Int64))
                        {
                            var int64 = (Int64)returnValue;
                            tempResults += $"{int64}";
                        }
                        else if (myType == typeof(IEnumerable<float>))
                        {
                            IEnumerable<float> enumFloats = (IEnumerable<float>)returnValue;
                            if (enumFloats.Count() > 0)
                            {
                                foreach (var enumFloat in enumFloats)
                                {
                                    tempResults += $"{enumFloat}\r\n";
                                }
                            }
                        }
                        else if (myType.FullName.Contains("SelectArrayIterator") && myType.FullName.Contains("Decimal"))
                        {
                            IEnumerable<decimal> enumDecimals = (IEnumerable<decimal>)returnValue;
                            if (enumDecimals.Count() > 0)
                            {
                                foreach (var enumDecimal in enumDecimals)
                                {
                                    tempResults += $"{enumDecimal}\r\n";
                                }
                            }
                        }
                        else if (myType.FullName.Contains("OrderedEnumerable") && myType.FullName.Contains("DateTime")
                        || myType == typeof(IEnumerable<DateTime>))
                        {
                            IOrderedEnumerable<DateTime> orderedDates = (IOrderedEnumerable<DateTime>)returnValue;
                            if (orderedDates.Count() > 0)
                            {
                                foreach (var orderedDate in orderedDates)
                                {
                                    tempResults += $"{orderedDate}\r\n";
                                }
                            }
                        }
                        else if (myType.FullName.Contains("OrderedEnumerable") && myType.FullName.Contains("Submission")
                        || myType.FullName.Contains("GroupJoinIterator") && myType.FullName.Contains("Submission")
                        || myType.FullName.Contains("SelectManyIterator") && myType.FullName.Contains("String")
                        || myType.FullName.Contains("SelectIterator") && myType.FullName.Contains("String")
                        || myType.FullName.Contains("SkipWhileIterator") && myType.FullName.Contains("String")
                        || myType.FullName.Contains("OfTypeIterator") && myType.FullName.Contains("String")
                        || myType.FullName.Contains("SelectArrayIterator") && myType.FullName.Contains("Double")
                        || myType.FullName.Contains("WhereArrayIterator") && myType.FullName.Contains("Submission"))
                        {
                            var orderedObjects = (IEnumerable<object>)returnValue;
                            if (orderedObjects.Count() > 0)
                            {
                                foreach (var orderedString in orderedObjects)
                                {
                                    tempResults += $"{orderedObjects}\r\n";
                                }
                            }
                        }
                        else if (myType.FullName.Contains("OrderedEnumerable") && myType.FullName.Contains("Int32"))
                        {
                            IOrderedEnumerable<int> orderedInts = (IOrderedEnumerable<int>)returnValue;
                            if (orderedInts.Count() > 0)
                            {
                                foreach (var orderedInt in orderedInts)
                                {
                                    tempResults += $"{orderedInt}\r\n";
                                }
                            }
                        }
                        else if (myType.FullName.Contains("OrderedEnumerable") && myType.FullName.Contains("String"))
                        {
                            IOrderedEnumerable<string> orderedStrings = (IOrderedEnumerable<string>)returnValue;
                            if (orderedStrings.Count() > 0)
                            {
                                foreach (var orderedString in orderedStrings)
                                {
                                    tempResults += $"{orderedString}\r\n";
                                }
                            }
                        }
                        else if (myType.FullName.Contains("JoinIterator") && myType.FullName.Contains("String")
                        || myType.FullName.Contains("ListPartition") && myType.FullName.Contains("String")
                        || myType.FullName.Contains("EmptyPartition") && myType.FullName.Contains("String")
                        || myType.FullName.Contains("RepeatIterator") && myType.FullName.Contains("String")
                        || myType.FullName.Contains("Concat2Iterator") && myType.FullName.Contains("String")
                        || myType == typeof(IEnumerable<string>))
                        {
                            var repeatObjects = (IEnumerable<string>)returnValue;
                            if (repeatObjects.Count() > 0)
                            {
                                foreach (var repeatObject in repeatObjects)
                                {
                                    tempResults += $"{repeatObject}\r\n";
                                }
                            }
                        }
                        else if (myType.FullName.Contains("ReverseIterator") && myType.FullName.Contains("Char"))
                        {
                            var reverseChars = (IEnumerable<char>)returnValue;
                            if (reverseChars.Count() > 0)
                            {
                                foreach (var reverseChar in reverseChars)
                                {
                                    tempResults += $"{reverseChar}\r\n";
                                }
                            }
                        }
                        else if (myType.FullName.Contains("TakeWhileIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("DistinctIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("ExceptIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("OfTypeIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("UnionIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("IntersectIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("WhereIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("WhereArrayIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("WhereSelectEnumerableIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("ZipIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("DefaultIfEmptyIterator") && myType.FullName.Contains("Int32")
                        || myType.FullName.Contains("ListPartition") && myType.FullName.Contains("Int32")
                        || myType.Name.Contains("RangeIterator")
                        || myType == typeof(IEnumerable<int>))
                        {
                            var takeWhileInts = (IEnumerable<int>)returnValue;
                            if (takeWhileInts.Count() > 0)
                            {
                                foreach (var takeWhileInt in takeWhileInts)
                                {
                                    tempResults += $"{takeWhileInt}\r\n";
                                }
                            }
                        }
                        else if (myType.FullName.Contains("Lookup") && myType.FullName.Contains("Int32"))
                        {
                            var lookupInts = (Lookup<int, string>)returnValue;
                            if (lookupInts.Count() > 0)
                            {
                                foreach (var lookupKeys in lookupInts)
                                {
                                    foreach (var lookupKey in lookupKeys)
                                    {
                                        tempResults += $"{lookupKey}\r\n";
                                    }
                                }
                            }
                        }
                        else if (myType.FullName.Contains("Dictionary") && myType.FullName.Contains("Int32"))
                        {
                            var dictionaryInts = (IDictionary<int, string>)returnValue;
                            if (dictionaryInts.Count() > 0)
                            {
                                foreach (var dictionaryInt in dictionaryInts)
                                {
                                    tempResults += $"{dictionaryInt}\r\n";
                                }
                            }
                        }
                        else if (myType.FullName.Contains("Dictionary") && myType.FullName.Contains("String"))
                        {
                            var dictionaryStrings = (IDictionary<string, string>)returnValue;
                            if (dictionaryStrings.Count() > 0)
                            {
                                foreach (var dictionaryString in dictionaryStrings)
                                {
                                    tempResults += $"{dictionaryString}\r\n";
                                }
                            }
                        }
                        else if (myType.Name.Contains("GroupedEnumerable"))
                        {
                            var groupedEnums = (IEnumerable<object>)returnValue;
                            if (groupedEnums.Count() > 0)
                            {
                                foreach (var groupedEnum in groupedEnums)
                                {
                                    tempResults += $"{groupedEnum}\r\n";
                                }
                            }
                        }
                        else
                        {
                            tempResults += $"Selected LINQ Query is not supported yet!\r\n{myType}";
                        }
                        if (tempResults.EndsWith("\r\n"))
                        {
                            tempResults = tempResults.Substring(0, tempResults.Length - "\r\n".Length);
                        }
                        tempResults = tempResults.Trim();
                        if (tempResults.Contains("\r\n"))
                        {
                            LinqPadResults.Children.Add(queryResultEquals);
                            queryResults = new() { Text = $"{tempResults}", Foreground = Brushes.Yellow, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                        }
                        else
                        {
                            queryResults = new() { Text = $"{Constants.LinqQueryEquals} {tempResults}", Foreground = Brushes.Yellow, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                        }
                        LinqPadResults.Children.Add(queryResults);
                    }
                }
                catch (Exception ex)
                {
                    exceptionResult = new() { Text = $"{ex.Message}\r\n", Foreground = Brushes.Red, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(exceptionResult);
                    exceptionAdditionMsg = new() { Text = $"{Constants.ExceptionAdditionMessage}", Foreground = Brushes.Red, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(exceptionAdditionMsg);
                }
            }).FireAndForget();

        }
        public async Task RunEditorLinqQueryAsync(LinqType linqType)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                LinqPadResults.Children.Clear();
                string modifiedSelection = string.Empty;

                DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
                _activeFile = docView?.Document?.FilePath;
                _activeProject = await VS.Solutions.GetActiveProjectAsync();
                _myNamespace = _activeProject.Name;
                TextBlock runningQueryResult = null;
                TextBlock NothingSelectedResult = null;
                TextBlock exceptionResult = null;
                Line line = new() { Margin = new Thickness(0, 0, 0, 20) };
                if (docView?.TextView == null)
                {
                    NothingSelectedResult = new() { Text = Constants.NoActiveDocument, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(NothingSelectedResult);
                    return;
                }
                if (docView.TextView.Selection != null && !docView.TextView.Selection.IsEmpty)
                {
                    runningQueryResult = new() { Text = $"{Constants.RunningSelectQuery}\r\n", Foreground = Brushes.LightBlue, FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(runningQueryResult);
                    string currentSelection = null;
                    string tempQueryPath = null;
                    try
                    {
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        currentSelection = docView.TextView.Selection.StreamSelectionSpan.GetText().Trim().Replace("  ", "").Trim();
                        int position = 0;
                        switch (linqType)
                        {
                            case LinqType.Method:
                                CurrentLinqMode = LinqType.Method;
                                if (LinqAdvancedOptions.Instance.EnableToolWindowResults == true)
                                {
                                    if (currentSelection.Contains("private")
                                    || currentSelection.Contains("public")
                                    || currentSelection.Contains("static")
                                    || currentSelection.Contains("async")
                                    || currentSelection.Contains("Task")
                                    || currentSelection.Contains("void"))
                                    {
                                        int firstIndexReturnNewLine = currentSelection.IndexOf("{");
                                        string firstLineSelection = currentSelection.Substring(0, firstIndexReturnNewLine + 1);
                                        modifiedSelection = currentSelection.Remove(0, firstLineSelection.Length);
                                        int lastIndexBrace = modifiedSelection.LastIndexOf("}");
                                        modifiedSelection = modifiedSelection.Substring(0, lastIndexBrace);
                                        if (modifiedSelection.EndsWith("\r\n}\r\n}\r\n"))
                                        {
                                            modifiedSelection = modifiedSelection.Substring(0, modifiedSelection.Length - "\r\n}\r\n}\r\n".Length);
                                        }
                                        if (modifiedSelection.EndsWith("\r\n}\r\n"))
                                        {
                                            modifiedSelection = modifiedSelection.Substring(0, modifiedSelection.Length - "\r\n}\r\n".Length);
                                        }
                                        if (modifiedSelection.EndsWith("\r\n\t\t}\r\n\t"))
                                        {
                                            modifiedSelection = modifiedSelection.Substring(0, modifiedSelection.Length - "\r\n\t\t}\r\n\t".Length);
                                        }
                                        if (modifiedSelection.EndsWith("\r\n\t\t"))
                                        {
                                            modifiedSelection = modifiedSelection.Substring(0, modifiedSelection.Length - "\r\n\t\t".Length);
                                        }
                                        if (modifiedSelection.EndsWith("\r\n"))
                                            if (modifiedSelection.EndsWith("\r\n\t\t}"))
                                            {
                                                modifiedSelection = modifiedSelection.Substring(0, modifiedSelection.Length - "\r\n\t\t}".Length);
                                            }
                                        if (modifiedSelection.EndsWith("\r\n"))
                                        {
                                            modifiedSelection = modifiedSelection.Substring(0, modifiedSelection.Length - "\r\n".Length);
                                        }
                                        if (modifiedSelection.StartsWith("\r\n"))
                                        {
                                            modifiedSelection = modifiedSelection.Substring("\r\n".Length, modifiedSelection.Length - "\r\n".Length);
                                        }
                                        modifiedSelection = modifiedSelection.Trim();
                                        CurrentLinqMode = LinqType.Method;
                                    }
                                    else
                                    {
                                        CurrentLinqMode = LinqType.Statement;
                                        modifiedSelection = currentSelection;
                                    }
                                    await RunLinqQueriesAsync(modifiedSelection);
                                }
                                if (CurrentLinqMode == LinqType.Statement)
                                {
                                    tempQueryPath = $"{Path.GetTempFileName()}{Constants.LinqExt}";
                                    File.WriteAllText(tempQueryPath, $"{modifiedSelection}");
                                }
                                else if (CurrentLinqMode == LinqType.Method)
                                {
                                    tempQueryPath = $"{Path.GetTempFileName()}{Constants.LinqExt}";
                                    File.WriteAllText(tempQueryPath, $"{modifiedSelection}");
                                }
                                break;
                            case LinqType.None:
                                CurrentLinqMode = LinqType.None;
                                NothingSelectedResult = new() { Text = Constants.NoActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                LinqPadResults.Children.Add(NothingSelectedResult);
                                return;
                            default:
                                NothingSelectedResult = new() { Text = Constants.NoActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                LinqPadResults.Children.Add(NothingSelectedResult);
                                return;
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
                    }
                }
                else
                {
                    NothingSelectedResult = new() { Text = Constants.NoActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(NothingSelectedResult);
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
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
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
    }
}
