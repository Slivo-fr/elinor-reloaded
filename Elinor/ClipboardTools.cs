using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows;




namespace Elinor
{
    internal class ClipboardTools
    {
        internal static void SetClipboardWrapper(double d)
        {
            bool copied = false;
            int Attempts = 0;

            while (copied == false && Attempts < 3)
            {
                Clipboard.Clear();
                try
                {
                    Clipboard.SetText(d > .01 ? Math.Round(d, 2).ToString(CultureInfo.InvariantCulture) : string.Empty);
                    copied = true;
                }
                catch (Exception) 
                {
                    Attempts ++;
                    Thread.Sleep(500 * Attempts);
                }
            }
        }
            
        internal static double GetSellPrice(double sell, Profile settings)
        {
            if (settings == null) return .0;

            double result = sell;

            if (settings.advancedStepSettings)
            {
                result -= (result - result*settings.sellPercentage) > settings.sellThreshold
                              ? (result - result*settings.sellPercentage)
                              : settings.sellThreshold;
            }
            else
            {
                result -= .01;
            }

            return result;
        }

        internal static double GetBuyPrice(double buy, Profile settings)
        {
            if (settings == null) return .0;

            double result = buy;

            if (settings.advancedStepSettings)
            {
                result += (result - result*settings.buyPercentage) > settings.buyThreshold
                              ? (result - result*settings.buyPercentage)
                              : settings.buyThreshold;
            }
            else
            {
                result += .01;
            }

            return result;
        }
    }
}
