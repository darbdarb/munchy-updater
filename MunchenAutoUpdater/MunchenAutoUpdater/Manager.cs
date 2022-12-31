﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using MelonLoader;
using MunchenManager;
using MunchenManager.Utils;
using Newtonsoft.Json;
using System.Security.Cryptography;

[assembly: MelonInfo(typeof(Manager), "Munchen Manager", "1.1", "Scrim")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonPriority]
[assembly: MelonColor(ConsoleColor.DarkRed)]

namespace MunchenManager
{
    internal class Manager : MelonPlugin
    {
        private Manager()
        {
            UserError.Check();
        }

        public override void OnPreInitialization()
        {
            MelonLogger.Msg("Initializing Updater...");
            UnpackMunchen.Initialize();
            MelonLogger.Msg("Starting ZIP Process...");
            UnpackMunchen.Start();
            MelonLogger.Msg(ConsoleColor.Green, "All operations completed!");
        }

        public override void OnEarlyInitializeMelon()
        {
            MelonLogger.Msg("Verifying file integrity...");
            //Updater.VerifyIntegrity();
            MelonLogger.Msg("Done!");
        }

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg(ConsoleColor.Green, "Game Initialized!");
            MelonLogger.Msg(ConsoleColor.White, "Enjoy using Munchen :)");
        }
    }

    public static class UnpackMunchen
    {
        private static readonly WebClient WC = new WebClient();

        public static void Initialize()
        {
            File.Delete(MelonUtils.GameDirectory + "\\Mods\\M�nchenClient.dll");
            MelonLogger.Msg(ConsoleColor.Yellow, "Starting Munchen checks...");
            {
                //if (File.Exists(MelonUtils.GameDirectory + "\\Mods\\M�nchenClient.dll")) File.Delete(MelonUtils.GameDirectory + "\\Mods\\M�nchenClient.dll");
                //if (File.Exists(MelonUtils.GameDirectory + "\\Mods\\MünchenClient.dll")) File.Delete(MelonUtils.GameDirectory + "\\Mods\\MünchenClient.dll");
                if (File.Exists(Environment.CurrentDirectory + "\\MunchenFiles.zip")) File.Delete(Environment.CurrentDirectory + "\\MunchenFiles.zip");
                WC.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
                var responseString = WC.DownloadString("https://api.github.com/repos/LessUnderRated/Munchin-Client-Repasted/releases/latest");
                dynamic data = JsonConvert.DeserializeObject(responseString);
                string downloadUrl = data.assets[1].browser_download_url;
                Console.WriteLine(downloadUrl);
                WC.DownloadFile(downloadUrl, Environment.CurrentDirectory + "\\MunchenFiles.zip");
            }
        }

        public static async void Start()
        {
            try
            {
                MelonLogger.Msg(ConsoleColor.Yellow, "Getting Munchen ZIP...");
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
                    var response = await client.GetAsync("https://api.github.com/repos/LessUnderRated/Munchin-Client-Repasted/releases/latest");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        dynamic data = JsonConvert.DeserializeObject(content);
                        string downloadUrl = data.assets[1].browser_download_url;
                        Console.WriteLine(downloadUrl);
                        using (var fileStream = new FileStream(Environment.CurrentDirectory + "\\MunchenFiles.zip", FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                        using (var contentStream = await client.GetStreamAsync(downloadUrl))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        MelonLogger.Msg(ConsoleColor.Red, "Failed to download MunchenZip!");
                        // log the response status code for further diagnosis
                        Console.WriteLine(response.StatusCode);
                    }
                }

                MelonLogger.Msg(ConsoleColor.Green, "Done!");
            }
            catch
            {
                MelonLogger.Msg(ConsoleColor.Red, "Failed to download MunchenZip!");
            }

            MelonLogger.Msg("Unpacking Munchen...");
            if (Directory.Exists(Environment.CurrentDirectory + "\\M�nchenClient")) Directory.Delete(Environment.CurrentDirectory + "\\M�nchenClient", true);

            ManagerUtils.UnpackZip($"{MelonUtils.GameDirectory}");
            string file1Path = MelonUtils.GameDirectory + "\\Mods\\M�nchenClient.dll";
            string file2Path = MelonUtils.GameDirectory + "\\Mods\\MünchenClient.dll";

            bool filesAreEqual = true;

            using (var stream1 = File.OpenRead(file1Path))
            using (var stream2 = File.OpenRead(file2Path))
            {
                var sha256 = new SHA256Managed();
                byte[] hash1 = sha256.ComputeHash(stream1);
                byte[] hash2 = sha256.ComputeHash(stream2);

                if (hash1.Length != hash2.Length)
                {
                    filesAreEqual = false;
                }
                else
                {
                    for (int i = 0; i < hash1.Length; i++)
                    {
                        if (hash1[i] != hash2[i])
                        {
                            filesAreEqual = false;
                            break;
                        }
                    }
                }
            }

            if (filesAreEqual)
            {
                MelonLogger.Msg("The files have the same hash.");
                File.Delete(MelonUtils.GameDirectory + "\\Mods\\M�nchenClient.dll");
            }
            else
            {
                MelonLogger.Msg("The files have different hashes. Replaceing File");
                if (File.Exists(MelonUtils.GameDirectory + "\\Mods\\M�nchenClient.dll")) File.Copy(MelonUtils.GameDirectory + "\\Mods\\M�nchenClient.dll", MelonUtils.GameDirectory + "\\Mods\\MünchenClient.dll", true);
            }
            try
            {
                if (Directory.Exists(Environment.CurrentDirectory + "\\MünchenClient"))
                {
                    MelonLogger.Msg("MünchenClient Folder Exists Moving ClientAssets");
                    try
                    {
                        string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\M�nchenClient\\Dependencies\\ClientAssets");
                        foreach (string file in files)
                        {
                            string fileName = Path.GetFileName(file);
                            string destFile = Path.Combine(Environment.CurrentDirectory + "\\MünchenClient\\Dependencies\\ClientAssets", fileName);
                            File.Copy(file, destFile, true);
                        }

                        Directory.Delete(MelonUtils.GameDirectory + "\\M�nchenClient");
                    }
                    catch
                    {
                    }
                }
                else
                {
                    MelonLogger.Msg("MünchenClient Folder Doesn't Exists Moving All Files");
                    Directory.Move(Environment.CurrentDirectory + "\\M�nchenClient",
                        Environment.CurrentDirectory + "\\MünchenClient");
                }
            }
            catch
            {
            }

            if (Directory.Exists($"{MelonUtils.GameDirectory}\\MünchenClient"))
            {
                MelonLogger.Msg(ConsoleColor.Green, "MünchenClient files unpacked successfully!");
                File.Delete(Environment.CurrentDirectory + "\\MunchenFiles.zip");
                Directory.Delete(MelonUtils.GameDirectory + "\\M�nchenClient", true);
            }

        }
    }
}