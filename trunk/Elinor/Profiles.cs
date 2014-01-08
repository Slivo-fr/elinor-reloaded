using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Elinor
{
    public class Profiles
    {
        private static readonly DirectoryInfo _profdir = new DirectoryInfo("profiles");

        public static List<Profile> getAllProfiles() 
        {
            List<Profile> profiles = new List<Profile>();

            foreach (FileInfo file in _profdir.GetFiles())
            {
                Profile profile = Profile.ReadSettings(file.Name.Replace(file.Extension, ""));

                if (profile != null)
                {
                    profiles.Add(profile);
                }
            }

            return profiles;
        }

    }
}
