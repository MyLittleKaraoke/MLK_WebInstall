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
            try
            {
                return File.ReadAllLines(Path.Combine(Path.Combine(InstallFilePath, "songs"), "version.txt")).First().ToString();
            }
            catch (Exception)
            {
                return "none";
            }
        }

        public void SetSongVersion(string InstallFilePath, string SongVersion)
        {
            File.WriteAllText(Path.Combine(Path.Combine(InstallFilePath, "songs"), "version.txt"), SongVersion);
        }

        public string GetPackageVersion(string InstallFilePath)
        {
            try
            {
                return File.ReadAllLines(Path.Combine(InstallFilePath, "version.txt")).First().ToString();
            }
            catch (Exception)
            {
                return "none";
            }
        }

        public void SetPackageVersion(string InstallFilePath, string PackageVersion)
        {
            File.WriteAllText(Path.Combine(InstallFilePath,"version.txt"), PackageVersion);
        }

        public bool IsMlkSimAC3(string InstallPath)
        {
            try
            {
                return GetSongVersion(InstallPath).Contains("5.0 Final");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsMlkSimAC3Package(string PackageName)
        {
            try
            {
                return PackageName.Contains("AC3-");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string[,] FilterPackageList(string[,] PackageList, string PackageVersionString)
        {
            try
            {
                int FoundItem = -1;
                string[,] internalPackageList = new string[1, 1]; //without this, the compiler does not trust that internalPackageList is always assigned
                for (int intCurrFile = 0; intCurrFile < PackageList.GetLength(0); intCurrFile++)
                {
                    if (FoundItem > -1)
                    {
                        internalPackageList[intCurrFile - FoundItem - 1, 0] = PackageList[intCurrFile,0];
                        internalPackageList[intCurrFile - FoundItem - 1, 1] = PackageList[intCurrFile, 1];
                    }
                    else //string not yet found
                    {
                        if (PackageList[intCurrFile,0].EndsWith(PackageVersionString))
                        {
                        FoundItem = intCurrFile;
                        internalPackageList = new string[(PackageList.GetLength(0) - intCurrFile), 2];
                        }
                    }
                }
                return internalPackageList;
            }
            catch (Exception ex)
            {
                cHelper.ShowErrorMessageDialog(ex.Message, ex.StackTrace, "FilterPackageList");
                return PackageList;
            }

        }
    }
}
