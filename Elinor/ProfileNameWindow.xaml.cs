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

        public ProfileNameWindow()
        {
            InitializeComponent();
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            string result = "OK";
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach (var invalid in invalidFileNameChars)
            {
                if (tbName.Text.Contains(invalid.ToString(CultureInfo.InvariantCulture))) result = "INVALID";
            }

            //if (MainWindow.cbProfiles.findString(tbName.Text) == -1)
            //TODO : Check for existing profile with same name

            switch (result)
            {
                case "OK":
                    ProfileName = tbName.Text;
                    DialogResult = true;
                    Close();
                    break;

                case "INVALID":
                    var sInvalid = invalidFileNameChars.Where(invalidFileNameChar => 
                    !char.IsControl(invalidFileNameChar)).Aggregate("", (current, invalidFileNameChar) 
                        => current + (invalidFileNameChar + " "));

                    MessageBox.Show(string.Format("Profile name may not contain\n{0}", sInvalid),
                                "Invalid profile name",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                case "ALREADY_EXIST":

                    MessageBox.Show(string.Format("This profile name already exist"),
                                "Invalid profile name",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
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
