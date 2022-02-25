using System.Runtime.InteropServices;
using System.Windows;

namespace LinqLanguageEditor2022.Options
{
    [ComVisible(true)]
    [Guid("4756C29F-40F5-48A6-B323-904CF4F4B594")]

    public class CodeStyleIndentationOptionPage : UIElementDialogPage
    {
        protected override UIElement Child
        {
            get
            {
                CodeStyleIndentationOptions page = new CodeStyleIndentationOptions
                {
                    indentationOptionsPage = this
                };
                page.Initialize();
                return page;
            }
        }
    }
}
