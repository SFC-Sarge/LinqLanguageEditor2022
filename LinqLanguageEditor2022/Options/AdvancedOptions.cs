using System.ComponentModel;
using System.Runtime.InteropServices;

namespace LinqLanguageEditor2022.Options
{
    internal partial class OptionsProvider
    {
        [ComVisible(true)]
        public class AdvancedOptions : BaseOptionPage<Options.AdvancedOptions> { }
    }

    public class AdvancedOptions : BaseOptionModel<AdvancedOptions>
    {
        [Category("Results")]
        [DisplayName("Open Linq Query and result in Visual Studio Preview Tab")]
        [Description("Determines if the VS preview tab window should render the Linq query and results.")]
        [DefaultValue(true)]
        public bool OpenInVSPreviewTab { get; set; } = true;

        [Category("Results")]
        [DisplayName("Enable LinqPad Dump Windows for Linq Query and results")]
        [Description("Determines if the LinqPad Dump Windows is enabled and displays the Linq query and results.")]
        [DefaultValue(true)]
        public bool UseLinqPadDumpWindow { get; set; } = false;

        [Category("Results")]
        [DisplayName("Enable Tool Window for Linq Query and results")]
        [Description("Determines if the Tool Window is enabled and displays the Linq query and results.")]
        [DefaultValue(true)]
        public bool EnableToolWindowResults { get; set; } = false;

    }
}
