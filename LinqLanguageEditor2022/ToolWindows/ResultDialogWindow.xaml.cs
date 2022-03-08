using System.Collections.Generic;


using Microsoft.VisualStudio.PlatformUI;

namespace LinqLanguageEditor2022.ToolWindows
{
    public partial class ResultDialogWindow : DialogWindow
    {

        public ResultDialogWindow()
        {
            InitializeComponent();
            tbResultChange.Text = Constants.ResultVarChangeMsg;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void okButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TempResultVar.ResultVar = "results";
            DialogResult = true;
            Close();
        }

        private void cancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
