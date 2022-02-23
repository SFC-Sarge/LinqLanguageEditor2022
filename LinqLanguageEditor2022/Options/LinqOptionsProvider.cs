using System.Runtime.InteropServices;

namespace LinqLanguageEditor2022.Options
{
    internal partial class LinqOptionsProvider
    {
        [ComVisible(true)]
        public class LinqAdvancedOptions : BaseOptionPage<Options.LinqAdvancedOptions> { }

        [ComVisible(true)]
        public class LinqCodeStyleOptions : BaseOptionPage<Options.LinqCodeStyleOptions> { }

        [ComVisible(true)]
        public class LinqIntelliSenseOptions : BaseOptionPage<Options.LinqIntelliSenseOptions> { }

    }
}
