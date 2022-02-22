namespace LinqLanguageEditor2022
{
    internal class Constants
    {
        public const string LinqLanguageName = "Linq";
        public const string LinqExt = ".linq";
        public static string[] CommentChars = new[] { "///", "//" };
        public const string LinqBaselanguageName = "CSharp";
        public const string noActiveDocument = "No Active Document View or Linq Selection!\r\nPlease Select Linq Statement in Active Document,\r\nthen try again!";
        public const string runningSelectQuery = "Running Selected Linq Query.\r\nPlease Wait!";
        public const string resultDump = "result.Dump()";
        public const string noActiveDocumentMethod = "No Active Document View or Linq Method Selection!\r\nPlease Select Linq Method in Active Document,\r\nthen try again!";
        public const string currentSelectionQueryMethod = "Current Selection Query Method Results";
        public const string currentSelectionQuery = "Current Selection Query Results";
        public const string runningSelectQueryMethod = "Running Selected Linq Query Method.\r\nPlease Wait!";
        public const string queryKindStatement = "<Query Kind='Statements' />";
        public const string queryKindMethod = "<Query Kind='Program' />";
        public const string exceptionIn = "Exception in ";
        public const string exceptionCall = "Call. ";
        public const string fileLPRun7Args = "-fx=6.0";
        public const string linpPadDump = "LinqPad Dump";
        public const string runSelectedLinqStatement = "Run Selected Linq Statement.";
        public const string runSelectedLinqMethod = "Run Selected Linq Method.";
        public const string runEditorLinqQuery = "Run Editor Linq Query.";
        public const string lPRun7Executable = "LPRun7-x64.exe";
        public const string LinqEditorToolWindowTitle = "Linq Query Tool Window";
        public const string solutionToolWindowsFolderName = "ToolWindows";
        public const string linqStatementTemplate = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Diagnostics;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\nnamespace {namespace}\r\n{\r\n\tpublic class {itemname}\r\n\t{\r\n\t\tpublic static void {methodname}()\r\n\t\t{\r\n\t\t\t{$}\r\n\t\t}\r\n\t}\r\n}";
        public const string linqMethodTemplate = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Diagnostics;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\nnamespace {namespace}\r\n{\r\n\tpublic class {itemname}\r\n\t{\r\n\t\t{$}\r\n\t}\r\n}";

    }
}