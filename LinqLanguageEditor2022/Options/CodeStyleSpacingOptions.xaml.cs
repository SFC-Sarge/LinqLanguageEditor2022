using System.Windows.Controls;

namespace LinqLanguageEditor2022.Options
{
    /// <summary>
    /// Interaction logic for CodeStyleSpacingOptions.xaml
    /// </summary>
    public partial class CodeStyleSpacingOptions : UserControl
    {
        public CodeStyleSpacingOptions()
        {
            InitializeComponent();
        }
        internal CodeStyleSpacingOptionPage spacingOptionsPage;

        public void Initialize()
        {
            cbInsertSpaceBetweenMethodNameOpenParenthesis.IsChecked = LinqCodeStyleOptions.Instance.InsertSpaceBetweenMethodNameOpenParenthesis;
            cbInsertSpaceInParameterlistParentheses.IsChecked = LinqCodeStyleOptions.Instance.InsertSpaceInParameterlistParentheses;
        }

        private void cbInsertSpaceBetweenMethodNameOpenParenthesis_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.InsertSpaceBetweenMethodNameOpenParenthesis = (bool)cbInsertSpaceBetweenMethodNameOpenParenthesis.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbInsertSpaceInParameterlistParentheses_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.InsertSpaceInParameterlistParentheses = (bool)cbInsertSpaceInParameterlistParentheses.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbInsertSpaceBetweenMethodNameOpenParenthesis_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.InsertSpaceBetweenMethodNameOpenParenthesis = (bool)cbInsertSpaceBetweenMethodNameOpenParenthesis.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }

        private void cbInsertSpaceInParameterlistParentheses_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            LinqCodeStyleOptions.Instance.InsertSpaceInParameterlistParentheses = (bool)cbInsertSpaceInParameterlistParentheses.IsChecked;
            LinqCodeStyleOptions.Instance.Save();
        }
    }
}
