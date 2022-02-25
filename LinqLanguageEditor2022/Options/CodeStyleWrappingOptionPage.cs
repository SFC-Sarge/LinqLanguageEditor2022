using System.Runtime.InteropServices;
using System.Windows;

namespace LinqLanguageEditor2022.Options
{
    [ComVisible(true)]
    [Guid("415E857B-5361-42A6-90E9-60B1E04B80C4")]

    public class CodeStyleWrappingOptionPage : UIElementDialogPage
    {
        protected override UIElement Child
        {
            get
            {
                CodeStyleWrappingOptions page = new CodeStyleWrappingOptions
                {
                    wrappingOptionPage = this
                };
                page.Initialize();
                return page;
            }
        }
    }
}
