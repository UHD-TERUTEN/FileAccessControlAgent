using FileAccessControlAgent.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Forms;

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

        private void mainWindowOnLoad(object sender, RoutedEventArgs e)
        {
            notifyIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.icon,
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip(),
                Text = "파일 접근 관리 에이전트",
            };

            var openItem = new ToolStripMenuItem() { Text = "열기" };
            var closeItem = new ToolStripMenuItem() { Text = "종료" };

            notifyIcon.ContextMenuStrip.Items.Add(openItem);
            notifyIcon.ContextMenuStrip.Items.Add(closeItem);

            // Register event handlers
            openItem.Click += ShowMainWindow;
            closeItem.Click += (object sender, EventArgs e) =>
            {
                System.Windows.Application.Current.Shutdown();
                notifyIcon.Dispose();
            };
            notifyIcon.DoubleClick += ShowMainWindow;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            base.OnClosing(e);
        }

        private void ShowMainWindow(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            Visibility = Visibility.Visible;
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

        private NotifyIcon notifyIcon;
    }
}