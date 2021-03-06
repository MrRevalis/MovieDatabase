using MovieDatabase.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MovieDatabase.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IFirebase FirebaseAuth => DependencyService.Get<IFirebase>();
        public IMovieDB MovieDB => DependencyService.Get<IMovieDB>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        //Connectivity
        private bool isNotConnected;
        public bool IsNotConnected
        {
            get => isNotConnected;
            set => SetProperty(ref isNotConnected, value);
        }

        private StackOrientation orientation;
        public StackOrientation Orientation
        {
            get => orientation;
            set => SetProperty(ref orientation, value);
        }

        public BaseViewModel()
        {
            Connectivity.ConnectivityChanged += ConnectionChanged;
            IsNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;

            DeviceDisplay.MainDisplayInfoChanged += OrientationChanged;
        }

        private void OrientationChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            DisplayOrientation orientation = e.DisplayInfo.Orientation;
            switch (orientation)
            {
                case DisplayOrientation.Portrait: Orientation = StackOrientation.Vertical; break;
                case DisplayOrientation.Landscape: Orientation = StackOrientation.Horizontal; break;
            }
        }

        ~BaseViewModel()
        {
            Connectivity.ConnectivityChanged -= ConnectionChanged;
            DeviceDisplay.MainDisplayInfoChanged -= OrientationChanged;
        }

        private async void ConnectionChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsNotConnected = e.NetworkAccess != NetworkAccess.Internet;
            var currentPage = Shell.Current.CurrentItem.Route;
            if (currentPage != "login" && IsNotConnected == true)
            {
                await Shell.Current.GoToAsync($"//{currentPage}/internet");
            }
            else
            {
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
