using System.Windows.Controls;

namespace LinqLanguageEditor2022.Options
{
    /// <summary>
    /// Interaction logic for CodeStyleIndentationOptions.xaml
    /// </summary>
    public partial class CodeStyleIndentationOptions : UserControl
    {
        internal CodeStyleIndentationOptionPage indentationOptionsPage;

        public CodeStyleIndentationOptions()
        {
            InitializeComponent();
        }
        public void Initialize()
        {
            cbIndentBlockContents.IsChecked = LinqCodeStyleOptions.Instance.IndentBlockContents;
            cbIndentOpenCloseBraces.IsChecked = LinqCodeStyleOptions.Instance.IndentOpenCloseBraces;
            cbIndentCaseContents.IsChecked = LinqCodeStyleOptions.Instance.IndentCaseContents;
            cbIndentCaseContentsInBlock.IsChecked = LinqCodeStyleOptions.Instance.IndentCaseContentsInBlock;
            cbIndentCaseLabels.IsChecked = LinqCodeStyleOptions.Instance.IndentCaseLabels;
        }

        private void cbIndentBlockContents_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.IndentBlockContents = (bool)cbIndentBlockContents.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbIndentOpenCloseBraces_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.IndentOpenCloseBraces = (bool)cbIndentOpenCloseBraces.IsChecked;
            LinqCodeStyleOptions.Instance.Save();

        }

        private void cbIndentCaseContents_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.IndentCaseContents = (bool)cbIndentCaseContents.IsChecked;
            LinqCodeStyleOptions.Instance.Save();

        }

        private void cbIndentCaseContentsInBlock_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.IndentCaseContentsInBlock = (bool)cbIndentCaseContentsInBlock.IsChecked;
            LinqCodeStyleOptions.Instance.Save();

        }

        private void cbIndentCaseLabels_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.IndentCaseLabels = (bool)cbIndentCaseLabels.IsChecked;
            LinqCodeStyleOptions.Instance.Save();

        }

        private void cbIndentBlockContents_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.IndentBlockContents = (bool)cbIndentBlockContents.IsChecked;
            LinqCodeStyleOptions.Instance.Save();

        }

        private void cbIndentOpenCloseBraces_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.IndentOpenCloseBraces = (bool)cbIndentOpenCloseBraces.IsChecked;
            LinqCodeStyleOptions.Instance.Save();

        }

        private void cbIndentCaseContents_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.IndentCaseContents = (bool)cbIndentCaseContents.IsChecked;
            LinqCodeStyleOptions.Instance.Save();

        }

        private void cbIndentCaseContentsInBlock_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.IndentCaseContentsInBlock = (bool)cbIndentCaseContentsInBlock.IsChecked;
            LinqCodeStyleOptions.Instance.Save();

        }

        private void cbIndentCaseLabels_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.IndentCaseLabels = (bool)cbIndentCaseLabels.IsChecked;
            LinqCodeStyleOptions.Instance.Save();

        }
    }
}
