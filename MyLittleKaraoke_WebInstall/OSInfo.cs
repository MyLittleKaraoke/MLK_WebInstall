using System;


namespace OperatingSystemInfo
{
    /// <summary>
    ///  Represents information about an operating system name  
    /// </summary>
    public sealed class OSInfo
    {
        /// <summary>
        /// Array to hold OS name 
        /// </summary>
        string[] osNameArray = new string[] { 
            "Unknown OS", "Windows 3.1", "Windows 95", "Windows 98", 
            "Windows 98 Second Edition", "Windows ME", "Windows NT 3.51",
            "Windows NT 4.0", "Windows 2000", "Windows XP", 
            "Windows 2003","Windows Vista", "Windows 2008", 
            "Windows 7","Windows 8","Windows 8.1","Windows 10","Windows CE", "Unix"};

        /// <summary>
        /// Get current Operating System name as string type
        /// </summary>
        public string GetOSName { get; private set; }

        /// <summary>
        /// Get OSNameEnum values that indicates the operating system name
        /// </summary>
        public OSNameEnum GetOSNameEnum { get; private set; }

        /// <summary>
        /// Gets a System.PlatformID enumeration value that identifies the operating
        /// system platform.
        /// </summary>
        public PlatformID Platform
        {
            get
            {
                return Environment.OSVersion.Platform;
            }
        }

        /// <summary>
        /// Gets the service pack version represented by this System.OperatingSystem
        /// object.
        /// The service pack version, if service packs are supported and 
        /// installed; otherwise, an empty string ("").
        /// </summary>
        public string ServicePack
        {
            get
            {
                return Environment.OSVersion.ServicePack;
            }
        }

        /// <summary>
        /// Gets a System.Version that describes the major version, minor version,
        /// build, and revision numbers for the operating system.
        /// </summary>
        public Version Version 
        {
            get
            {
                return Environment.OSVersion.Version;
            }
        }

        /// <summary>
        /// Gets the concatenated string representation of the platform identifier, version,
        /// and service pack that are currently installed on the operating system.
        /// </summary>
        public string VersionString
        {
            get
            {
             return  Environment.OSVersion.VersionString;
            }
        }

        /// <summary>
        /// Initializes a new instance of the OSInfo.
        /// </summary>
        public OSInfo()
        {
            // Retrieve OS info
            GetOSNameEnum = GetOSInfo();
            GetOSName = osNameArray[(int)GetOSNameEnum];
        }

        /// <summary>
        /// Function to get the name of current Operating System.
        /// </summary>
        /// <returns> Name of the OS as OSNameEnum type.</returns>
        private OSNameEnum GetOSInfo()
        {
            OSNameEnum osName = OSNameEnum.Unknown;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32S:
                    // Windows 3.1
                    osName = OSNameEnum.Win_3_1;
                    break;

                case PlatformID.Win32Windows:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            // "Windows 95"
                            osName = OSNameEnum.Win_95;
                            break;

                        case 10:
                            if (Environment.OSVersion.Version.Revision.ToString() == "2222A")
                            {
                                //"Windows 98 Second Edition"
                                osName = OSNameEnum.Win_98_SE;
                            }
                            else
                            {
                                //"Windows 98"
                                osName = OSNameEnum.Win_98;
                            }
                            break;

                        case 90:
                            // "Windows ME"
                            osName = OSNameEnum.Win_ME;
                            break;
                    }
                    break;

                case PlatformID.Win32NT:
                    switch (Environment.OSVersion.Version.Major)
                    {
                        case 3:
                            // "Windows NT 3.51"
                            osName = OSNameEnum.Win_NT_3_51;
                            break;

                        case 4:
                            // "Windows NT 4.0"
                            osName = OSNameEnum.Win_NT_4_0;
                            break;

                        case 5:
                            switch (Environment.OSVersion.Version.Minor)
                            {
                                case 0:
                                    // "Windows 2000"
                                    osName = OSNameEnum.Win_2000;
                                    break;

                                case 1:
                                    // "Windows XP"
                                    osName = OSNameEnum.Win_XP;
                                    break;

                                case 2:
                                    // "Windows 2003"
                                    osName = OSNameEnum.Win_2003;
                                    break;
                            }
                            break;

                        case 6:
                            switch (Environment.OSVersion.Version.Minor)
                            {
                                case 0:
                                    // "Windows Vista"
                                    osName = OSNameEnum.Win_Vista;
                                    break;

                                case 1:
                                    // "Windows 7"
                                    osName = OSNameEnum.Win_7;
                                    break;

                                case 2:
                                    // "Windows 8"
                                    osName = OSNameEnum.Win_8;
                                    break;

                                case 3:
                                    // "Windows 8.1"
                                    osName = OSNameEnum.Win_81;
                                    break;
                            }
                            break;

                        case 10:
                            switch (Environment.OSVersion.Version.Minor)
                            {
                                case 0:
                                    //Windows 10
                                    osName = OSNameEnum.Win_10;
                                break;
                            }
                            break;
                            
                    }
                    break;

                case PlatformID.WinCE:
                    // "Windows CE"
                    osName = OSNameEnum.Win_CE;
                    break;

                case PlatformID.Unix:
                    // "Unix"
                    osName = OSNameEnum.Unix;
                    break;
            }
            return osName;
        }
    }

    /// <summary>
    /// Identifies the Operating System,
    /// </summary>
    public enum OSNameEnum
    {
        /// <summary>
        /// Unknown OS
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The operating system is Windows 3.1  
        /// </summary>
        Win_3_1,
        
        /// <summary>
        /// The operating system is Windows 95
        /// </summary>
        Win_95,
        
        /// <summary>
        /// The operating system is Windows 98
        /// </summary>
        Win_98,
        
        /// <summary>
        /// The operating system is Windows 98 Second Edition
        /// </summary>
        Win_98_SE,
       
        /// <summary>
        /// The operating system is Windows ME
        /// </summary>
        Win_ME,
        
        /// <summary>
        /// The operating system is Windows NT 3.51
        /// </summary>
        Win_NT_3_51,
        
        /// <summary>
        /// The operating system is Windows NT 4.0
        /// </summary>
        Win_NT_4_0,
        
        /// <summary>
        /// The operating system is Windows 2000
        /// </summary>
        Win_2000,
        
        /// <summary>
        /// The operating system is Windows XP
        /// </summary>
        Win_XP,
        
        /// <summary>
        /// The operating system is Windows 2003
        /// </summary>
        Win_2003,
        
        /// <summary>
        /// The operating system is Windows Vista
        /// </summary>
        Win_Vista,
        
        /// <summary>
        /// The operating system is Windows 2008
        /// </summary>
        Win_2008,
        
        /// <summary>
        /// The operating system is Windows 7
        /// </summary>
        Win_7,

        /// <summary>
        /// The operating system is Windows 8
        /// </summary>
        Win_8,

        /// <summary>
        /// The operating system is Windows 8.1
        /// </summary>
        Win_81,

        /// <summary>
        /// The operating system is Windows 10
        /// </summary>
        Win_10,
        
        /// <summary>
        /// The operating system is Windows CE
        /// </summary>
        Win_CE,
        
        /// <summary>
        /// The operating system is Unix
        /// </summary>
        Unix,
    }
}
