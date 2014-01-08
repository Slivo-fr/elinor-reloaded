using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Elinor
{
    internal class Tutorial
    {
        public static MainWindow Main;
        private static int _step;
        private static Popup _popup = new Popup();

        public static void ShowTutorialHint()
        {
            if (Properties.Settings.Default.showtutorial)
            {
                Properties.Settings.Default.showtutorial = false;
                Properties.Settings.Default.Save();
                MiscTools.FlashControl(Main.btnTutorial, Colors.Yellow, Main);
                var tutHint = new Popup
                {
                    VerticalOffset = -3,
                    PlacementTarget = Main.btnTutorial,
                    Placement = PlacementMode.Top,
                    IsOpen = true
                };
                var brd = new Border
                {
                    BorderBrush =
                        new LinearGradientBrush(Colors.LightSlateGray, Colors.Black, .45),
                    BorderThickness = new Thickness(1),
                    Background =
                        new LinearGradientBrush(Colors.LightYellow, Colors.PaleGoldenrod, .25),
                    Child = new TextBlock
                    {
                        Margin = new Thickness(4),
                        FontSize = 12,
                        Text = "Click to start a short tutorial on how to use Elinor"
                    }
                };
                tutHint.Child = brd;
                tutHint.MouseDown += delegate { tutHint.IsOpen = false; };
            }
        }

        public static void NextTip()
        {
            if (_step >= 15) Cancel();

            _popup.IsOpen = false;
            _popup = new Popup
                         {
                             AllowsTransparency = true,
                         };

            var btnNext = new Button
                              {
                                  Content = "Next",
                                  HorizontalAlignment = HorizontalAlignment.Right,
                                  Width = 65,
                              };
            btnNext.Click += (o, a) => NextTip();
            var btnClose = new Button
                               {
                                   Content = "Close",
                                   HorizontalAlignment = HorizontalAlignment.Left,
                                   Width = 65,
                               };
            btnClose.Click += (o, a) => Cancel();
            var panel = new StackPanel
                            {
                                Margin = new Thickness(6, 6, 6, 6)
                            };

            switch (_step)
            {
                case 0:
                    SelectTab(0);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "Welcome to Elinor tutorial.\n\n" +
                                                      "To proceed to the first step, please click the \"Next\" button.\n" +
                                                      "To cancel the tutorial, please click the \"Cancel\" button"
                                           });
                    _popup.PlacementTarget = Main;
                    _popup.Placement = PlacementMode.Center;
                    _popup.IsOpen = true;
                    break;

                case 1:
                    SelectTab(0);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "First you should customize a profile for your trading character."
                                           });
                    _popup.PlacementTarget = Main.cbProfiles;
                    _popup.Placement = PlacementMode.Bottom;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = 5;
                    _popup.HorizontalOffset = -50;
                    MiscTools.FlashControl(Main.cbProfiles, Colors.LimeGreen, Main);
                    break;

                case 2:
                    SelectTab(0);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "You can do this either by clicking \"New...\" and entering\n" +
                                                      "your data manually on the \"Settings\" tab..."
                                           });
                    _popup.PlacementTarget = Main.btnNew;
                    _popup.Placement = PlacementMode.Left;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = -5;
                    _popup.HorizontalOffset = -5;
                    MiscTools.FlashControl(Main.btnNew, Colors.LimeGreen, Main);
                    break;

                case 3:
                    SelectTab(0);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "Or by clicking \"Import...\" and\n" +
                                                      "fetching your character's data from the Eve API."
                                           });
                    _popup.PlacementTarget = Main.btnImport;
                    _popup.Placement = PlacementMode.Left;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = -5;
                    _popup.HorizontalOffset = -5;
                    MiscTools.FlashControl(Main.btnImport, Colors.LimeGreen, Main);
                    break;

                case 4:
                    SelectTab(0);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "You can delete a profile at any time by clicking \"Delete\"."
                                           });
                    _popup.PlacementTarget = Main.btnDelete;
                    _popup.Placement = PlacementMode.Left;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = -5;
                    _popup.HorizontalOffset = -5;
                    MiscTools.FlashControl(Main.btnDelete, Colors.LimeGreen, Main);
                    break;


                case 5:
                    SelectTab(1);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "You can set which margins should be displayed as \"good\"\n" +
                                                      "or \"bad\" margins on the \"Settings\" tab."
                                           });
                    _popup.PlacementTarget = Main.gbMarginsSettings;
                    _popup.Placement = PlacementMode.Bottom;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = 5;
                    _popup.HorizontalOffset = 10;
                    MiscTools.FlashControl(Main.tiSettings, Colors.LimeGreen, Main);
                    MiscTools.FlashControl(Main.gbMarginsSettings, Colors.LimeGreen, Main);
                    break;

                case 6:
                    SelectTab(1);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text =
                                                   "Here's where you can set you character's skills and standings, too."
                                           });
                    _popup.PlacementTarget = Main.gbFeesAndTaxes;
                    _popup.Placement = PlacementMode.Top;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = -5;
                    _popup.HorizontalOffset = 10;
                    MiscTools.FlashControl(Main.gbFeesAndTaxes, Colors.LimeGreen, Main);
                    break;

                case 7:
                    SelectTab(1);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "Should you screw your settings up, you can always reset them."
                                           });
                    _popup.PlacementTarget = Main.btnDefault;
                    _popup.Placement = PlacementMode.Top;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = -5;
                    _popup.HorizontalOffset = -270;
                    MiscTools.FlashControl(Main.btnDefault, Colors.LimeGreen, Main);
                    break;


                case 8:
                    SelectTab(0);
                    MiscTools.FlashControl(Main.tiOverview, Colors.LimeGreen, Main);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "Now - please open your EVE Online client.\n" +
                                                      "Open the market window and select an item you're interested in.\n" +
                                                      "Push the button named \"Export to File\" at the bottom of the market window.\n"
                                           });
                    _popup.PlacementTarget = Main;
                    _popup.Placement = PlacementMode.Center;
                    _popup.IsOpen = true;
                    break;

                case 9:
                    SelectTab(0);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "Elinor has now automatically imported the item's prices and\n" +
                                                      "calculated some data which can help determinate if the item\n" +
                                                      "is a fitting candidate for station trading."
                                           });
                    _popup.PlacementTarget = Main.brdImportant;
                    _popup.Placement = PlacementMode.Bottom;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = 5;
                    MiscTools.FlashForeground(Main.brdImportant);
                    break;

                case 10:
                    SelectTab(0);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "If you checked auto copy, the corresponding price plus/minus\n" +
                                                      ".01 ISK has now been copied to your clipboard ready to get\n" +
                                                      "pasted into the set up/modify order window in EVE Online."
                                           });
                    _popup.PlacementTarget = Main.cbAutoCopy;
                    _popup.Placement = PlacementMode.Top;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = -5;
                    _popup.HorizontalOffset = -115;
                    MiscTools.FlashControl(Main.gbAutocopy, Colors.LimeGreen, Main);
                    MiscTools.FlashForeground(Main.cbAutoCopy);
                    break;

                case 11:
                    SelectTab(0);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "If you don't want to work with auto copy, you can click\n" +
                                                      "the price tags to copy sell or buy price to your\n" +
                                                      "clipboard."
                                           });
                    _popup.PlacementTarget = Main.sprt1;
                    _popup.Placement = PlacementMode.Bottom;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = -45;
                    _popup.HorizontalOffset = 5;
                    MiscTools.FlashForeground(Main.lblBuy);
                    MiscTools.FlashForeground(Main.lblSell);
                    break;

                case 12:
                    SelectTab(0);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text =
                                                   "By clicking on the \"Pin\" button you can make Elinor stay above the\n" +
                                                   "EVE Online window so you can easily modify orders in EVE while always\n" +
                                                   "having Elinors data within your sight."
                                           });
                    _popup.PlacementTarget = Main.sbMain;
                    _popup.Placement = PlacementMode.Top;
                    _popup.IsOpen = true;
                    _popup.VerticalOffset = -5;
                    _popup.HorizontalOffset = 5;
                    MiscTools.FlashControl(Main.btnStayOnTop, Colors.LimeGreen, Main);
                    break;

                case 13:
                    SelectTab(0);
                    panel.Children.Add(new TextBlock
                                           {
                                               Text = "That's it!\n\n" +
                                                      "Most elements of Elinor provide a more or less helpful tooltips.\n" +
                                                      "Just explore a bit.\n" +
                                                      "\nPlease notice that Elinor is still in beta and be careful when pasting stuff\n" +
                                                      "into EVE Online."
                                           });
                    _popup.PlacementTarget = Main;
                    _popup.Placement = PlacementMode.Center;
                    _popup.IsOpen = true;
                    MiscTools.FlashControl(Main.btnBeta, Colors.LimeGreen, Main);
                    MiscTools.FlashControl(Main.btnAbout, Colors.LimeGreen, Main);
                    MiscTools.FlashForeground(Main.tbStatus);

                    break;
            }
            var grid = new Grid();
            if (_step >= 14)
            {
                btnClose.HorizontalAlignment = HorizontalAlignment.Right;
                grid.Children.Add(btnClose);
            }
            else
            {
                grid.Children.Add(btnNext);
                grid.Children.Add(btnClose);
            }
            grid.Margin = new Thickness(0, 20, 0, 0);
            panel.Children.Add(grid);

            var brd = new Border
                          {
                              CornerRadius = new CornerRadius(5),
                              BorderBrush = new LinearGradientBrush(Colors.LightSlateGray, Colors.Black, .45),
                              BorderThickness = new Thickness(1),
                              Background = new LinearGradientBrush(Colors.LightYellow, Colors.PaleGoldenrod, .25),
                              Child = panel,
                          };
            _popup.Child = brd;
            _step++;
        }

        private static void SelectTab(int i)
        {
            Main.Dispatcher.Invoke(new Action(delegate { Main.tcMain.SelectedIndex = i; }));
        }

        public static void Cancel()
        {
            _popup.IsOpen = false;
            _step = 0;
        }
    }
}