using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnewheroVisitorManagement.Utilities;
using System.Windows.Input;

namespace OnewheroVisitorManagement.ViewModel
{
    public class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand AdminPageCommand { get; set; }
        public ICommand VisitorPageCommand { get; set; }
        public ICommand VCCommand { get; set; }
        public ICommand AnalyticsCommand { get; set; }
        public ICommand AboutCommand { get; set; }
        public ICommand AdminEventsCommand { get; set; }
        public ICommand VisitorEventsCommand { get; set; }
        public ICommand ContactCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand BackCommand { get; set; }

        private void AdminPage(object obj) => CurrentView = new AdminPageVM();
        private void VisitorPage(object obj) => CurrentView = new VisitorPageVM();
        private void VisitorControl(object obj) => CurrentView = new VisitorControlVM();
        private void Analytics(object obj) => CurrentView = new AnalyticsVM();
        private void About(object obj) => CurrentView = new AboutVM();
        private void AdminEvents(object obj) => CurrentView = new AdminEventsVM();
        private void VisitorEvents(object obj) => CurrentView = new VisitorEventsVM();
        private void Contact(object obj) => CurrentView = new ContactVM();
        private void Logout(object obj) => CurrentView = new MainWindow();
        private void Exit(object obj) => Environment.Exit(0);

        public NavigationVM(string user)
        {
            AdminPageCommand = new RelayCommand(AdminPage);
            VisitorPageCommand = new RelayCommand(VisitorPage);
            VCCommand = new RelayCommand(VisitorControl);
            AnalyticsCommand = new RelayCommand(Analytics);
            AboutCommand = new RelayCommand(About);
            AdminEventsCommand = new RelayCommand(AdminEvents);
            VisitorEventsCommand = new RelayCommand(VisitorEvents);
            ContactCommand = new RelayCommand(Contact);
            LogoutCommand = new RelayCommand(Logout);
            ExitCommand = new RelayCommand(Exit);

            // Set default view
            if (user == "Admin")
            {
                CurrentView = new AdminPageVM();
            }
            else if (user == "Visitor")
            {
                CurrentView = new VisitorPageVM();
            }
        }

    }
}
