﻿using System.Runtime.InteropServices;
using System.Windows;

using static LinqLanguageEditor2022.Options.LinqOptionsProvider;

namespace LinqLanguageEditor2022.Options
{
    [ComVisible(true)]
    [Guid("D8B47497-8AC9-4E2E-9D62-D8E8E7A47AA4")]

    public class GeneralOptionPage : UIElementDialogPage
    {
        protected override UIElement Child
        {
            get
            {
                GeneralOptions page = new GeneralOptions
                {
                    generalOptionsPage = this
                };
                page.Initialize();
                return page;
            }
        }
    }
}
