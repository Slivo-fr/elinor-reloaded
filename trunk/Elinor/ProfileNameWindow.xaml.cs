using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Elinor
{
    /// <summary>
    /// Interaction logic for ProfileNameWindow.xaml
    /// </summary>
    public partial class ProfileNameWindow
    {
        internal string ProfileName { get; private set; }
        protected MainWindow mainWindow;

        public ProfileNameWindow(MainWindow mainwindow)
        {
            InitializeComponent();
            this.mainWindow = mainwindow;

        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            string result = "OK";
            var profile = new Profile();
            var invalidFileNameChars = Path.GetInvalidFileNameChars();

            foreach (var invalid in invalidFileNameChars)
            {
                if (tbName.Text.Contains(invalid.ToString(CultureInfo.InvariantCulture))) result = "INVALID";
            }

            string fName = string.Format("profiles\\{0}.dat", tbName.Text);

            if (File.Exists(fName))
            {
                result = "ALREADY_EXIST";
            }

            switch (result)
            {
                case "OK":

                    profile.profileName = tbName.Text;
                    mainWindow.cbProfiles.Items.Add(profile);
                    mainWindow.cbProfiles.SelectedItem = profile;
                    mainWindow.tcMain.SelectedIndex = 1;
                    Profile.SaveSettings(profile);

                    DialogResult = true;
                    Close();

                    break;

                case "INVALID":

                    var sInvalid = invalidFileNameChars.Where(invalidFileNameChar => !char.IsControl(invalidFileNameChar))
                        .Aggregate("", (current, invalidFileNameChar) => current + (invalidFileNameChar + " "));

                    MessageBox.Show(
                        string.Format("Profile name may not contain\n{0}", sInvalid),
                        "Invalid profile name",
                        MessageBoxButton.OK, 
                        MessageBoxImage.Warning
                    );

                    break;

                case "ALREADY_EXIST":

                    MessageBox.Show(
                        "You must enter an unused profile name.",
                        "Profile name already used",
                        MessageBoxButton.OK, 
                        MessageBoxImage.Warning
                    );

                    break;


            }
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            tbName.Focus();
        }

        private void TbNameTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            btnOk.IsEnabled = tbName.Text != string.Empty;
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape) Close();
            if(e.Key == Key.Enter) BtnOkClick(this, null);
        }
    }
}
