using System.Runtime.InteropServices;
using System.Windows;

namespace LinqLanguageEditor2022.Options
{
    [ComVisible(true)]
    [Guid(Constants.LinqAdvancedOptionPageGuid)]
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
