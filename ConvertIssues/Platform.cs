using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertIssues
{
    public class Platform
    {
        public string SystemName { get; set; }
        public string SSID { get; set; }

        public string OS { get; set; }
        public List<Issue> Issues { get; set; }

        public Platform(string sn, string ssid, string os)
        {
            SystemName = sn;
            SSID = ssid;
            OS = os;
            Issues = new List<Issue>();
        }

        public void AddIssue(Issue i)
        {
            Issues.Add(i);
        }

    }
}
