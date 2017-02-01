using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Elinor
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow
    {
        private const string Charname = "Slivo";
        private const string Support = "Reddit";

        public AboutWindow()
        {
            InitializeComponent();
            SetLinearGradientUnderline();

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.ProductVersion;

            lblVersion.Content = version;
            lblChar.Content = Charname;
            LblSupport.Content = Support;
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void LblCharMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.reddit.com/user/Slivo-fr");
        }

        private void LblCharMouseEnter(object sender, MouseEventArgs e)
        {
            lblChar.Content = CreateUnderlinedTextBlock(Charname);
        }

        private void LblCharMouseLeave(object sender, MouseEventArgs e)
        {
            lblChar.Content = Charname;
        }

        private void LblSupportMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://www.reddit.com/r/EveElinor/");
        }

        private void LblSupportMouseEnter(object sender, MouseEventArgs e)
        {
            LblSupport.Content = CreateUnderlinedTextBlock(Support);
        }

        private void LblSupportMouseLeave(object sender, MouseEventArgs e)
        {
            LblSupport.Content = Support;
        }

        private TextBlock CreateUnderlinedTextBlock(string text)
        {
            var myUnderline = new TextDecoration
                                  {
                                      Pen = new Pen(Brushes.Blue, 1),
                                      PenThicknessUnit = TextDecorationUnit.FontRecommended
                                  };
            var myCollection = new TextDecorationCollection {myUnderline};
            var blockHead = new TextBlock {TextDecorations = myCollection, Text = text};
            return blockHead;
        }

        private void SetLinearGradientUnderline()
        {
            var myBaseLine = new TextDecoration();
            var myUnderLine = new TextDecoration();

            var gy = new Pen
                         {
                             Brush =
                                 new LinearGradientBrush(Colors.Green, Colors.Yellow, new Point(0, 0.5),
                                                         new Point(1, 0.5)) {Opacity = 0.5},
                             Thickness = .75,
                             DashStyle = DashStyles.DashDotDot
                         };
            myBaseLine.Pen = gy;
            myBaseLine.PenThicknessUnit = TextDecorationUnit.FontRecommended;
            myBaseLine.Location = TextDecorationLocation.OverLine;

            var yr = new Pen
                         {
                             Brush =
                                 new LinearGradientBrush(Colors.Yellow, Colors.Red, new Point(0, 0.5), new Point(1, 0.5))
                                     {Opacity = 0.5},
                             Thickness = .75,
                             DashStyle = DashStyles.DashDotDot
                         };
            myUnderLine.Pen = yr;
            myUnderLine.PenThicknessUnit = TextDecorationUnit.FontRecommended;

            var myCollection = new TextDecorationCollection {myBaseLine, myUnderLine};
            var block = new TextBlock {TextDecorations = myCollection, Text = "Elinor"};
            label1.Content = block;
        }


    }
}