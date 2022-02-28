using System.Windows.Controls;

namespace LinqLanguageEditor2022.Options
{
    /// <summary>
    /// Interaction logic for AdvancedOptions.xaml
    /// </summary>
    public partial class AdvancedOptions : UserControl
    {
        public AdvancedOptions()
        {
            InitializeComponent();
        }
        internal LinqAdvancedOptionPage advancedOptionsPage;

        public void Initialize()
        {
            advanceOptionText.Text = Constants.AdvanceOptionText;
            cbOpenInVSPreviewTab.IsChecked = LinqAdvancedOptions.Instance.OpenInVSPreviewTab;
            cbEnableToolWindowResults.IsChecked = LinqAdvancedOptions.Instance.EnableToolWindowResults;
            LinqAdvancedOptions.Instance.Save();
        }

        private void cbOpenInVSPreviewTab_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqAdvancedOptions.Instance.OpenInVSPreviewTab = (bool)cbOpenInVSPreviewTab.IsChecked;
            LinqAdvancedOptions.Instance.Save();
        }

        private void cbEnableToolWindowResults_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqAdvancedOptions.Instance.EnableToolWindowResults = (bool)cbEnableToolWindowResults.IsChecked;
            LinqAdvancedOptions.Instance.Save();
        }

        private void cbOpenInVSPreviewTab_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqAdvancedOptions.Instance.OpenInVSPreviewTab = (bool)cbOpenInVSPreviewTab.IsChecked;
            LinqAdvancedOptions.Instance.Save();
        }

        private void cbEnableToolWindowResults_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqAdvancedOptions.Instance.EnableToolWindowResults = (bool)cbEnableToolWindowResults.IsChecked;
            LinqAdvancedOptions.Instance.Save();
        }
    }
}
