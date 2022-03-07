using System.ComponentModel;
using System.Windows.Media;

using Microsoft.VisualStudio.Settings;

namespace LinqLanguageEditor2022.Options
{

    public class LinqAdvancedOptions : BaseOptionModel<LinqAdvancedOptions>
    {
        [Category("Results")]
        [DisplayName("Open Linq Query and result in Visual Studio Preview Tab")]
        [Description("Determines if the VS preview tab window should render the Linq query and results.")]
        [DefaultValue(true)]
        public bool OpenInVSPreviewTab { get; set; }

        [Category("Results")]
        [DisplayName("Enable Tool Window for Linq Query and results")]
        [Description("Determines if the Tool Window is enabled and displays the Linq query and results.")]
        [DefaultValue(true)]
        public bool EnableToolWindowResults { get; set; }

        [Category("Results")]
        [DisplayName("Select LINQ Results Code Color")]
        [Description("Select LINQ Results Code Color.")]
        [DefaultValue("LightGreen")]

        public string LinqCodeResultsColor { get; set; }

        [Category("Results")]
        [DisplayName("Select LINQ Results Text Color")]
        [Description("Select LINQ Results Text Color.")]
        [DefaultValue("Yellow")]
        public string LinqResultsColor { get; set; }

        [Category("Results")]
        [DisplayName("Select LINQ Results Error Text Color")]
        [Description("Select LINQ Results Error Text Color.")]
        [DefaultValue("Red")]
        public string LinqExceptionAdditionMsgColor { get; set; }

        [Category("Results")]
        [DisplayName("Running Select Query Method Message Color")]
        [Description("Running Select Query Method Message Color.")]
        [DefaultValue("LightBlue")]
        public string LinqResultsEqualMsgColor { get; set; }

        [Category("Results")]
        [DisplayName("Running Select Query Method Message Color")]
        [Description("Running Select Query Method Message Color.")]
        [DefaultValue("LightBlue")]
        public string LinqRunningSelectQueryMsgColor { get; set; }
    }
}
