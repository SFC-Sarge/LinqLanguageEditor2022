using System.Runtime.InteropServices;
using System.Windows;

namespace LinqLanguageEditor2022.Options
{
    [ComVisible(true)]
    [Guid("FE4E4122-3DA7-4521-9D19-FB9A8531CF94")]

    public class CodeStyleSpacingOptionPage : UIElementDialogPage
    {
        protected override UIElement Child
        {
            get
            {
                CodeStyleSpacingOptions page = new CodeStyleSpacingOptions
                {
                    spacingOptionsPage = this
                };
                page.Initialize();
                return page;
            }
        }
    }
}
