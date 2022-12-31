using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MunchenManager.Utils
{
    internal class UserError
    {
        public static void Check()
        {
            if(File.Exists(ManagerUtils.ModsFolder + $"\\MunchenManager (1).dll") 
                || File.Exists(ManagerUtils.ModsFolder + $"\\MunchenManager.dll"))
            {
                MelonLogger.Msg(ConsoleColor.Yellow, "[!] ---------------------------------------------- [!]");
                MelonLogger.Msg(ConsoleColor.Red, "Please put the Munchen Manager DLL in plugins!");
                MelonLogger.Msg(ConsoleColor.Red, "Please put the Munchen Manager DLL in plugins!");
                MelonLogger.Msg(ConsoleColor.Red, "Please put the Munchen Manager DLL in plugins!");
                MelonLogger.Msg(ConsoleColor.Red, "Please put the Munchen Manager DLL in plugins!");
                MelonLogger.Msg(ConsoleColor.Yellow, "[!] ---------------------------------------------- [!]");
                MelonLogger.Msg(ConsoleColor.White, "It's not a Mod you idiot it's a Plugin");
                Thread.Sleep(-1);
            }
        }

        public static void Fix()
        {
            //Will do later
        }
    }
}
