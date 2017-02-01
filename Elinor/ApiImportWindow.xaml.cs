using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using EVE.Net;
using EVE.Net.Character;

namespace Elinor
{
    /// <summary>
    /// Interaction logic for ApiImportWindow.xaml
    /// </summary>
    public partial class ApiImportWindow
    {
        public Profile profile = new Profile();
        private bool? _getStandingAccess;

        public ApiImportWindow()
        {
            InitializeComponent();
            profile.accounting = 0;
            profile.brokerRelations = 0;
            profile.keyId = null;
            profile.vcode = null;
            profile.charName = null;
            profile.isAPI = true;
        }

        private void BtnGetCharsClick(object sender, RoutedEventArgs e)
        {
            cbChars.Items.Clear();

            try
            {
                profile.keyId = tbKeyId.Text;
                profile.vcode = tbVCode.Text;

                var info = new APIKeyInfo(profile.keyId.ToString(CultureInfo.InvariantCulture), profile.vcode);
                info.Query();

                if (info.characters.Count == 0)
                {
                    MessageBox.Show(
                        "No characters for this API information.\nPlease check you API information",
                        "No characters found", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Warning
                    );
                }
                else
                {
                    lblChar.Visibility = Visibility.Visible;
                    cbChars.Visibility = Visibility.Visible;

                    foreach (APIKeyInfo.Character chr in info.characters)
                    {
                        var chara = new CharWrapper
                        {
                            KeyId = profile.keyId,
                            VCode = profile.vcode,
                            Charname = chr.characterName,
                            CharId = chr.characterID
                        };

                        cbChars.Items.Add(chara);
                        cbChars.SelectedIndex = 0;
                    }
                    if (cbChars.Items.Count > 0)
                    {
                        btnOk.IsEnabled = true;
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "Key ID must be a number", 
                    "Invalid Key ID", 
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            tbKeyId.Focus();
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            btnOk.IsEnabled = false;
            pbLoading.Visibility = Visibility.Visible;
            var chara = (CharWrapper) cbChars.SelectedItem;

            var worker = new BackgroundWorker();
            _getStandingAccess = true;

            worker.RunWorkerCompleted += delegate
            {
                if (_getStandingAccess == true) DialogResult = true;
                else Dispatcher.Invoke(new Action(Close));
            };

            worker.DoWork += delegate
            {
                var sheet = new CharacterSheet(chara.KeyId, chara.VCode, chara.CharId.ToString(CultureInfo.InvariantCulture));
                sheet.Query();

                foreach (CharacterSheet.Skill skill in sheet.skills)
                {
                    if (skill.typeID == 3446) //"Broker Relations"
                        profile.brokerRelations = skill.level;
                    if (skill.typeID == 16622) //"Accounting" 
                        profile.accounting = skill.level;
                }

                Dispatcher.Invoke(new Action(delegate
                {
                    var aisfw = new ApiImportSelectFactionWindow(chara)
                    {
                        Topmost = true,
                        Top = Top + 10,
                        Left = Left + 10,
                    };

                    _getStandingAccess = aisfw.ShowDialog();

                    if (_getStandingAccess == true)
                    {
                        profile.corpStanding = aisfw.corpStanding;
                        profile.factionStanding = aisfw.factionStanding;
                        profile.corporation = aisfw.corpName;
                        profile.faction = aisfw.factionName;
                    }
                    else
                    {
                        _getStandingAccess = false;
                    }
                }));

                profile.charName = chara.Charname;
                profile.charId = chara.CharId;

                if (profile.corporation != null)
                {
                    profile.profileName = chara.Charname + " - " + profile.corporation;
                }
                else
                {
                    profile.profileName = chara.Charname + " - no corporation";
                }
            };

            worker.RunWorkerAsync();
        }

        private void BtnCreateKeyClick(object sender, RoutedEventArgs e)
        {
            Process.Start(@"https://community.eveonline.com/support/api-key/CreatePredefined?accessMask=1099431944");
        }
    }
}