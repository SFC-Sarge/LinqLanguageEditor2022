using System.Runtime.InteropServices;
using System.Windows;

namespace LinqLanguageEditor2022.Options
{
    [ComVisible(true)]
    [Guid("93C8EFAD-2DDB-4E07-B46A-86954F680A93")]

    public class CodeStyleNewLineOptionPage : UIElementDialogPage
    {
        protected override UIElement Child
        {
            get
            {
                CodeStyleNewLineOptions page = new CodeStyleNewLineOptions
                {
                    newLineOptionsPage = this
                };
                page.Initialize();
                return page;
            }
        }
    }
}
