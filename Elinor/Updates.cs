using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Elinor
{
    class Updates
    {
        public static void CheckForUpdates()
        {
            if (Properties.Settings.Default.checkforupdates)
            {
                Version newVersion = null;
                var url = string.Empty;
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(@"http://cylide.de/elinor/currentVersion.xml");
                    reader.MoveToContent();
                    var elementName = "";
                    if ((reader.NodeType == XmlNodeType.Element) &&
                        (reader.Name == "elinor"))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                                elementName = reader.Name;
                            else
                            {
                                if ((reader.NodeType == XmlNodeType.Text) &&
                                    (reader.HasValue))
                                {
                                    switch (elementName)
                                    {
                                        case "version":
                                            newVersion = new Version(reader.Value);
                                            break;
                                        case "url":
                                            url = reader.Value;
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    if (newVersion != null && url != string.Empty)
                    {
                        var assembly = Assembly.GetExecutingAssembly();
                        var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                        var currentVersion = new Version(fvi.ProductVersion);

                        if (newVersion > currentVersion)
                        {
                            if (DialogResult.Yes ==
                                MessageBox.Show("There's a new version of Elinor available, do you want to download it?",
                                    "New version available",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question))
                            {
                                Process.Start(url);
                            }
                        }
                    }
                }
                    // ReSharper disable EmptyGeneralCatchClause
                catch (Exception) //if it fails, don't bother the user
                    // ReSharper restore EmptyGeneralCatchClause
                {
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
            }
        }
    }
}
