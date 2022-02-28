namespace LinqLanguageEditor2022
{
    internal class Constants
    {
        public const string LinqLanguageName = "Linq";
        public const string LinqExt = ".linq";
        public const string LinqTmpExt = ".tmp";
        public static string[] CommentChars = new[] { "///", "//" };
        public const string LinqBaselanguageName = "CSharp";
        public const string NoActiveDocument = "No Active Document View or LINQ Query Selection!\r\nPlease Select LINQ Query Statement in Active Document,\r\nthen try again!";
        public const string RunningSelectQuery = "Running Selected LINQ Query.\r\nPlease Wait!";
        public const string ResultDump = "result.Dump()";
        public const string NoActiveDocumentMethod = "No Active Document View or LINQ Query Method Selection!\r\nPlease Select LINQ Query Method in Active Document,\r\nthen try again!";
        public const string CurrentSelectionQueryMethod = "Current Selection Query Method Results";
        public const string CurrentSelectionQuery = "Current Selection Query Results";
        public const string RunningSelectQueryMethod = "Running Selected LINQ Query Method.\r\nPlease Wait!";
        public const string QueryKindStatement = "<Query Kind='Statements' />";
        public const string QueryKindMethod = "<Query Kind='Program' />";
        public const string QueryStartsWith = "<Query Kind=";
        public const string VoidMain = "void Main()";
        public const string ExceptionIn = "Exception in ";
        public const string ExceptionCall = "Call. ";
        public const string FileLPRun7Args = "-fx=6.0";
        public const string LinpPadDump = "LinqPad Dump";
        public const string PaneGuid = "A938BB26-03F8-4861-B920-6792A7D4F07C";
        public const string RunSelectedLinqStatement = "Run Selected LINQ Query Statement.";
        public const string RunSelectedLinqMethod = "Run Selected LINQ Query Method.";
        public const string RunEditorLinqQuery = "Run LINQ Query File.";
        public const string LPRun7Executable = "LPRun7-x64.exe";
        public const string LinqEditorToolWindowTitle = "LINQ Query Tool Window";
        public const string SolutionToolWindowsFolderName = "ToolWindows";
        public const string ProvideFileIcon = "KnownMonikers.RegistrationScript";
        public const string ProvideMenuResource = "Menus.ctmenu";
        public const string LinqAdvancedOptionPage = "Advanced";
        public const string AdvanceOptionText = "LINQ Language Editor Advanced Option Settings";
        public const string CodeStyleGeneralOptionPage = @"Code Style\General";
        public const string CodeStyleIndentationOptionPage = @"Code Style\Formatting\Indentation";
        public const string CodeStyleNewLineOptionPage = @"Code Style\Formatting\New lines";
        public const string CodeStyleSpacingOptionPage = @"Code Style\Formatting\Spacing";
        public const string CodeStyleWrappingOptionPage = @"Code Style\Formatting\Wrapping";
        public const string IntelliSenseOptionPage = "IntelliSense";
        public const string LinqStatementTemplate = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Diagnostics;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\nnamespace {namespace}\r\n{\r\n\tpublic class {itemname}\r\n\t{\r\n\t\tpublic static void {methodname}()\r\n\t\t{\r\n\t\t\t{$}\r\n\t\t}\r\n\t}\r\n}";
        public const string LinqMethodTemplate = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Diagnostics;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\nnamespace {namespace}\r\n{\r\n\tpublic class {itemname}\r\n\t{\r\n\t\t{$}\r\n\t}\r\n}";
    }
}
