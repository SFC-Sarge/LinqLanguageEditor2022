using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.PlatformUI;

namespace LinqLanguageEditor2022.ToolWindows
{
    public partial class ResultDialogWindow : DialogWindow
    {
        public string ResultsVar = null;
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
            TempResultVar.ResultVar = RadioListBox1.SelectedItem.ToString();
            DialogResult = true;
            Close();
        }

        private void cancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void RadioListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CurrentSelection.Text = RadioListBox1.SelectedItem.ToString();
        }

        private void DialogWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RadioListBox1.ItemsSource = null;
            RadioListBox1.Items.Clear();
            if (ResultsVar.Trim().EndsWith(","))
            {
                ResultsVar = ResultsVar.Trim().Substring(0, ResultsVar.Length - 1);
            }
            RadioListBox1.ItemsSource = ResultsVar.Split(',');

        }
    }
}
