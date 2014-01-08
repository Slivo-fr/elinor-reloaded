using System.IO;

namespace Elinor
{
    class CacheTools
    {
        internal static void ClearMarketLogs(DirectoryInfo logdir)
        {
            foreach (FileInfo fi in logdir.GetFiles())
            {
                if (!MiscTools.IsFileLocked(fi))
                    fi.Delete();
            }
        }

        internal static void ClearApiCache()
        {
            if (Directory.Exists("Cache"))
            {
                foreach (string file in Directory.GetFiles("Cache"))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    // ReSharper disable EmptyGeneralCatchClause
                    catch //no fucks given
                    // ReSharper restore EmptyGeneralCatchClause
                    {
                    }
                }
            }
        }
    }
}
