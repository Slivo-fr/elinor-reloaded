using System;
using System.Globalization;
using System.Windows;

namespace Elinor
{
    internal class ClipboardTools
    {
        internal static void SetClipboardWrapper(double d)
        {
            Clipboard.SetText(d > .01 ? Math.Round(d, 2).ToString(CultureInfo.InvariantCulture) : string.Empty);
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
                result += result*settings.buyPercentage > settings.buyThreshold
                              ? settings.buyThreshold
                              : settings.buyPercentage*result;
            }
            else
            {
                result += .01;
            }

            return result;
        }
    }
}