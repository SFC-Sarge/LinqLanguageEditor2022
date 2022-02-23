using System.ComponentModel;

namespace LinqLanguageEditor2022.Options
{

    public class LinqIntelliSenseOptions : BaseOptionModel<LinqIntelliSenseOptions>
    {
        [Category("IntelliSense")]
        [DisplayName("Show completion list after a character is typed")]
        [Description("Show completion list after a character is typed.")]
        [DefaultValue(true)]
        public bool ShowCompletionListAfterCharTyped { get; set; } = true;

        [Category("IntelliSense")]
        [DisplayName("Show completion list after a character is deleted")]
        [Description("Show completion list after a character is deleted.")]
        [DefaultValue(true)]
        public bool ShowCompletionListAfterCharDeleted { get; set; } = true;

        [Category("IntelliSense")]
        [DisplayName("Automatically show completion list in argument lists")]
        [Description("Automatically show completion list in argument lists.")]
        [DefaultValue(true)]
        public bool AutoShowCompletionListInArgumentList { get; set; } = true;

    }
}
