using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LinqLanguageEditor2022.ToolWindows
{
    public class ResultDialogViewModel : INotifyPropertyChanged
    {

        string header;
        public string Header { get => header; set => SetProperty(ref header, value); }
        bool checkedProperty;
        public bool CheckedProperty { get => checkedProperty; set => SetProperty(ref checkedProperty, value); }

        private System.Collections.IEnumerable radio;
        public System.Collections.IEnumerable Radio { get => radio; set => SetProperty(ref radio, value); }

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!(object.Equals(field, newValue)))
            {
                field = (newValue);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }

    }
}
