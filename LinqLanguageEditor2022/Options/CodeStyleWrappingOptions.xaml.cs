using System.Windows.Controls;

namespace LinqLanguageEditor2022.Options
{
    /// <summary>
    /// Interaction logic for CodeStyleWrappingOption.xaml
    /// </summary>
    public partial class CodeStyleWrappingOptions : UserControl
    {
        public CodeStyleWrappingOptions()
        {
            InitializeComponent();
        }
        internal CodeStyleWrappingOptionPage wrappingOptionPage;

        public void Initialize()
        {
            cbLeaveBlockOnSingleLine.IsChecked = LinqCodeStyleOptions.Instance.LeaveBlockOnSingleLine;
            cbLeaveStatementMemberDeclareOnSameLine.IsChecked = LinqCodeStyleOptions.Instance.LeaveStatementMemberDeclareOnSameLine;
        }

        private void cbLeaveBlockOnSingleLine_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.LeaveBlockOnSingleLine = (bool)cbLeaveBlockOnSingleLine.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbLeaveStatementMemberDeclareOnSameLine_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.LeaveStatementMemberDeclareOnSameLine = (bool)cbLeaveStatementMemberDeclareOnSameLine.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbLeaveBlockOnSingleLine_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.LeaveBlockOnSingleLine = (bool)cbLeaveBlockOnSingleLine.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbLeaveStatementMemberDeclareOnSameLine_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.LeaveStatementMemberDeclareOnSameLine = (bool)cbLeaveStatementMemberDeclareOnSameLine.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }
    }
}
