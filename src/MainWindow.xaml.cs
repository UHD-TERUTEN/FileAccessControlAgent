using FileAccessControlAgent.ViewModels;
using System;
using System.Windows;

namespace FileAccessControlAgent
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ChangeMenuView = DoChangeMenuView;
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void DoChangeMenuView(object parameter)
        {
            ((MainWindowViewModel)DataContext).ChangeMenuView.Invoke(parameter);
        }

        public Action<object> ChangeMenuView { get; set; }
    }
}