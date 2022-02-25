using System.Runtime.InteropServices;
using System.Windows;

namespace LinqLanguageEditor2022.Options
{
    [ComVisible(true)]
    [Guid("1E8D4998-0738-4C6B-A391-4F7729D7E96D")]
    public class IntelliSenseOptionPage : UIElementDialogPage
    {
        protected override UIElement Child
        {
            get
            {
                IntelliSenseOptions page = new IntelliSenseOptions
                {
                    intelliSenseOptionsPage = this
                };
                page.Initialize();
                return page;
            }
        }
    }
}
