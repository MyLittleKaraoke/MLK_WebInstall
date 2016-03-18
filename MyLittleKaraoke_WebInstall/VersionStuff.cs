using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLittleKaraoke_WebInstall
{
    class VersionStuff
    {
        HelperClass cHelper = new HelperClass();

        public string GetSongVersion(string InstallFilePath)
        {
            return File.ReadAllLines(Path.Combine(Path.Combine(InstallFilePath, "songs"), "version.txt")).First().ToString();
        }

        public void SetSongVersion(string InstallFilePath, string SongVersion)
        {
            File.WriteAllText(Path.Combine(Path.Combine(InstallFilePath, "songs"), "version.txt"), SongVersion);
        }

        public string GetPackageVersion(string InstallFilePath)
        {
            return File.ReadAllLines(Path.Combine(InstallFilePath, "version.txt")).First().ToString();
        }

        public void SetPackageVersion(string InstallFilePath, string PackageVersion)
        {
            File.WriteAllText(Path.Combine(InstallFilePath, "songs"), PackageVersion);
        }

        public bool IsMlkSimAC3(string InstallPath)
        {
            try
            {
                return System.IO.File.ReadAllLines(InstallPath + @"\songs\version.txt").First().Contains("5.0 Final");
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
