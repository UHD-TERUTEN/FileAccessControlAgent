using FileAccessControlAgent.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace FileAccessControlAgent
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            ChangeMenuView = DoChangeMenuView;
            InitializeComponent();
            DataContext = new MainWindowViewModel(NavigateToInquiryMenu);
        }

        private void DoChangeMenuView(object parameter)
        {
            ((MainWindowViewModel)DataContext).ChangeMenuView.Invoke(parameter);
        }

        private void NavigateToInquiryMenu()
        {
            ButtonAutomationPeer peer = new ButtonAutomationPeer(nav.inquiryMenuNavigateButton);
            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();
        }

        public Action<object> ChangeMenuView { get; set; }
    }
}