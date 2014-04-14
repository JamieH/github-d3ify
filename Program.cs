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
            
            var d3obj = getD3File(githubdata);
            var d3json = JsonConvert.SerializeObject(d3obj);
            
            Console.WriteLine(d3json);
            Console.ReadLine();
        }

        public static D3.Repository getD3File(string githubjson)
        {

            dynamic github = JsonConvert.DeserializeObject<dynamic>(githubjson);

            Dictionary<string, List<Github.Project>> languages = new Dictionary<string, List<Github.Project>>();

            foreach(dynamic s in github)
            {
                if(!languages.ContainsKey(s.language.ToString()))
                {
                    var list = new List<Github.Project>();
                    languages.Add(s.language.ToString(), list);
                }
                
                
                    Github.Project ghp = new Github.Project();
                    ghp.name = s.name;
                    ghp.size = s.size;
                    ghp.language = s.language;
                    languages[s.language.ToString()].Add(ghp);
               
                //Console.WriteLine("---");
                //Console.WriteLine(s.name);
                //Console.WriteLine(s.size);
                //Console.WriteLine(s.language);
                //Console.WriteLine("---");
            }

            D3.Repository repo = new D3.Repository();
            repo.name = "flare";
            repo.children = new List<D3.Language>();

            foreach(string lang in languages.Keys)
            {
                D3.Language children = new D3.Language();
                children.name = lang;
                children.children = new List<D3.Project>();

                foreach(Github.Project ghproj in languages[lang])
                {
                    D3.Project d3prog = new D3.Project();
                    d3prog.name = ghproj.name;
                    d3prog.size = ghproj.size;
                    children.children.Add(d3prog);
                }
                Console.WriteLine("Language: {0} has {1} projects", lang, children.children.Count);
                repo.children.Add(children);
            }

            return repo;
        }
    }
}
