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
            cbOpenInVSPreviewTab.IsChecked = LinqAdvancedOptions.Instance.OpenInVSPreviewTab;
            cbUseLinqPadDumpWindow.IsChecked = LinqAdvancedOptions.Instance.UseLinqPadDumpWindow;
            cbEnableToolWindowResults.IsChecked = LinqAdvancedOptions.Instance.EnableToolWindowResults;
        }

        private void cbOpenInVSPreviewTab_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqAdvancedOptions.Instance.OpenInVSPreviewTab = (bool)cbOpenInVSPreviewTab.IsChecked;
            LinqAdvancedOptions.Instance.Save();
        }

        private void cbUseLinqPadDumpWindow_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqAdvancedOptions.Instance.UseLinqPadDumpWindow = (bool)cbUseLinqPadDumpWindow.IsChecked;
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

        private void cbUseLinqPadDumpWindow_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqAdvancedOptions.Instance.UseLinqPadDumpWindow = (bool)cbUseLinqPadDumpWindow.IsChecked;
            LinqAdvancedOptions.Instance.Save();

        }

        private void cbEnableToolWindowResults_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqAdvancedOptions.Instance.EnableToolWindowResults = (bool)cbEnableToolWindowResults.IsChecked;
            LinqAdvancedOptions.Instance.Save();

        }
    }
}
