using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using EVE.Net;
using EVE.Net.Character;

namespace Elinor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        internal Profile profile { get; set; }

        private readonly FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher();

        private DirectoryInfo _logdir =
            new DirectoryInfo(Environment.GetEnvironmentVariable("USERPROFILE") + @"\Documents\EVE\logs\marketlogs");

        private readonly DirectoryInfo _profdir = new DirectoryInfo("profiles");
        private double _buy;
        private bool _cacheCleared;
        private bool _closePending;
        private FileSystemEventArgs _lastEvent;

        private double _sell;
        private int _typeId;

        private List<double> hubIds = new List<double> {
            60003760,   // Jita
            60004588,   // Rens
            60008494,   // Amarr
            60011866,   // Dodixie
            60005686    // Hek
        };


        public MainWindow()
        {
            InitializeComponent();

            string customLogDir = Properties.Settings.Default.logpath;

            if (customLogDir != "")
            {
                _logdir = new DirectoryInfo(customLogDir);
            }
  
            if (!_logdir.Exists)
            {
                selectLogPath();
            }
            else
            {
                SetWatcherAndStuff();
            }
        }

        public void selectLogPath()
        {
            var dlg = new SelectLogPathWindow();
            bool? showDialog = dlg.ShowDialog();

            if (showDialog != null && (bool)showDialog)
            {
                _logdir = dlg.Logpath;

                Properties.Settings.Default.logpath = _logdir.FullName;
                Properties.Settings.Default.Save();

                SetWatcherAndStuff();
            }
        }
            
        
        private void SetWatcherAndStuff()
        {
            if (!_logdir.Exists)
                _logdir.Create();
            if (!_profdir.Exists)
                _profdir.Create();

            var init = new BackgroundWorker();
            init.DoWork += (sender, args) =>
            {
                var stat = new ServerStatus();
                stat.Query();
            };

            init.RunWorkerAsync();

            _fileSystemWatcher.Path = _logdir.FullName;
            _fileSystemWatcher.Created += FileSystemWatcherOnCreated;
            _fileSystemWatcher.EnableRaisingEvents = true;

            UpdateStatus();
        }

        private void ProcessData(string s)
        {
            List<List<string>> table = CSVReader.GetTableFromCSV(s);

            if (table == null) return;

            if (profile.sellRange == (int)Profile.ranges.HUB)
            {
                IOrderedEnumerable<List<string>> sell = from List<string> row in table
                                                        where row[7] == "False" && row[13] == "0" && hubIds.Contains(double.Parse(row[10]))
                                                        orderby
                                                            double.Parse(row[0], CultureInfo.InvariantCulture) ascending
                                                        select row;

                string sss = sell.Any() ? sell.ElementAt(0)[0] : "-1.0";
                _sell = double.Parse(sss, CultureInfo.InvariantCulture);


            }
            else if (profile.sellRange == (int)Profile.ranges.SYSTEM)
            {
                IOrderedEnumerable<List<string>> sell = from List<string> row in table
                                                        where row[7] == "False" && row[13] == "0"
                                                        orderby
                                                            double.Parse(row[0], CultureInfo.InvariantCulture) ascending
                                                        select row;

                string sss = sell.Any() ? sell.ElementAt(0)[0] : "-1.0";
                _sell = double.Parse(sss, CultureInfo.InvariantCulture);
            
            }
            else if (profile.sellRange == (int)Profile.ranges.ONEJUMP)
            {
                IOrderedEnumerable<List<string>> sell = from List<string> row in table
                                                        where row[7] == "False" && Int32.Parse(row[13]) < 2
                                                        orderby
                                                            double.Parse(row[0], CultureInfo.InvariantCulture) ascending
                                                        select row;

                string sss = sell.Any() ? sell.ElementAt(0)[0] : "-1.0";
                _sell = double.Parse(sss, CultureInfo.InvariantCulture);
            
            }
            else
            {
                IOrderedEnumerable<List<string>> sell = from List<string> row in table
                                                        where row[7] == "False" && Int32.Parse(row[13]) < 3
                                                        orderby
                                                            double.Parse(row[0], CultureInfo.InvariantCulture) ascending
                                                        select row;

                string sss = sell.Any() ? sell.ElementAt(0)[0] : "-1.0";
                _sell = double.Parse(sss, CultureInfo.InvariantCulture);
            
            }

            if(profile.buyRange == (int)Profile.ranges.HUB)
            {
                IOrderedEnumerable<List<string>> buy = from List<string> row in table
                                                       where row[7] == "True" && row[13] == "0" && hubIds.Contains(double.Parse(row[10]))
                                                       orderby
                                                           double.Parse(row[0], CultureInfo.InvariantCulture) descending
                                                       select row;
                string bbb = buy.Any() ? buy.ElementAt(0)[0] : "-1.0";
                _buy = double.Parse(bbb, CultureInfo.InvariantCulture);
            }
            else if (profile.buyRange == (int)Profile.ranges.SYSTEM)
            {
                IOrderedEnumerable<List<string>> buy = from List<string> row in table
                                                       where row[7] == "True" && row[13] == "0"
                                                       orderby
                                                           double.Parse(row[0], CultureInfo.InvariantCulture) descending
                                                       select row;
                string bbb = buy.Any() ? buy.ElementAt(0)[0] : "-1.0";
                _buy = double.Parse(bbb, CultureInfo.InvariantCulture);
            }
            else if (profile.buyRange == (int)Profile.ranges.ONEJUMP)
            {
                IOrderedEnumerable<List<string>> buy = from List<string> row in table
                                                       where row[7] == "True" && Int32.Parse(row[13]) < 2
                                                       orderby
                                                           double.Parse(row[0], CultureInfo.InvariantCulture) descending
                                                       select row;
                string bbb = buy.Any() ? buy.ElementAt(0)[0] : "-1.0";
                _buy = double.Parse(bbb, CultureInfo.InvariantCulture);
            }
            else
            {
                IOrderedEnumerable<List<string>> buy = from List<string> row in table
                                                       where row[7] == "True" && Int32.Parse(row[13]) < 3
                                                       orderby
                                                           double.Parse(row[0], CultureInfo.InvariantCulture) descending
                                                       select row;
                string bbb = buy.Any() ? buy.ElementAt(0)[0] : "-1.0";
                _buy = double.Parse(bbb, CultureInfo.InvariantCulture);
            }


            IEnumerable<List<string>> aRow = from List<string> row in table
                                             select row;

            foreach (var list in aRow)
            {
                int i;
                _typeId = int.TryParse(list[2], out i) ? i : -1;
                break;
            }

            var setItemName = new BackgroundWorker();
            setItemName.DoWork += (sender, args) =>
            {
                var prod = new TypeName(new[] {_typeId.ToString(CultureInfo.InvariantCulture)});
                prod.Query();

                Dispatcher.Invoke(new Action(delegate
                {
                    if (prod.types.Count > 0)
                    {
                        TypeName.GameType type = prod.types[0];
                        lblItemName.Content = type.typeName;
                        lblItemName.ToolTip = type.typeName;
                    }
                    else
                    {
                        lblItemName.Content = "Unknown";
                        lblItemName.ToolTip ="Product not found";
                    }
                }));
            };

            setItemName.RunWorkerAsync();

            Dispatcher.Invoke(new Action(delegate
            {
                lblSell.Content = _sell >= 0
                                    ? String.Format("{0:n} ISK", _sell)
                                    : "No orders in range";
                lblBuy.Content = _buy >= 0
                                    ? String.Format("{0:n} ISK", _buy)
                                    : "No orders in range";
            }));

            var cdt = new CalculateDataThread(_sell, _buy, this);
            var calc = new Thread(cdt.Run);
            calc.Start();
        }

        private void FileSystemWatcherOnCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            Dispatcher.Invoke(new Action(delegate
            {
                lblItemName.Content = "Fetching...";
                lblItemName.ToolTip = string.Empty;

                if (cbAutoCopy.IsChecked != null && (bool) cbAutoCopy.IsChecked)
                {
                    var img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri("pack://application:,,,/Elinor;component/Images/38_16_195.png");
                    img.EndInit();
                    imgCopyStatus.Source = img;
                }
            }));

            _lastEvent = fileSystemEventArgs;
            while (MiscTools.IsFileLocked(new FileInfo(fileSystemEventArgs.FullPath))) Thread.Sleep(25);
            if (fileSystemEventArgs.ChangeType == WatcherChangeTypes.Created &&
                fileSystemEventArgs.Name.EndsWith(".txt"))
            {
                ProcessData(fileSystemEventArgs.FullPath);
            }

            Dispatcher.Invoke(new Action(delegate
            {
                if (cbAutoCopy.IsChecked != null && (bool) cbAutoCopy.IsChecked)
                {
                    bool isSell = rbSell.IsChecked != null && (bool) rbSell.IsChecked;

                    if (rbSell.IsChecked != null && (bool) rbSell.IsChecked)
                        ClipboardTools.SetClipboardWrapper(
                            ClipboardTools.GetSellPrice(_sell, profile));
                    else if (rbBuy.IsChecked != null && (bool) rbBuy.IsChecked)
                        ClipboardTools.SetClipboardWrapper(
                            ClipboardTools.GetBuyPrice(_buy, profile));


                    var img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = (isSell && _sell > 0) || (!isSell && _buy > 0)
                            ? new Uri("pack://application:,,,/Elinor;component/Images/38_16_193.png")
                            : new Uri("pack://application:,,,/Elinor;component/Images/38_16_194.png");
                    img.EndInit();
                    imgCopyStatus.Source = img;
                }
            }));

            UpdateStatus();
        }
        
        private void UpdateStatus()
        {
            long size = _logdir.GetFiles().Sum(fi => fi.Length);

            Dispatcher.Invoke(
                new Action(delegate { tbStatus.Text = String.Format("Market logs: {0:n0} KB", size/1024); }));
        }

        private void TbStatusMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CacheTools.ClearMarketLogs(_logdir);
            UpdateStatus();
        }

        private void BtnStayOnTopClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(delegate
            {
                if (btnStayOnTop.IsChecked != null)
                    Topmost = (bool) btnStayOnTop.IsChecked;
            }));
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            Properties.Settings.Default.pin = btnStayOnTop.IsChecked != null && (bool) btnStayOnTop.IsChecked;

            if (cbAutoCopy.IsChecked == null || !(bool)cbAutoCopy.IsChecked)
                Properties.Settings.Default.autocopy = 0;
            else if (rbSell.IsChecked != null && (bool)rbSell.IsChecked)
                Properties.Settings.Default.autocopy = 1;
            else if(rbBuy.IsChecked != null && (bool) rbBuy.IsChecked)
                Properties.Settings.Default.autocopy = -1;
            
            Properties.Settings.Default.Save();
            Profile.SaveSettings(profile);

            Properties.Settings.Default.selectedprofile = profile.profileName;
            Properties.Settings.Default.Save();
        }

        private void TiSettingsLostFocus(object sender, RoutedEventArgs e)
        {
            Profile.SaveSettings(profile);
        }

        private void tiTradeSettingsLostFocus(object sender, RoutedEventArgs e)
        {
            Profile.SaveSettings(profile);
        }

        private void tiRangeSettingsLostFocus(object sender, RoutedEventArgs e)
        {
            Profile.SaveSettings(profile);
        }

        private void tiBrokerSettingsLostFocus(object sender, RoutedEventArgs e)
        {
            Profile.SaveSettings(profile);
        }

        // TEST, TO BE REMOVED
        private void tiTradeSettingsGotFocus(object sender, RoutedEventArgs e)
        {
            //Profile.SaveSettings(profile);
        }

        private void tiRangeSettingsGotFocus(object sender, RoutedEventArgs e)
        {
            //Profile.SaveSettings(profile);
        }

        private void tiBrokerSettingsGotFocus(object sender, RoutedEventArgs e)
        {
            //Profile.SaveSettings(profile);
        }

        private void TiSettingsGotFocus(object sender, RoutedEventArgs e)
        {
            //updateSettingsDisplay();
        }

        private void updateSettingsDisplay()
        {
            Dispatcher.Invoke(new Action(delegate
            {
                // Charaters settings
                tbPreferred.Text = Convert.ToString(profile.marginThreshold * 100);
                tbMinimum.Text = Convert.ToString(profile.minimumThreshold * 100);

                tbCorpStanding.Text =
                    string.Format(
                        CultureInfo.InvariantCulture, "{0:n2}",
                        profile.corpStanding
                    );
                tbFactionStanding.Text =
                    string.Format(
                        CultureInfo.InvariantCulture, "{0:n2}",
                        profile.factionStanding
                    );

                cbBrokerRelations.SelectedIndex = profile.brokerRelations;
                cbAccounting.SelectedIndex = profile.accounting;

                // Trade settings
                cbAdvancedSettings.IsChecked = profile.advancedStepSettings;

                tbSellFract.Text = (profile.sellPercentage * 100).ToString(CultureInfo.InvariantCulture);
                tbBuyFract.Text = (profile.buyPercentage * 100).ToString(CultureInfo.InvariantCulture);
                tbSellThresh.Text = profile.sellThreshold.ToString(CultureInfo.InvariantCulture);
                tbBuyThresh.Text = profile.buyThreshold.ToString(CultureInfo.InvariantCulture);

                // Range settings
                cbSellRange.SelectedIndex = profile.sellRange;
                cbBuyRange.SelectedIndex = profile.buyRange;

                // Broker settings
                cbUseCustomBuyBroker.IsChecked = profile.useBuyCustomBroker;
                tbCustomBuyBroker.Text = (profile.buyCustomBroker * 100).ToString(CultureInfo.InvariantCulture);

                cbUseCustomSellBroker.IsChecked = profile.useSellCustomBroker;
                tbCustomSellBroker.Text = (profile.sellCustomBroker * 100).ToString(CultureInfo.InvariantCulture);

            }));
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            tbCorpStanding.TextChanged += TbCorpStandingTextChanged;
            tbFactionStanding.TextChanged += TbFactionStandingTextChanged;
            tbCorpStanding.LostFocus += TbStandingOnLostFocus;
            tbFactionStanding.LostFocus += TbStandingOnLostFocus;

            cbBrokerRelations.SelectionChanged += CbBrokerRelationsSelectionChanged;
            cbAccounting.SelectionChanged += CbAccountingSelectionChanged;

            cbSellRange.SelectionChanged += cbSellRangeSelectionChanged;
            cbBuyRange.SelectionChanged += cbBuyRangeSelectionChanged;

            for (int i = 0; i < 6; i++)
            {
                cbBrokerRelations.Items.Add(i);
                cbAccounting.Items.Add(i);
            }

            List<Object> rangeItems = new List<Object>();

            foreach (int i in Enum.GetValues(typeof(Profile.ranges)))
            {
                var type = typeof(Profile.ranges);
                var memInfo = type.GetMember(Enum.GetName(typeof(Profile.ranges), i).ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = ((DescriptionAttribute)attributes[0]).Description;

                ComboboxItem item = new ComboboxItem();
                item.Text = description;
                item.Value = i;

                cbBuyRange.Items.Add(item);
                cbSellRange.Items.Add(item);
            }

            profile = new Profile();
            cbProfiles.Items.Add(profile);
            cbProfiles.SelectedIndex = 0;

            PopupPlacements();

            UpdateProfiles();

            btnUpdate.IsChecked = Properties.Settings.Default.checkforupdates;

            var delayer = new BackgroundWorker();
            delayer.DoWork += (o, args) => Updates.CheckForUpdates();
            delayer.RunWorkerAsync();

            btnStayOnTop.IsChecked = Properties.Settings.Default.pin;
            Topmost = Properties.Settings.Default.pin;

            if (Properties.Settings.Default.autocopy == 0)
            {
                cbAutoCopy.IsChecked = false;
                rbSell.IsChecked = true;
            }
            else
            {
                cbAutoCopy.IsChecked = true;
                rbSell.IsChecked = Properties.Settings.Default.autocopy > 0;
                rbBuy.IsChecked = Properties.Settings.Default.autocopy < 0;
            }

            if (Properties.Settings.Default.selectedprofile != "")
            {
                cbProfiles.Text = Properties.Settings.Default.selectedprofile;
            }
        }

        private void TbStandingOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            double standing;
            var tbSender = (TextBox) sender;
            if (double.TryParse(tbSender.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out standing))
            {
                if (standing > 10) tbSender.Text = "10";
                if (standing < -10) tbSender.Text = "-10";
            }
        }

        private void PopupPlacements()
        {
            ppFactionStanding.PlacementTarget = tbFactionStanding;
            ppCorpStanding.PlacementTarget = tbCorpStanding;
        }
        
        private void UpdateProfiles()
        {
            List<Profile> profiles = Profiles.getAllProfiles();

            foreach(Profile profile in profiles)
            {
                cbProfiles.Items.Add(profile);
            }
        }

        private void CbBrokerRelationsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            profile.brokerRelations = cbBrokerRelations.SelectedIndex;
            UpdateBrokerFee();
        }

        private void BtnResetCharClick(object sender, RoutedEventArgs e)
        {
            profile.accounting = 0;
            profile.brokerRelations = 0;

            profile.corpStanding = 0;
            profile.factionStanding = 0;

            Profile.SaveSettings(profile);
            updateSettingsDisplay();
        }

        private void BtnResetTradeClick(object sender, RoutedEventArgs e)
        {
            profile.advancedStepSettings = false;
            profile.buyPercentage = 0;
            profile.sellPercentage = 0;
            profile.buyThreshold = 0;
            profile.sellThreshold = 0;

            profile.marginThreshold = 0.1;
            profile.minimumThreshold = 0.02;

            Profile.SaveSettings(profile);
            updateSettingsDisplay();
        }

        private void UpdateBrokerFee()
        {
            Dispatcher.Invoke(new Action(delegate
            {
                lblBrokerRelations.Content = String.Format("Broker fee: {0:n}%",
                    CalculateDataThread.NpcBroker(profile) * 100);
            }));
        }

        private void CbAccountingSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            profile.accounting = cbAccounting.SelectedIndex;
            Dispatcher.Invoke(
                new Action(delegate
                {
                    lblSalesTax.Content = String.Format("Sales tax: {0:n}%", CalculateDataThread.SalesTax(profile.accounting)*100);
                })
            );
        }

        private void CbProfilesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = cbProfiles.SelectedItem.ToString() != "Default";
            Profile.SaveSettings(profile);
            profile = (Profile) cbProfiles.SelectedItem;

            btnUpdateProfile.IsEnabled = (profile.isAPIProfile()) ? true : false;

            updateSettingsDisplay();
            if (_lastEvent != null) FileSystemWatcherOnCreated(this, _lastEvent);
        }

        private void BtnNewClick(object sender, RoutedEventArgs e)
        {
            var window = new NewProfileWindow(this)
            {
                Topmost = true,
                Top = Top + Height / 10,
                Left = Left + Width / 10,
            };

            window.ShowDialog();
        }

        public void btnUpdateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Skills
                var sheet = new CharacterSheet(
                    profile.keyId,
                    profile.vcode,
                    profile.charId.ToString(CultureInfo.InvariantCulture)
                );

                sheet.Query();

                if (sheet.characterID > 0)
                {
                    foreach (CharacterSheet.Skill skill in sheet.skills)
                    {
                        if (skill.typeID == 3446) //"Broker Relations"
                            profile.brokerRelations = skill.level;
                        if (skill.typeID == 16622) //"Accounting" 
                            profile.accounting = skill.level;
                    }

                    //Standings
                    var standings = new NPCStandings(
                        profile.keyId,
                        profile.vcode,
                        profile.charId.ToString(CultureInfo.InvariantCulture)
                    );

                    standings.Query();

                    if (profile.corporation != null)
                    {
                        foreach (NPCStandings.Standing standing in standings.standings.NPCCorporations)
                        {
                            if (profile.corporation == standing.fromName)
                            {
                                profile.corpStanding = standing.standing;
                            }
                        }
                    }

                    if (profile.faction != null)
                    {
                        foreach (NPCStandings.Standing standing in standings.standings.factions)
                        {
                            if (profile.faction == standing.fromName)
                            {
                                profile.factionStanding = standing.standing;
                            }
                        }
                    }

                    updateSettingsDisplay(); ;

                    MessageBox.Show("Profile successfully updated");
                }
                else
                {
                    MessageBox.Show("An error occured while updating the profile. The API key might have been deleted or modified.");
                }


            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while updating the profile.");
            }
        }

        private void BtnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (cbProfiles.SelectedItem.ToString() == "Default") return;
            Profile tSet = profile;
            int i = cbProfiles.SelectedIndex;
            cbProfiles.SelectedIndex = i - 1;
            cbProfiles.Items.RemoveAt(i);
            File.Delete("profiles\\" + tSet.profileName + ".dat");
        }

        private void BtnPath(object sender, RoutedEventArgs e)
        {
            selectLogPath();
        }

        private void BtnAboutClick(object sender, RoutedEventArgs e)
        {
            var abt = new AboutWindow
            {
                Topmost = Topmost,
                Top = Top + Height/10,
                Left = Left + Width/10
            };
            abt.ShowDialog();
        }

        private void PinWindow(object sender, ExecutedRoutedEventArgs e)
        {
            btnStayOnTop.IsChecked = !btnStayOnTop.IsChecked;
            BtnStayOnTopClick(this, null);
        }

        private void CbAutoCopyChecked(object sender, RoutedEventArgs e)
        {
            gbAutocopy.IsEnabled = true;
            imgCopyStatus.Source = null;
        }

        private void CbAutoCopyUnchecked(object sender, RoutedEventArgs e)
        {
            gbAutocopy.IsEnabled = false;
            imgCopyStatus.Source = null;
        }

        private void AutoCopy(object sender, ExecutedRoutedEventArgs e)
        {
            cbAutoCopy.IsChecked = !cbAutoCopy.IsChecked;
        }

        private void LblSellMouseDown(object sender, MouseButtonEventArgs e)
        {
            ClipboardTools.SetClipboardWrapper(ClipboardTools.GetSellPrice(_sell, profile));
        }

        private void LblBuyMouseDown(object sender, MouseButtonEventArgs e)
        {
            ClipboardTools.SetClipboardWrapper(ClipboardTools.GetBuyPrice(_buy, profile));
        }

        private void MiSubmitBugClick(object sender, RoutedEventArgs e)
        {
            Process.Start(@"https://github.com/Slivo-fr/elinor-reloaded/issues");
        }

        private void MiSubmitFeatureClick(object sender, RoutedEventArgs e)
        {
            Process.Start(@"http://redd.it/xl6mf");
        }

        private void RbChecked(object sender, RoutedEventArgs e)
        {
            double price = rbSell.IsChecked != null && (bool) rbSell.IsChecked
                               ? ClipboardTools.GetSellPrice(_sell, profile)
                               : ClipboardTools.GetBuyPrice(_buy, profile);
            ClipboardTools.SetClipboardWrapper(price);
        }

        private void CbAdvancedSettingsChecked(object sender, RoutedEventArgs e)
        {
            profile.advancedStepSettings = true;
            gbAdvancedSettings.IsEnabled = true;
        }

        private void CbAdvancedSettingsUnchecked(object sender, RoutedEventArgs e)
        {
            profile.advancedStepSettings = false;
            gbAdvancedSettings.IsEnabled = false;
        }

        private void TbSellFractTextChanged(object sender, TextChangedEventArgs e)
        {
            double fract;
            if (double.TryParse(tbSellFract.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out fract))
            {
                profile.sellPercentage = fract/100;
            }
        }

        private void TbSellThreshTextChanged(object sender, TextChangedEventArgs e)
        {
            double thresh;
            if (double.TryParse(tbSellThresh.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out thresh))
            {
                profile.sellThreshold = thresh;
            }
        }

        private void TbBuyFractTextChanged(object sender, TextChangedEventArgs e)
        {
            double fract;
            if (double.TryParse(tbBuyFract.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out fract))
            {
                profile.buyPercentage = fract/100;
            }
        }

        private void TbBuyThreshTextChanged(object sender, TextChangedEventArgs e)
        {
            double thresh;
            if (double.TryParse(tbBuyThresh.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out thresh))
            {
                profile.buyThreshold = thresh;
            }
        }

        private void TbCorpStandingTextChanged(object sender, TextChangedEventArgs e)
        {
            double standing;

            if (double.TryParse(tbCorpStanding.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out standing))
            {
                if (standing <= 10 && standing >= -10)
                {
                    ppCorpStanding.IsOpen = false;
                    profile.corpStanding = standing;
                    UpdateBrokerFee();
                }
                else
                {
                    ppCorpStanding.IsOpen = true;
                }
            }
        }

        private void TbFactionStandingTextChanged(object sender, TextChangedEventArgs e)
        {
            double standing;

            if (double.TryParse(tbFactionStanding.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out standing))
            {
                if (standing <= 10 && standing >= -10)
                {
                    ppFactionStanding.IsOpen = false;
                    profile.factionStanding = standing;
                    UpdateBrokerFee();
                }
                else
                {
                    ppFactionStanding.IsOpen = true;
                }
            }
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (!_cacheCleared)
            {
                _closePending = true;
                e.Cancel = true;
                var clearCache = new BackgroundWorker();
                clearCache.DoWork += (o, args) => CacheTools.ClearApiCache();
                clearCache.RunWorkerCompleted += (o, args) =>
                {
                    _cacheCleared = true;
                    if (_closePending) Close();
                };
                clearCache.RunWorkerAsync();
            }
        }

        private void BtnUpdateClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.checkforupdates = btnUpdate.IsChecked != null && (bool) btnUpdate.IsChecked;
            Properties.Settings.Default.Save();

            if(Properties.Settings.Default.checkforupdates) Updates.CheckForUpdates();
        }

        private void TbPreferredTextChanged(object sender, TextChangedEventArgs e)
        {
            if (profile != null)
            {

                double d;

                if (double.TryParse(tbPreferred.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                {
                    d /= 100;

                    if (d <= 1 && d >= 0)
                    {
                        profile.marginThreshold = d;
                    }
                }
            }
        }

        private void TbMinimumTextChanged(object sender, TextChangedEventArgs e)
        {
            if (profile != null)
            {

                double d;

                if (double.TryParse(tbMinimum.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                {
                    d /= 100;

                    if (d <= profile.marginThreshold && d >= 0)
                    {
                        profile.minimumThreshold = d;
                    }
                }
            }
        }

        private void TbCustomBuyBrokerChanged(object sender, TextChangedEventArgs e)
        {
            if (profile != null)
            {

                double d;

                if (double.TryParse(tbCustomBuyBroker.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                {
                    d /= 100;

                    if (d <= 1 && d >= 0)
                    {
                        profile.buyCustomBroker = d;
                    }
                }
            }
        }

        private void TbCustomSellBrokerChanged(object sender, TextChangedEventArgs e)
        {
            if (profile != null)
            {

                double d;

                if (double.TryParse(tbCustomSellBroker.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                {
                    d /= 100;

                    if (d <= 1 && d >= 0)
                    {
                        profile.sellCustomBroker = d;
                    }
                }
            }
        }

        private void cbUseCustomBuyBrokerChecked(object sender, RoutedEventArgs e)
        {
            profile.useBuyCustomBroker = true;
        }

        private void cbUseCustomBuyBrokerUnchecked(object sender, RoutedEventArgs e)
        {
            profile.useBuyCustomBroker = false;
        }

        private void cbUseCustomSellBrokerChecked(object sender, RoutedEventArgs e)
        {
            profile.useSellCustomBroker = true;
        }

        private void cbUseCustomSellBrokerUnchecked(object sender, RoutedEventArgs e)
        {
            profile.useSellCustomBroker = false;
        }

        private void cbSellRangeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSellRange.SelectedItem != null)
                profile.sellRange = ((ComboboxItem)cbSellRange.SelectedItem).Value;

            /*
            Dispatcher.Invoke(
                new Action(delegate
                {
                    lblSalesTax.Content = String.Format("Sales tax: {0:n}%", CalculateDataThread.SalesTax(profile.accounting) * 100);
                })
            );
            */
        }

        private void cbBuyRangeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbBuyRange.SelectedItem != null)
                profile.buyRange = ((ComboboxItem)cbBuyRange.SelectedItem).Value;

            /*
            Dispatcher.Invoke(
                new Action(delegate
                {
                    lblSalesTax.Content = String.Format("Sales tax: {0:n}%", CalculateDataThread.SalesTax(profile.accounting) * 100);
                })
            );
            */
        }
    }
}