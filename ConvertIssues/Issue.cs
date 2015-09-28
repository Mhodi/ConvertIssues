using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConvertIssues
{
    public class Issue
    {
        public string description { get; set; }
        public string bios { get; set; }
        public string fixedIn { get; set; }

        public Issue(string d, string b, string f)
        {
            description = d;
            bios = b;
            fixedIn = f;
        }

    }
}
