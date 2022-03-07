using System.ComponentModel;
using System.Windows.Media;

using Microsoft.VisualStudio.Settings;

namespace LinqLanguageEditor2022.Options
{

    public class LinqAdvancedOptions : BaseOptionModel<LinqAdvancedOptions>
    {
        [Category(Constants.OptionCategoryResults)]
        [DisplayName(Constants.LinqResultMessageText)]
        [Description(Constants.LinqResultMessageText)]
        [DefaultValue(Constants.LinqResultText)]
        public string LinqResultText { get; set; }

        [Category(Constants.OptionCategoryResults)]
        [DisplayName(Constants.LinqEditorOpenPreviewTabMessage)]
        [Description(Constants.LinqEditorOpenPreviewTabMessage)]
        [DefaultValue(Constants.OpenInVSPreviewTab)]
        public bool OpenInVSPreviewTab { get; set; }

        [Category(Constants.OptionCategoryResults)]
        [DisplayName(Constants.LinqEditorResultsInToolWindow)]
        [Description(Constants.LinqEditorResultsInToolWindow)]
        [DefaultValue(Constants.EnableToolWindowResults)]
        public bool EnableToolWindowResults { get; set; }

        [Category(Constants.OptionCategoryResults)]
        [DisplayName(Constants.RunningSelectQueryMsgColor)]
        [Description(Constants.RunningSelectQueryMsgColor)]
        [DefaultValue(Constants.LinqRunningSelectQueryMsgColor)]
        public string LinqRunningSelectQueryMsgColor { get; set; }

        [Category(Constants.OptionCategoryResults)]
        [DisplayName(Constants.ResultsCodeTextColor)]
        [Description(Constants.ResultsCodeTextColor)]
        [DefaultValue(Constants.LinqCodeResultsColor)]
        public string LinqCodeResultsColor { get; set; }

        [Category(Constants.OptionCategoryResults)]
        [DisplayName(Constants.QueryEqualsMsgColor)]
        [Description(Constants.QueryEqualsMsgColor)]
        [DefaultValue(Constants.LinqResultsEqualMsgColor)]
        public string LinqResultsEqualMsgColor { get; set; }

        [Category(Constants.OptionCategoryResults)]
        [DisplayName(Constants.ResultColor)]
        [Description(Constants.ResultColor)]
        [DefaultValue(Constants.LinqResultsColor)]
        public string LinqResultsColor { get; set; }

        [Category(Constants.OptionCategoryResults)]
        [DisplayName(Constants.ExceptionAdditionMsgColor)]
        [Description(Constants.ExceptionAdditionMsgColor)]
        [DefaultValue(Constants.LinqExceptionAdditionMsgColor)]
        public string LinqExceptionAdditionMsgColor { get; set; }
    }
}
