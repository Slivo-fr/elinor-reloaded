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
                result -= (result*settings.sellPercentage > settings.sellThreshold)
                              ? settings.sellThreshold
                              : settings.sellPercentage*result;
            }
            else
            {
                result -= Math.Max(Math.Pow(10, Math.Floor(Math.Log10(result / 1000))), 0.01);
            }

            return result;
        }

        internal static double GetBuyPrice(double buy, Profile settings)
        {
            if (settings == null) return .0;

            double result = buy;

            if (settings.advancedStepSettings)
            {
                result += result*settings.buyPercentage > settings.buyThreshold
                              ? settings.buyThreshold
                              : settings.buyPercentage*result;
            }
            else
            {
                result += Math.Max(Math.Pow(10, Math.Floor(Math.Log10(result / 1000))), 0.01);
            }

            return result;
        }
    }
}