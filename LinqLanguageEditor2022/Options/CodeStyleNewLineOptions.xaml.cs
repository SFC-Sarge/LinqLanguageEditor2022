using System.Windows.Controls;

namespace LinqLanguageEditor2022.Options
{
    /// <summary>
    /// Interaction logic for CodeStyleNewLineOptions.xaml
    /// </summary>
    public partial class CodeStyleNewLineOptions : UserControl
    {
        public CodeStyleNewLineOptions()
        {
            InitializeComponent();
        }
        internal CodeStyleNewLineOptionPage newLineOptionsPage;

        public void Initialize()
        {
            cbOpenBraceOnNewLineForTypes.IsChecked = LinqCodeStyleOptions.Instance.OpenBraceOnNewLineForTypes;
            cbOpenBraceOnNewLineForMethods.IsChecked = LinqCodeStyleOptions.Instance.OpenBraceOnNewLineForMethods;
        }

        private void cbOpenBraceOnNewLineForTypes_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.OpenBraceOnNewLineForTypes = (bool)cbOpenBraceOnNewLineForTypes.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbOpenBraceOnNewLineForMethods_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.OpenBraceOnNewLineForMethods = (bool)cbOpenBraceOnNewLineForMethods.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }
    }
}
