namespace LinqLanguageEditor2022
{
    internal class Constants
    {
        public const string LinqLanguageName = "Linq";
        public const string LinqExt = ".linq";
        public const string LinqTmpExt = ".tmp";
        public const string ResultsCodeTextColor = "Result Code Text Color";
        public const string ExceptionAdditionMsgColor = "Result Error Message Color";
        public const string RunningSelectQueryMsgColor = "Running Selected LINQ Query Method Message Color.";
        public const string QueryEqualsMsgColor = "Selected LINQ Query Results Equal Message Color.";
        public const string ResultColor = "Result Text Color";
        public const string LinqBaselanguageName = "CSharp";
        public const string NoActiveDocument = "No Active Document View or LINQ Query Selection!\r\nPlease Select LINQ Query Statement or Method in Active Document,\r\nthen try again!";
        public const string RunningSelectQuery = "Running Selected LINQ Query.\r\nPlease Wait!";
        public const string CurrentSelectionQueryMethod = "Current Selection Query Method Results";
        public const string RunningSelectQueryMethod = "Running Selected LINQ Query Method.\r\n\r\nPlease Wait!";
        public const string ExceptionAdditionMessage = "Try Selecting the complete LINQ Query code line or the entire LINQ Query Method code block!";
        public const string VoidMain = "void Main()";
        public const string PaneGuid = "A938BB26-03F8-4861-B920-6792A7D4F07C";
        public const string RunSelectedLinqMethod = "Run Selected LINQ Query Statement or Method.";
        public const string LinqEditorToolWindowTitle = "LINQ Query Tool Window";
        public const string SolutionToolWindowsFolderName = "ToolWindows";
        public const string ProvideFileIcon = "KnownMonikers.RegistrationScript";
        public const string ProvideMenuResource = "Menus.ctmenu";
        public const string LinqAdvancedOptionPage = "Advanced";
        public const string AdvanceOptionText = "LINQ Language Editor Advanced Option Settings";
        public const string LinqResultText = "result";
        public const string LinqQueryEquals = "Current Selected LINQ Query Results: =";
        public const string LinqQueryTextHeader = "Selected LINQ Query:";
        public const string LinqStatementTemplate = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Diagnostics;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\nnamespace {namespace}\r\n{\r\n\tpublic class {itemname}\r\n\t{\r\n\t\tpublic static void {methodname}()\r\n\t\t{\r\n\t\t\t{$}\r\n\t\t}\r\n\t}\r\n}";
        public const string LinqMethodTemplate = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Diagnostics;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\nnamespace {namespace}\r\n{\r\n\tpublic class {itemname}\r\n\t{\r\n\t\t{$}\r\n\t}\r\n}";
    }
}
