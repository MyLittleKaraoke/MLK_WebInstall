using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLittleKaraoke_WebInstall
{
    class RemovalHelper
    {
        string MLK_FilePath = "";

        public RemovalHelper(string InstallFilePath)
        {
            MLK_FilePath = InstallFilePath;
        }

        HelperClass cHelper = new HelperClass;
        public Boolean DetectIfUninstallTodo()
        {
            try
            {
                if (cHelper.IsMlkSimAC3(MLK_FilePath))
                {
                    return true;
                }
                else if (true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
