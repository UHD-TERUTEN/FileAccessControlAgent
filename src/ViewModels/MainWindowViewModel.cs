using FileAccessControlAgent.Views;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace FileAccessControlAgent.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            mainMenuView = new MainMenuView();
            fileAccessRejectLogMenuView = new FileAccessRejectLogMenuView();
            inquiryMenuView = new InquiryMenuView();

            CurrentMenuView = mainMenuView;
            ChangeMenuView = DoChangeMenuView;
        }

        private void DoChangeMenuView(object parameter)
        {
            string targetMenuName = parameter as string;

            if (targetMenuName == "MainMenuView")
                CurrentMenuView = mainMenuView;

            else if (targetMenuName == "FileAccessRejectLogMenuView")
                CurrentMenuView = fileAccessRejectLogMenuView;

            else if (targetMenuName == "InquiryMenuView")
                CurrentMenuView = inquiryMenuView;

            RaisePropertyChanged("CurrentMenuView");
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Action<object> ChangeMenuView { get; set; }

        private UserControl mainMenuView;

        private UserControl fileAccessRejectLogMenuView;

        private UserControl inquiryMenuView;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserControl CurrentMenuView { get; set; }

    }
}
