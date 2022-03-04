using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;

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
            cmbResultCodeColor.ItemsSource = typeof(Brushes).GetProperties();
            cmbResultColor.ItemsSource = typeof(Brushes).GetProperties();
            cmbRunningQueryMsgColor.ItemsSource = typeof(Brushes).GetProperties();
            cmbExceptionAdditionMsgColor.ItemsSource = typeof(Brushes).GetProperties();
            cmbResultsEqualMsgColor.ItemsSource = typeof(Brushes).GetProperties();
            advanceOptionText.Text = Constants.AdvanceOptionText;
            tbResultCodeColor.Text = Constants.ResultsCodeTextColor;
            tbResultColor.Text = Constants.ResultColor;
            tbResultsEqualMsgColor.Text = Constants.QueryEqualsMsgColor;
            tbRunningQueryMsgColor.Text = Constants.RunningSelectQueryMsgColor;
            tbExceptionAdditionMsgColor.Text = Constants.ExceptionAdditionMsgColor;


            tbExceptionAdditionMsgColor.Text = Constants.ExceptionAdditionMsgColor;
            if (LinqAdvancedOptions.Instance.LinqCodeResultsColor == null)
            {
                LinqAdvancedOptions.Instance.LinqCodeResultsColor = new SolidColorBrush(Colors.LightGreen);
                LinqAdvancedOptions.Instance.Save();
            }
            if (LinqAdvancedOptions.Instance.LinqResultsColor == null)
            {
                LinqAdvancedOptions.Instance.LinqResultsColor = new SolidColorBrush(Colors.Yellow);
                LinqAdvancedOptions.Instance.Save();
            }
            if (LinqAdvancedOptions.Instance.LinqResultsEqualMsgColor == null)
            {
                LinqAdvancedOptions.Instance.LinqResultsEqualMsgColor = new SolidColorBrush(Colors.LightBlue);
                LinqAdvancedOptions.Instance.Save();
            }
            if (LinqAdvancedOptions.Instance.LinqRunningSelectQueryMsgColor == null)
            {
                LinqAdvancedOptions.Instance.LinqRunningSelectQueryMsgColor = new SolidColorBrush(Colors.LightBlue);
                LinqAdvancedOptions.Instance.Save();
            }
            if (LinqAdvancedOptions.Instance.LinqExceptionAdditionMsgColor == null)
            {
                LinqAdvancedOptions.Instance.LinqExceptionAdditionMsgColor = new SolidColorBrush(Colors.Red);
                LinqAdvancedOptions.Instance.Save();
            }
            cbOpenInVSPreviewTab.IsChecked = LinqAdvancedOptions.Instance.OpenInVSPreviewTab;
            cbEnableToolWindowResults.IsChecked = LinqAdvancedOptions.Instance.EnableToolWindowResults;
            cmbResultCodeColor.SelectedItem = LinqAdvancedOptions.Instance.LinqCodeResultsColor;
            cmbResultColor.SelectedItem = LinqAdvancedOptions.Instance.LinqResultsColor;
            cmbResultsEqualMsgColor.SelectedItem = LinqAdvancedOptions.Instance.LinqResultsEqualMsgColor;
            cmbRunningQueryMsgColor.SelectedItem = LinqAdvancedOptions.Instance.LinqRunningSelectQueryMsgColor;
            cmbExceptionAdditionMsgColor.SelectedItem = LinqAdvancedOptions.Instance.LinqExceptionAdditionMsgColor;
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

        private void cmbResultCodeColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
            LinqAdvancedOptions.Instance.LinqCodeResultsColor = selectedColor;
            LinqAdvancedOptions.Instance.Save();
        }

        private void cmbResultColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
            LinqAdvancedOptions.Instance.LinqResultsColor = selectedColor;
            LinqAdvancedOptions.Instance.Save();
        }

        private void cmbResultsEqualMsgColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
            LinqAdvancedOptions.Instance.LinqResultsEqualMsgColor = selectedColor;
            LinqAdvancedOptions.Instance.Save();
        }

        private void cmbRunningQueryMsgColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
            LinqAdvancedOptions.Instance.LinqRunningSelectQueryMsgColor = selectedColor;
            LinqAdvancedOptions.Instance.Save();
        }
        private void cmbExceptionAdditionMsgColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Brush selectedColor = (Brush)(e.AddedItems[0] as PropertyInfo).GetValue(null, null);
            LinqAdvancedOptions.Instance.LinqExceptionAdditionMsgColor = selectedColor;
            LinqAdvancedOptions.Instance.Save();
        }

    }
}
