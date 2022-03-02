using System.Collections.Generic;
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

        public async Task RunEditorLinqQueryAsync(LinqType linqType)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                LinqPadResults.Children.Clear();
                DocumentView docView = await VS.Documents.GetActiveDocumentViewAsync();
                _activeFile = docView?.Document?.FilePath;
                _activeProject = await VS.Solutions.GetActiveProjectAsync();
                _myNamespace = _activeProject.Name;
                TextBlock runningQueryResult = null;
                TextBlock NothingSelectedResult = null;
                TextBlock selectedQueryResult = null;
                TextBlock exceptionResult = null;
                TextBlock queryResultMsg = null;
                TextBlock queryResults = null;
                TextBlock queryResultEquals = new() { Text = Constants.LinqQueryEquals, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                TextBlock queryCodeHeader = new() { Text = Constants.LinqQueryTextHeader, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };

                Line line = new() { Margin = new Thickness(0, 0, 0, 20) };
                if (docView?.TextView == null)
                {
                    NothingSelectedResult = new() { Text = Constants.NoActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                    LinqPadResults.Children.Add(NothingSelectedResult);
                    return;
                }
                if (docView.TextView.Selection != null && !docView.TextView.Selection.IsEmpty)
                {
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
                                return;
                            default:
                                NothingSelectedResult = new() { Text = Constants.NoActiveDocument, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                LinqPadResults.Children.Add(NothingSelectedResult);
                                return;
                        }
                        if (LinqAdvancedOptions.Instance.EnableToolWindowResults == true)
                        {
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
                            var allVariables = result.Variables;
                            var variable = allVariables.Where(n => n.Name == Constants.LinqResultText);

                            if (variable.First().Name == Constants.LinqResultText)
                            {
                                var returnValue = result.GetVariable(Constants.LinqResultText).Value;
                                var myType = returnValue.GetType();
                                queryResultMsg = new() { Text = $"{result.Script.Code}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                LinqPadResults.Children.Add(queryCodeHeader);
                                LinqPadResults.Children.Add(line);
                                LinqPadResults.Children.Add(queryResultMsg);
                                LinqPadResults.Children.Add(line);
                                if (myType == typeof(int) || myType == typeof(string) || myType == typeof(bool) || myType == typeof(float) || myType == typeof(double))
                                {
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    queryResults = new() { Text = $"{result.GetVariable(Constants.LinqResultText).Value}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                    LinqPadResults.Children.Add(queryResults);
                                    LinqPadResults.Children.Add(line);
                                }
                                else if (myType == typeof(string[]))
                                {
                                    var stringArrays = (string[])result.GetVariable(variable.First().Name).Value;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (stringArrays.Length > 0)
                                    {
                                        foreach (var stringArray in stringArrays)
                                        {
                                            queryResults = new() { Text = $"{stringArray}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType == typeof(int[]))
                                {
                                    var intArrays = (int[])result.GetVariable(variable.First().Name).Value;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (intArrays.Length > 0)
                                    {
                                        foreach (var intArray in intArrays)
                                        {
                                            queryResults = new() { Text = $"{intArray}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType == typeof(List<string>))
                                {
                                    var listStrings = (List<string>)result.GetVariable(variable.First().Name).Value;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (listStrings.Count() > 0)
                                    {
                                        foreach (var listString in listStrings)
                                        {
                                            queryResults = new() { Text = $"{listString}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType == typeof(IEnumerable<double>) || (myType.FullName.Contains("OfTypeIterator") && myType.FullName.Contains("Double")))
                                {
                                    IEnumerable<double> enumDoubles = (IEnumerable<double>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (enumDoubles.Count() > 0)
                                    {
                                        foreach (var enumDouble in enumDoubles)
                                        {
                                            queryResults = new() { Text = $"{enumDouble}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType == typeof(Int64))
                                {
                                    var int64 = (Int64)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    queryResults = new() { Text = $"{int64}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                    LinqPadResults.Children.Add(queryResults);
                                    LinqPadResults.Children.Add(line);
                                }
                                else if (myType == typeof(IEnumerable<float>))
                                {
                                    IEnumerable<float> enumFloats = (IEnumerable<float>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (enumFloats.Count() > 0)
                                    {
                                        foreach (var enumFloat in enumFloats)
                                        {
                                            queryResults = new() { Text = $"{enumFloat}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType.FullName.Contains("SelectArrayIterator") && myType.FullName.Contains("Decimal"))
                                {
                                    IEnumerable<decimal> enumDecimals = (IEnumerable<decimal>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (enumDecimals.Count() > 0)
                                    {
                                        foreach (var enumDecimal in enumDecimals)
                                        {
                                            queryResults = new() { Text = $"{enumDecimal}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType.FullName.Contains("OrderedEnumerable") && myType.FullName.Contains("DateTime")
                                || myType == typeof(IEnumerable<DateTime>))
                                {
                                    IOrderedEnumerable<DateTime> orderedDates = (IOrderedEnumerable<DateTime>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (orderedDates.Count() > 0)
                                    {
                                        foreach (var orderedDate in orderedDates)
                                        {
                                            queryResults = new() { Text = $"{orderedDate}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType.FullName.Contains("OrderedEnumerable") && myType.FullName.Contains("Submission")
                                || myType.FullName.Contains("GroupJoinIterator") && myType.FullName.Contains("Submission")
                                || myType.FullName.Contains("SelectManyIterator") && myType.FullName.Contains("String")
                                || myType.FullName.Contains("SelectIterator") && myType.FullName.Contains("String")
                                || myType.FullName.Contains("SkipWhileIterator") && myType.FullName.Contains("String")
                                || myType.FullName.Contains("OfTypeIterator") && myType.FullName.Contains("String")
                                || myType.FullName.Contains("Concat2Iterator") && myType.FullName.Contains("String")
                                || myType.FullName.Contains("RepeatIterator") && myType.FullName.Contains("String")
                                || myType.FullName.Contains("SelectArrayIterator") && myType.FullName.Contains("Double")
                                || myType.FullName.Contains("WhereArrayIterator") && myType.FullName.Contains("Submission"))
                                {
                                    var orderedObjects = (IEnumerable<object>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (orderedObjects.Count() > 0)
                                    {
                                        foreach (var orderedString in orderedObjects)
                                        {
                                            queryResults = new() { Text = $"{orderedString}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType.FullName.Contains("OrderedEnumerable") && myType.FullName.Contains("Int32"))
                                {
                                    IOrderedEnumerable<int> orderedInts = (IOrderedEnumerable<int>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (orderedInts.Count() > 0)
                                    {
                                        foreach (var orderedInt in orderedInts)
                                        {
                                            queryResults = new() { Text = $"{orderedInt}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType.FullName.Contains("OrderedEnumerable") && myType.FullName.Contains("String"))
                                {
                                    IOrderedEnumerable<string> orderedStrings = (IOrderedEnumerable<string>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (orderedStrings.Count() > 0)
                                    {
                                        foreach (var orderedString in orderedStrings)
                                        {
                                            queryResults = new() { Text = $"{orderedString}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType.FullName.Contains("JoinIterator") && myType.FullName.Contains("String")
                                || myType.FullName.Contains("ListPartition") && myType.FullName.Contains("String")
                                || myType.FullName.Contains("EmptyPartition") && myType.FullName.Contains("String")
                                || myType == typeof(IEnumerable<string>))
                                {
                                    var repeatObjects = (IEnumerable<string>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (repeatObjects.Count() > 0)
                                    {
                                        foreach (var repeatObject in repeatObjects)
                                        {
                                            queryResults = new() { Text = $"{repeatObject}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType.FullName.Contains("ReverseIterator") && myType.FullName.Contains("Char"))
                                {
                                    var reverseChars = (IEnumerable<char>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (reverseChars.Count() > 0)
                                    {
                                        foreach (var reverseChar in reverseChars)
                                        {
                                            queryResults = new() { Text = $"{reverseChar}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType.FullName.Contains("TakeWhileIterator") && myType.FullName.Contains("Int32")
                                || myType.FullName.Contains("OfTypeIterator") && myType.FullName.Contains("Int32")
                                || myType.FullName.Contains("UnionIterator") && myType.FullName.Contains("Int32")
                                || myType.FullName.Contains("IntersectIterator") && myType.FullName.Contains("Int32")
                                || myType.FullName.Contains("WhereIterator") && myType.FullName.Contains("Int32")
                                || myType.FullName.Contains("WhereArrayIterator") && myType.FullName.Contains("Int32")
                                || myType.FullName.Contains("ZipIterator") && myType.FullName.Contains("Int32")
                                || myType.FullName.Contains("ExceptIterator") && myType.FullName.Contains("Int32")
                                || myType.FullName.Contains("DefaultIfEmptyIterator") && myType.FullName.Contains("Int32")
                                || myType.FullName.Contains("ListPartition") && myType.FullName.Contains("Int32")
                                || myType.Name.Contains("RangeIterator")
                                || myType == typeof(IEnumerable<int>))
                                {
                                    var takeWhileInts = (IEnumerable<int>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (takeWhileInts.Count() > 0)
                                    {
                                        foreach (var takeWhileInt in takeWhileInts)
                                        {
                                            queryResults = new() { Text = $"{takeWhileInt}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType.FullName.Contains("Lookup") && myType.FullName.Contains("Int32"))
                                {
                                    var lookupInts = (Lookup<int, string>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (lookupInts.Count() > 0)
                                    {
                                        foreach (var lookupKeys in lookupInts)
                                        {
                                            foreach (var lookupKey in lookupKeys)
                                            {
                                                queryResults = new() { Text = $"{lookupKey}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                                LinqPadResults.Children.Add(queryResults);
                                                LinqPadResults.Children.Add(line);
                                            }
                                        }
                                    }
                                }
                                else if (myType.FullName.Contains("Dictionary") && myType.FullName.Contains("String"))
                                {
                                    var takeWhileInts = (IDictionary<string, string>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (takeWhileInts.Count() > 0)
                                    {
                                        foreach (var takeWhileInt in takeWhileInts)
                                        {
                                            queryResults = new() { Text = $"{takeWhileInt}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else if (myType.Name.Contains("GroupedEnumerable"))
                                {
                                    var groupedEnums = (IGrouping<int, bool>)returnValue;
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    if (groupedEnums.Count() > 0)
                                    {
                                        foreach (var groupedEnum in groupedEnums)
                                        {
                                            queryResults = new() { Text = $"{groupedEnum}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                            LinqPadResults.Children.Add(queryResults);
                                            LinqPadResults.Children.Add(line);
                                        }
                                    }
                                }
                                else
                                {
                                    LinqPadResults.Children.Add(queryResultEquals);
                                    LinqPadResults.Children.Add(line);
                                    queryResults = new() { Text = $"{myType}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                                    LinqPadResults.Children.Add(queryResults);
                                    LinqPadResults.Children.Add(line);
                                }
                            }
                            ////LinqPadResults.Children.Clear();
                            //selectedQueryResult = new() { Text = $"{currentSelection}\r\n\r\nresult = {queryResult}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                            ////selectedQueryResult = new() { Text = $"{currentSelection} \r\n\r\n{Constants.CurrentSelectionQuery} = \r\n\r\n{queryResult}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                            ////selectedQueryResult = new() { Text = $"{Constants.CurrentSelectionQuery} = \r\n\r\n{queryResult}", TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) };
                            //LinqPadResults.Children.Add(selectedQueryResult);
                            //LinqPadResults.Children.Add(line);
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
    }
}
