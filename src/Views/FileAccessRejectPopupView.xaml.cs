using System;
using System.Windows;
using System.Windows.Threading;

namespace FileAccessControlAgent
{
    public partial class FileAccessRejectPopup : Window
    {
        public FileAccessRejectPopup()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                Left = corner.X - ActualWidth;
                Top = corner.Y - ActualHeight;
            }));
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            Close();
        }
    }
}