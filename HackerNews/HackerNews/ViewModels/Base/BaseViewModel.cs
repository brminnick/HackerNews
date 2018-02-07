using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HackerNews
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Fields
        static int _networkIndicatorCount = 0;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        protected void SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyname = "", System.Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyname);
        }

        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}