
using System.Windows.Controls;

namespace LinqLanguageEditor2022.ToolWindows
{
    /// <summary>
    /// Interaction logic for RadioListBox.xaml
    /// </summary>
    public partial class RadioListBox : ListBox
    {
        public RadioListBox()
        {
            InitializeComponent();

            SelectionMode = SelectionMode.Single;
        }

        public new SelectionMode SelectionMode
        {
            get
            {
                return base.SelectionMode;
            }
            private set
            {
                base.SelectionMode = value;
            }
        }

        //private void ItemRadioClick(object sender, RoutedEventArgs e)
        //{
        //    ListBoxItem sel = (e.Source as RadioButton).TemplatedParent as ListBoxItem;
        //    int newIndex = this.ItemContainerGenerator.IndexFromContainer(sel); ;
        //    this.SelectedIndex = newIndex;
        //}

        //protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        //{
        //    base.OnSelectionChanged(e);

        //    CheckRadioButtons(e.RemovedItems, false);
        //    CheckRadioButtons(e.AddedItems, true);
        //}

        private void CheckRadioButtons(System.Collections.IList radioButtons, bool isChecked)
        {
            foreach (object item in radioButtons)
            {
                ListBoxItem lbi = this.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;

                if (lbi != null)
                {
                    RadioButton radio = lbi.Template.FindName("radio", lbi) as RadioButton;
                    if (radio != null)
                        radio.IsChecked = isChecked;
                }
            }
        }

        private void ItemRadioClick(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
