using System.Runtime.InteropServices;
using System.Windows;

namespace LinqLanguageEditor2022.Options
{
    [ComVisible(true)]
    [Guid("05F5CC22-0DF4-4D38-9B25-F54AAF567201")]
    public class LinqAdvancedOptionPage : UIElementDialogPage
    {
        protected override UIElement Child
        {
            get
            {
                AdvancedOptions page = new AdvancedOptions
                {
                    advancedOptionsPage = this
                };
                page.Initialize();
                return page;
            }
        }
    }
}
