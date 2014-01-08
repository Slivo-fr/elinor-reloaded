using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using EVE.Net.Character;

namespace Elinor
{
    /// <summary>
    /// Interaction logic for ApiImportSelectFactionWindow.xaml
    /// </summary>
    public partial class ApiImportSelectFactionWindow
    {
        private readonly CharWrapper _chara;
        public double corpStanding;
        public double factionStanding;
        public string corpName;
        public string factionName;

        internal ApiImportSelectFactionWindow(CharWrapper chr)
        {
            InitializeComponent();
            _chara = chr;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var none = new StandingWrapper("<None>", .0);
            cbCorp.Items.Add(none);
            cbFaction.Items.Add(none);
            cbCorp.SelectedIndex = 0;
            cbFaction.SelectedIndex = 0;

            var standings = new NPCStandings(
                _chara.KeyId, 
                _chara.VCode,
                _chara.CharId.ToString(CultureInfo.InvariantCulture)
            );

            standings.Query();

            foreach (NPCStandings.Standing standing in standings.standings.NPCCorporations)
            {
                var wrap = new StandingWrapper(standing.fromName, standing.standing);
                cbCorp.Items.Add(wrap);
            }

            foreach (NPCStandings.Standing standing in standings.standings.factions)
            {
                var wrap = new StandingWrapper(standing.fromName, standing.standing);
                cbFaction.Items.Add(wrap);
            }

            ToolTipService.SetShowDuration(imgHelp, int.MaxValue);
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            
            corpStanding = ((StandingWrapper)cbCorp.SelectedItem).standing;

            if (corpStanding == 0)
            {
                corpName = null;
            }
            else
            {
                corpName = ((StandingWrapper)cbCorp.SelectedItem).name; 
            }
            
            factionStanding = ((StandingWrapper) cbFaction.SelectedItem).standing;

            if (factionStanding == 0)
            {
                factionName = null;
            }
            else
            {
                factionName = ((StandingWrapper)cbFaction.SelectedItem).name;
            }

            DialogResult = true;
            Close();
        }
    }
}