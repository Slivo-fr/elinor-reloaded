using System.Collections.Generic;
using System.IO;

namespace Elinor
{
    internal class CSVReader
    {
        internal static List<List<string>> GetTableFromCSV(string path)
        {
            var result = new List<List<string>>();
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(new FileStream(path, FileMode.Open));
                sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    var readLine = sr.ReadLine();
                    if (readLine != null) result.Add(new List<string>(readLine.Split(",".ToCharArray())));
                }
                return result;
            }
            catch (IOException)
            {
                return null;
            }
            finally
            {
                if (sr != null) sr.Close();
            }
        }
    }
}