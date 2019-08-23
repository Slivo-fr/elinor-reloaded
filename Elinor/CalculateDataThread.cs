using System;
using System.Data;
using System.Windows.Media;

namespace Elinor
{
    internal class CalculateDataThread
    {
        private readonly double _buyPrice;
        private readonly MainWindow _main;
        private readonly double _sellPrice;

        public CalculateDataThread(double sell, double buy, MainWindow main)
        {
            _sellPrice = sell;
            _buyPrice = buy;
            _main = main;
        }

        internal static double BuyBrokerFee(Profile profile)
        {
            if (profile.useBuyCustomBroker)
            {
                return profile.buyCustomBroker;
            }
            else
            {
                return NpcBroker(profile);
            }
        }

        internal static double SellBrokerFee(Profile profile)
        {
            if (profile.useSellCustomBroker)
            {
                return profile.sellCustomBroker;
            }
            else
            {
                return NpcBroker(profile);
            }
        }

        internal static double NpcBroker(Profile profile)
        {
            return (5 - (profile.brokerRelations * 0.3 + profile.factionStanding * 0.03 + profile.corpStanding * 0.02)) / 100;
        }

        internal static double SalesTax(int accounting)
        {
            return .050*(1 - (accounting*.11));
        }

        internal void Run()
        {
            if (_sellPrice < 0 || _buyPrice < 0)
            {
                _main.Dispatcher.Invoke(new Action(delegate
                {
                    _main.lblRevenue.Content = "- ISK";
                    _main.lblCoS.Content = "- ISK";
                    _main.lblProfit.Content = "- ISK";
                    _main.lblMargin.Content = "- %";
                    _main.lblMarkup.Content = "- %";

                    _main.brdImportant.BorderBrush =
                        new SolidColorBrush(Colors.LightGray);
                    _main.lblMargin.Foreground = new SolidColorBrush(Colors.Black);
                }));
            }
            else
            {
                double buyBrokerFee = BuyBrokerFee(_main.profile);
                double sellBrokerFee = SellBrokerFee(_main.profile);
                double salesTax = SalesTax(_main.profile.accounting);


                double revenue = ((_sellPrice - .01) - (_sellPrice - .01)* sellBrokerFee - (_sellPrice - .01)*salesTax);
                double cos = (_buyPrice + .01) + (_buyPrice + .01)*buyBrokerFee;

                double costOfBuyOrder = _buyPrice * buyBrokerFee;
                double costOfSellOrder = _sellPrice * sellBrokerFee + _sellPrice * salesTax;
   

                _main.Dispatcher.Invoke(new Action(delegate
                {
                    _main.lblRevenue.Content = String.Format("{0:n} ISK", revenue);
                    _main.lblCoS.Content = String.Format("{0:n} ISK", cos);

                    _main.lblBuyOrderCost.Content = String.Format("{0:n} ISK", costOfBuyOrder);
                    _main.lblSellOrderCost.Content = String.Format("{0:n} ISK", costOfSellOrder);


                    _main.lblProfit.Content = String.Format("{0:n} ISK",
                                                            revenue - cos);

                    double margin = 100*(revenue - cos)/revenue;
                    _main.lblMargin.Content = Math.Abs(margin) < 10000
                                                    ? String.Format("{0:n}%",
                                                                    margin)
                                                    : (margin > 0 ? "∞%" : "-∞%");

                    double markup = 100 * (revenue - cos) / cos;
                    _main.lblMarkup.Content = Math.Abs(markup) < 10000
                                                    ? String.Format("{0:n}%",
                                                                    markup)
                                                    : (markup > 0 ? "∞%" : "-∞%");

                    if (margin/100 >= _main.profile.marginThreshold)
                    {
                        _main.lblMargin.Foreground =
                            new SolidColorBrush(Colors.ForestGreen);
                        _main.brdImportant.BorderBrush =
                            new LinearGradientBrush(Colors.LimeGreen,
                                                    Colors.ForestGreen, 45.0);
                    }
                    else if (margin/100 > _main.profile.minimumThreshold)
                    {
                        _main.lblMargin.Foreground =
                            new SolidColorBrush(Colors.Orange);
                        _main.brdImportant.BorderBrush =
                            new LinearGradientBrush(Colors.Yellow, Colors.Orange,
                                                    45.0);
                    }
                    else
                    {
                        _main.lblMargin.Foreground =
                            new SolidColorBrush(Colors.Red);
                        _main.brdImportant.BorderBrush =
                            new LinearGradientBrush(Colors.OrangeRed, Colors.Red,
                                                    45.0);
                    }

                }));
            }
        }
    }
}