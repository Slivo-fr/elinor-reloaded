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

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int GetWindowText(int hwnd, StringBuilder text, int count);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetOpenClipboardWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetWindowTextLength(int hwnd);

        internal static void SetClipboardWrapper(double d)
        {
            bool copied = false;
            int Attempts = 0;

            while (copied == false && Attempts < 3)
            {
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

        private static Process getProcessLockingClipboard()
        {
            int processId;

            GetWindowThreadProcessId(GetOpenClipboardWindow(), out processId);

            return Process.GetProcessById(processId);
        }

        private static string getOpenClipboardWindowText()
        {
            var hwnd = GetOpenClipboardWindow();
            if (hwnd == IntPtr.Zero)
            {
                return "Unknown";
            }
            var int32Handle = hwnd.ToInt32();
            var len = GetWindowTextLength(int32Handle);
            var sb = new StringBuilder(len);
            GetWindowText(int32Handle, sb, len);
            return sb.ToString();
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