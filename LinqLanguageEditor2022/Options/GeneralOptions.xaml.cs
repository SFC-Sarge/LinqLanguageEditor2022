using System.Windows.Controls;

namespace LinqLanguageEditor2022.Options
{
    /// <summary>
    /// Interaction logic for GeneralOptions.xaml
    /// </summary>
    public partial class GeneralOptions : UserControl
    {
        public GeneralOptions()
        {
            InitializeComponent();
        }
        internal GeneralOptionPage generalOptionsPage;

        public void Initialize()
        {
            cbMyOption.IsChecked = General.Instance.MyOption;
        }

        private void cbMyOption_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            General.Instance.MyOption = (bool)cbMyOption.IsChecked;
            General.Instance.Save();
        }

        private void cbMyOption_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            General.Instance.MyOption = (bool)cbMyOption.IsChecked;
            General.Instance.Save();
        }
    }
}
