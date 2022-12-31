using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace MunchenManager.Utils
{
    internal class ManagerUtils
    {
        public static string MunchenURL = "http://vrchat.scrimcreations.tk/hosted/munchen/MunchenClient.dll";
        public static string Munchenname1 = "MunchenClient.dll", Munchenname2 = "MünchenClient.dll";
        public static string MunchenURL_GH = "http://vrchat.scrimcreations.tk/hosted/munchen/gh_redirect.txt";
        public static string PlguinsFolder = $"{MelonUtils.GameDirectory}\\Plugins";
        public static string ModsFolder = $"{MelonUtils.GameDirectory}\\Mods";
        public static string M1 = $"{ModsFolder}\\{Munchenname1}", M2 = $"{ModsFolder}\\{Munchenname2}";
        public static string MunchenZipFileurl = "http://vrchat.scrimcreations.tk/hosted/munchen/munchenzipfile.txt";
        public static string MunchenZipFileLoc = $"{MelonUtils.GameDirectory}\\MunchenFiles.zip";

        public static string GetBackupUrl()
        {
            WebClient wc = new WebClient();
            string getfromhostedurl = MunchenURL_GH;
            string githuburl = wc.DownloadString(getfromhostedurl);
            return githuburl;
        }

        public static void UnpackZip(string path)
        {
            if(path != null)
            {
                string ZipPath = MunchenZipFileLoc;
                string ExtractedPath = path;
                //ZipFile.ExtractToDirectory(ZipPath, ExtractedPath);
                ZipFile.ExtractToDirectory(ZipPath, ExtractedPath);
            }
        }

        public static string GetAssemName()
        {
            if (File.Exists(M1))
            {
                AssemblyName assemname1 = AssemblyName.GetAssemblyName(M1);
                string a1string = assemname1.Name.ToString();
                return a1string;
            }
            else if(File.Exists(M2))
            {
                AssemblyName assemname2 = AssemblyName.GetAssemblyName(M2);
                string a2string = assemname2.Name.ToString();
                return a2string;
            }
            else
            {
                return "string not found";
            }
        }
    }
}
