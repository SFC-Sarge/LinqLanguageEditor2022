using System.ComponentModel;
using System.Windows.Media;

namespace LinqLanguageEditor2022.Options
{

    public class LinqAdvancedOptions : BaseOptionModel<LinqAdvancedOptions>
    {
        public LinqAdvancedOptions()
        {
            if (LinqCodeResultsColor == null)
            {
                LinqCodeResultsColor = new SolidColorBrush(Colors.LightGreen);
                Save();
            }
            if (LinqResultsColor == null)
            {
                LinqResultsColor = new SolidColorBrush(Colors.LightGreen);
                Save();
            }

        }
        [Category("Results")]
        [DisplayName("Open Linq Query and result in Visual Studio Preview Tab")]
        [Description("Determines if the VS preview tab window should render the Linq query and results.")]
        [DefaultValue(true)]
        public bool OpenInVSPreviewTab { get; set; } = true;

        [Category("Results")]
        [DisplayName("Enable Tool Window for Linq Query and results")]
        [Description("Determines if the Tool Window is enabled and displays the Linq query and results.")]
        [DefaultValue(true)]
        public bool EnableToolWindowResults { get; set; } = true;

        [Category("Results")]
        [DisplayName("Select LINQ Results Code Color")]
        [Description("Select LINQ Results Code Color.")]
        public Brush LinqCodeResultsColor { get; set; } = new SolidColorBrush(Colors.LightGreen);

        [Category("Results")]
        [DisplayName("Select LINQ Results Text Color")]
        [Description("Select LINQ Results Text Color.")]
        public Brush LinqResultsColor { get; set; } = new SolidColorBrush(Colors.Yellow);

        [Category("Results")]
        [DisplayName("Select LINQ Results Error Text Color")]
        [Description("Select LINQ Results Error Text Color.")]
        public Brush LinqExceptionAdditionMsgColor { get; set; } = new SolidColorBrush(Colors.Red);

        [Category("Results")]
        [DisplayName("Running Select Query Method Message Color")]
        [Description("Running Select Query Method Message Color.")]
        public Brush LinqResultsEqualMsgColor { get; set; } = new SolidColorBrush(Colors.LightBlue);

        [Category("Results")]
        [DisplayName("Running Select Query Method Message Color")]
        [Description("Running Select Query Method Message Color.")]
        public Brush LinqRunningSelectQueryMsgColor { get; set; } = new SolidColorBrush(Colors.LightBlue);

    }
}
