using System;
using System.IO;
using System.Security;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Elinor
{
    /// <summary>
    /// Interaction logic for SelectLogPathWindow.xaml
    /// </summary>
    public partial class SelectLogPathWindow
    {
        public SelectLogPathWindow()
        {
            InitializeComponent();
        }

        internal DirectoryInfo Logpath { get; private set; }

        private void BtnFileSelectClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbPath.Text = dialog.SelectedPath;
            }
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Logpath = new DirectoryInfo(tbPath.Text);
                DialogResult = true;
            }
            catch (SecurityException)
            {
                MessageBox.Show("You don't have access to that Folder");
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong.");
            }
            Close();
        }
    }
}