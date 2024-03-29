﻿using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Elinor
{
    class MiscTools
    {
        internal static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (FileNotFoundException)
            {
                return true;
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        public static void FlashControl(Control control, Color flashColor, MainWindow main)
        {
            var thread = new Thread(new ThreadStart(delegate
                                                        {
                                                            Brush old = null;
                                                            Color oldColor;
                                                            main.Dispatcher.Invoke(
                                                                new Action(delegate { old = control.BorderBrush; }));
                                                            try
                                                            {
                                                                oldColor = ((SolidColorBrush) old).Color;
                                                            }
                                                            catch (Exception)
                                                            {
                                                                oldColor = Colors.WhiteSmoke;
                                                            }

                                                            for (int i = 0; i < 16; i++)
                                                            {
                                                                int ii = i%4;
                                                                switch (ii)
                                                                {
                                                                    case 0:
                                                                        main.Dispatcher.Invoke(
                                                                            new Action(
                                                                                delegate
                                                                                    {
                                                                                        control.BorderBrush =
                                                                                            new LinearGradientBrush(
                                                                                                flashColor, oldColor, 0);
                                                                                    }));
                                                                        break;

                                                                    case 1:
                                                                        main.Dispatcher.Invoke(
                                                                            new Action(
                                                                                delegate
                                                                                    {
                                                                                        control.BorderBrush =
                                                                                            new SolidColorBrush(
                                                                                                flashColor);
                                                                                    }));
                                                                        break;

                                                                    case 2:
                                                                        main.Dispatcher.Invoke(
                                                                            new Action(
                                                                                delegate
                                                                                    {
                                                                                        control.BorderBrush =
                                                                                            new LinearGradientBrush(
                                                                                                oldColor, flashColor, 0);
                                                                                    }));
                                                                        break;

                                                                    case 3:
                                                                        main.Dispatcher.Invoke(
                                                                            new Action(
                                                                                delegate { control.BorderBrush = old; }));
                                                                        break;
                                                                }
                                                                Thread.Sleep(175);
                                                            }
                                                            main.Dispatcher.Invoke(
                                                                new Action(delegate { control.BorderBrush = old; }));
                                                        }));
            thread.Start();
        }

    }
}
