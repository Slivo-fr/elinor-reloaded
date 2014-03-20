using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Elinor
{
    /// <summary>
    /// Logique d'interaction pour NewProfileWindow.xaml
    /// </summary>
    public partial class NewProfileWindow : Window
    {

        protected MainWindow mainWindow;

        public NewProfileWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
        }

        private void btnCreateNew_Click(object sender, RoutedEventArgs e)
        {
            var profileNameWindow = new ProfileNameWindow(mainWindow)
            {
                Topmost = true,
                Top = Top,
                Left = Left,
            };

            profileNameWindow.ShowDialog();

            if (profileNameWindow.DialogResult == true)
            {
                Close();
            }
        }

        private void btnImportAPI_Click(object sender, RoutedEventArgs e)
        {
            var aiw = new ApiImportWindow()
            {
                Topmost = true,
                Top = Top,
                Left = Left,
            };

            if (aiw.ShowDialog() == true)
            {
                Profile settings = aiw.profile;
                string fName = string.Format("profiles\\{0}.dat", settings.profileName);

                if (File.Exists(fName))
                {
                    MessageBoxResult result = MessageBox.Show(
                        "This profile already exist. Would you update it ?\nYou can delete the profile and import it back from a new API key if needed.",
                        "Profile already exists",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information
                    );

                    if (result == MessageBoxResult.Yes)
                    {
                        mainWindow.btnUpdateClick(null, null);
                    }
                }
                else
                {
                    mainWindow.cbProfiles.Items.Add(settings);
                    mainWindow.cbProfiles.SelectedItem = settings;
                    mainWindow.tcMain.SelectedIndex = 1;
                }

                Close();
            }            
        }
    }
}
