using System.Windows.Controls;

namespace LinqLanguageEditor2022.Options
{
    /// <summary>
    /// Interaction logic for CodeStyleGeneralOptions.xaml
    /// </summary>
    public partial class CodeStyleGeneralOptions : UserControl
    {
        public CodeStyleGeneralOptions()
        {
            InitializeComponent();
        }
        internal CodeStyleGeneralOptionPage newLineOptionsPage;

        public void Initialize()
        {
            cbAutoFormatWhenTyping.IsChecked = LinqCodeStyleOptions.Instance.AutoFormatWhenTyping;
            cbAutoFormatStatementOn.IsChecked = LinqCodeStyleOptions.Instance.AutoFormatStatementOn;
            cbAutoFormatBlockOn.IsChecked = LinqCodeStyleOptions.Instance.AutoFormatBlockOn;
            cbAutoFormatOnReturn.IsChecked = LinqCodeStyleOptions.Instance.AutoFormatOnReturn;
        }

        private void cbAutoFormatWhenTyping_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.AutoFormatWhenTyping = (bool)cbAutoFormatWhenTyping.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbAutoFormatStatementOn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.AutoFormatStatementOn = (bool)cbAutoFormatStatementOn.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbAutoFormatBlockOn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.AutoFormatBlockOn = (bool)cbAutoFormatBlockOn.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbAutoFormatOnReturn_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.AutoFormatOnReturn = (bool)cbAutoFormatOnReturn.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }
    }
}
