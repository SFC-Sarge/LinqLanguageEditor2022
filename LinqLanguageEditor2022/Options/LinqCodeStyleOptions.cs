using System.ComponentModel;

namespace LinqLanguageEditor2022.Options
{

    public class LinqCodeStyleOptions : BaseOptionModel<LinqCodeStyleOptions>
    {
        [Category(@"General")]
        [DisplayName("Automatically format when typing")]
        [Description("Automatically format when typing.")]
        [DefaultValue(true)]
        public bool AutoFormatWhenTyping { get; set; } = true;

        [Category(@"General")]
        [DisplayName("Automatically format statement on ;")]
        [Description("Automatically format statement on ;.")]
        [DefaultValue(true)]
        public bool AutoFormatStatementOn { get; set; } = true;

        [Category(@"General")]
        [DisplayName("Automatically format statement on }")]
        [Description("Automatically format statement on }.")]
        [DefaultValue(true)]
        public bool AutoFormatBlockOn { get; set; } = true;

        [Category(@"General")]
        [DisplayName("Automatically format on return")]
        [Description("Automatically format on return.")]
        [DefaultValue(true)]
        public bool AutoFormatOnReturn { get; set; } = true;

        [Category("Indentation")]
        [DisplayName("Indent block contents")]
        [Description("Indent block contents.")]
        [DefaultValue(true)]
        public bool IndentBlockContents { get; set; } = true;

        [Category("Indentation")]
        [DisplayName("Indent open and close braces")]
        [Description("Indent open and close braces.")]
        [DefaultValue(true)]
        public bool IndentOpenCloseBraces { get; set; } = true;

        [Category("Indentation")]
        [DisplayName("Indent case contents")]
        [Description("Indent case contents.")]
        [DefaultValue(true)]
        public bool IndentCaseContents { get; set; } = true;

        [Category("Indentation")]
        [DisplayName("Indent case contents (when block)")]
        [Description("Indent case contents (when block).")]
        [DefaultValue(true)]
        public bool IndentCaseContentsInBlock { get; set; } = true;

        [Category("Indentation")]
        [DisplayName("Indent case labels")]
        [Description("Indent caselabels.")]
        [DefaultValue(true)]
        public bool IndentCaseLabels { get; set; } = true;

        [Category("New Lines")]
        [DisplayName("Place open brace on new line for types")]
        [Description("Place open brace on new line for types.")]
        [DefaultValue(true)]
        public bool OpenBraceOnNewLineForTypes { get; set; } = true;

        [Category("New Lines")]
        [DisplayName("Place open brace on new line for methods and local functions")]
        [Description("Place open brace on new line for methods and local functions.")]
        [DefaultValue(true)]
        public bool OpenBraceOnNewLineForMethods { get; set; } = true;

        [Category("Spacing")]
        [DisplayName("Insert space between method name and its opening parenthesis")]
        [Description("Insert space between method name and its opening parenthesis.")]
        [DefaultValue(true)]
        public bool InsertSpaceBetweenMethodNameOpenParenthesis { get; set; } = true;

        [Category("Spacing")]
        [DisplayName("Insert space within parameter list parentheses")]
        [Description("Insert space within parameter list parentheses.")]
        [DefaultValue(true)]
        public bool InsertSpaceInParameterlistParentheses { get; set; } = true;

        [Category("Wrapping")]
        [DisplayName("Leave block on single line")]
        [Description("Leave block on single line.")]
        [DefaultValue(true)]
        public bool LeaveBlockOnSingleLine { get; set; } = true;

        [Category("Wrapping")]
        [DisplayName("Leave statements and member declarations on the same line")]
        [Description("Leave statements and member declarations on the same line.")]
        [DefaultValue(true)]
        public bool LeaveStatementMemberDeclareOnSameLine { get; set; } = true;
    }
}
