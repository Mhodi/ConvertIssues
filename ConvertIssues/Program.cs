using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ConvertIssues
{
    class Program
    {
        public static string origIssues;
        public static List<Platform> Platforms= new List<Platform>(); 

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                origIssues = GetIssues(args[0]);
            }
            else
            {
                origIssues = GetIssues("originalissues.txt");
            }
            ParseText();
            ExportJson();
        }

        private static void ExportJson()
        {
            string output = JsonConvert.SerializeObject(Platforms);
            File.WriteAllText("issues.json", output);
        }

        static string GetIssues(string file)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string line = sr.ReadToEnd();
                    return line;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem with retrieving issues: " + e.Message);
                return "";
            }
        }

        static void ParseText()
        {
            string[] separators =
            {
                "----------------------\r\n",
                "-----------------------\r\n",
                "------------------------\r\n",
                "-------------------------\r\n",
                "--------------------------\r\n",
                "---------------------------\r\n",
                "----------------------------\r\n",
                "-----------------------------\r\n",
                "------------------------------\r\n",
                "-------------------------------\r\n",
                "--------------------------------\r\n",
                "---------------------------------\r\n",
                "----------------------------------\r\n",
                "-----------------------------------\r\n",
                "------------------------------------\r\n",
                "-------------------------------------\r\n",
                "--------------------------------------\r\n",
                "---------------------------------------\r\n",
                "----------------------------------------\r\n",
                "-----------------------------------------\r\n",
                "------------------------------------------\r\n"
            };
            string[] issueSeparators =
            {
                "\r\n-",
                "(BIOS "
            };
            string[] sep =
            {
                "(Fixed ",
                "(fixed "
            
            };

            //breakup origIssues into substrings from ------- to ---------
            string[] issueStrings = origIssues.Split(separators, StringSplitOptions.RemoveEmptyEntries);


            //build platform objects from all even issueStrings indexes
            for (int i = 0; i < issueStrings.Length; i += 2)
            {
                string[] stuff = issueStrings[i].Split(' ');

                string sys = stuff[0];
                string ssid = stuff[1];
                string os = "";
                if (stuff.Length < 3)
                {
                    os = "Win7";
                }
                else
                {
                    if (stuff.Length > 3)
                    {
                        stuff[2] += stuff[3];
                    }

                    stuff[2] = stuff[2].Replace("\r\n", "");

                    os = stuff[2];
                }

                Platform newSys = new Platform(sys, ssid, os);
                Platforms.Add(newSys);
            }

            for (int j = 1; j <= issueStrings.Length; j += 2)
            {
                string[] issues = issueStrings[j].Split(issueSeparators, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < issues.Length-1; x += 2)
                {
                    string desc = issues[x];

                    string[] breaks = null;
                    string bios = "";
                    if (issues[x + 1].Contains("(Fixed "))
                    {
                        breaks = issues[x + 1].Split(sep, StringSplitOptions.None);
                        bios = breaks[0];
                    }
                    else
                    {
                        bios = issues[x + 1].Replace("\r\n", "");
                    }
                    string fixedin = "";

                    if ((breaks != null) && (breaks.Length > 1))
                    {
                        fixedin = breaks[1].Replace("\r\n", "");
                    }

                    bios = bios.Replace(")", "");
                    fixedin = fixedin.Replace(")", "");

                    Issue newIssue = new Issue(desc, bios, fixedin);

                    if (j - 1 == 0)
                    {
                        Platforms[0].Issues.Add(newIssue);
                    }
                    else
                    {
                        Platforms[(j - 1)/2].Issues.Add(newIssue);
                    }
                }
            }
        }
    }
}
