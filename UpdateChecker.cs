﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForgeModBuilder
{
    public static class UpdateChecker
    {
        public static string UpdateURL = "https://raw.githubusercontent.com/CJMinecraft01/ForgeModBuilder/master/update.json";

        public static void CheckForUpdates(string url)
        {
            WebClient client = new WebClient();
            Console.WriteLine("Checking for updates at URL: " + url);
            Program.INSTANCE.AddConsoleText("Checking for updates at URL: " + url + "\n");
            try
            {
                string data = client.DownloadString(url);
                Update update = JsonConvert.DeserializeObject<Update>(data);
                if(Application.ProductVersion != update.version)
                {
                    Console.WriteLine("An update is available!");
                    Program.INSTANCE.AddConsoleText("An update is available! \n");
                    string changelog = "";
                    foreach(string line in update.changelog)
                    {
                        changelog += line + "\n";
                    }
                    if(MessageBox.Show("An update is available! \n" + update.name + "\n" + update.version + "\n" + changelog + "\nWould you like to update now?", "Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        Process.Start(update.download);
                    }
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
                ForgeModBuilder.Program.INSTANCE.AddConsoleText("An error occurred" + e.Message + "\n");
                MessageBox.Show("An error occurred: " + e.Message, "An error occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class Update
        {
            public string name;
            public string version;
            public List<string> changelog;
            public string download;

            public Update(string name, string version, List<string> changelog, string download)
            {
                this.name = name;
                this.version = version;
                this.changelog = changelog;
                this.download = download;
            }
        }
    }
}
