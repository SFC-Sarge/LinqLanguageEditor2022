using System.Windows.Controls;

namespace LinqLanguageEditor2022.Options
{
    /// <summary>
    /// Interaction logic for IntelliSenseOptions.xaml
    /// </summary>
    public partial class IntelliSenseOptions : UserControl
    {
        public IntelliSenseOptions()
        {
            InitializeComponent();
        }
        internal IntelliSenseOptionPage intelliSenseOptionsPage;

        public void Initialize()
        {
            cbShowCompletionListAfterCharTyped.IsChecked = LinqIntelliSenseOptions.Instance.ShowCompletionListAfterCharTyped;
            cbShowCompletionListAfterCharDeleted.IsChecked = LinqIntelliSenseOptions.Instance.ShowCompletionListAfterCharDeleted;
            cbAutoShowCompletionListInArgumentList.IsChecked = LinqIntelliSenseOptions.Instance.AutoShowCompletionListInArgumentList;
        }

        private void cbShowCompletionListAfterCharTyped_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqIntelliSenseOptions.Instance.ShowCompletionListAfterCharTyped = (bool)cbShowCompletionListAfterCharTyped.IsChecked;
            LinqIntelliSenseOptions.Instance.Save();
        }

        private void cbShowCompletionListAfterCharDeleted_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqIntelliSenseOptions.Instance.ShowCompletionListAfterCharTyped = (bool)cbShowCompletionListAfterCharTyped.IsChecked;
            LinqIntelliSenseOptions.Instance.Save();
        }

        private void cbAutoShowCompletionListInArgumentList_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqIntelliSenseOptions.Instance.ShowCompletionListAfterCharTyped = (bool)cbShowCompletionListAfterCharTyped.IsChecked;
            LinqIntelliSenseOptions.Instance.Save();
        }

        private void cbShowCompletionListAfterCharTyped_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqIntelliSenseOptions.Instance.ShowCompletionListAfterCharTyped = (bool)cbShowCompletionListAfterCharTyped.IsChecked;
            LinqIntelliSenseOptions.Instance.Save();
        }

        private void cbShowCompletionListAfterCharDeleted_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqIntelliSenseOptions.Instance.ShowCompletionListAfterCharTyped = (bool)cbShowCompletionListAfterCharTyped.IsChecked;
            LinqIntelliSenseOptions.Instance.Save();
        }

        private void cbAutoShowCompletionListInArgumentList_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqIntelliSenseOptions.Instance.ShowCompletionListAfterCharTyped = (bool)cbShowCompletionListAfterCharTyped.IsChecked;
            LinqIntelliSenseOptions.Instance.Save();
        }
    }
}
