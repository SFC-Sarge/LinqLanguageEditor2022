using System.Runtime.InteropServices;
using System.Windows;

namespace LinqLanguageEditor2022.Options
{
    [ComVisible(true)]
    [Guid("4BF46AEA-E21C-4D34-B085-CA2CF0A2281F")]

    public class CodeStyleGeneralOptionPage : UIElementDialogPage
    {
        protected override UIElement Child
        {
            get
            {
                CodeStyleGeneralOptions page = new CodeStyleGeneralOptions
                {
                    newLineOptionsPage = this
                };
                page.Initialize();
                return page;
            }
        }
    }
}
