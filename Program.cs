using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Net;
namespace github_d3ify
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            WebClient wc = new WebClient();
            wc.Headers.Add("User-Agent:  RepoDataDownloader - JamieH");
            string githubdata = wc.DownloadString("https://api.github.com/users/JamieH/repos");

            getD3File(githubdata);
        }

        public static D3.Repository getD3File(string githubjson)
        {

            dynamic github = JsonConvert.DeserializeObject<dynamic>(githubjson);

            foreach(dynamic s in github)
            {
                Console.WriteLine("---");
                Console.WriteLine(s.name);
                Console.WriteLine(s.size);
                Console.WriteLine(s.language);
                Console.WriteLine("---");
            }
            
            D3.Repository d3 = new D3.Repository();
            Console.ReadLine();
            return d3;
        }
    }
}
