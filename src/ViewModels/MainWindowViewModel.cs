using FileAccessControlAgent.Views;
using System.Windows.Controls;

namespace FileAccessControlAgent.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            CurrentMenuView = new MainMenuView();
        }

        public UserControl CurrentMenuView { get; set; }
    }
}
