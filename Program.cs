using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Net;
using System.IO;

namespace github_d3ify
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 2)
            {
                Console.WriteLine("Error: unrecognized or incomplete command line.\n");
                Console.WriteLine("USAGE:");
                Console.WriteLine("\tgithub-d3ify <Github Username> <output file>");
            }
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                WebClient wc = new WebClient();
                wc.Headers.Add("User-Agent:  RepoDataDownloader - JamieH");
                string githubdata;
                try
                {
                    githubdata = wc.DownloadString("https://api.github.com/users/" + args[0] + "/repos");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                    throw;
                }

                var d3obj = getD3File(githubdata);
                var d3json = JsonConvert.SerializeObject(d3obj);
                using (StreamWriter writer = new StreamWriter(args[1], false))
                {
                    writer.Write(d3json);
                }
                Console.WriteLine("Wrote to file {0}", args[1]);
            }

        }

        public static D3.Repository getD3File(string githubjson)
        {

            dynamic github = JsonConvert.DeserializeObject<dynamic>(githubjson);

            Dictionary<string, List<Github.Project>> languages = new Dictionary<string, List<Github.Project>>();

            foreach(dynamic s in github)
            {
                if (s.fork != true)
                {
                    if(!languages.ContainsKey(s.language.ToString()))
                    {
                        var list = new List<Github.Project>();
                        languages.Add(s.language.ToString(), list);
                    }

            
                    Github.Project ghp = new Github.Project();
                    ghp.name = s.name;
                    if (ghp.size >= 34)
                    {
                        ghp.size = 34;
                    }
                    else
                    {
                        ghp.size = s.size;
                    }
                    ghp.language = s.language;
                    ghp.description = s.description;
                    ghp.stargazers_count = s.stargazers_count;
                    ghp.forks_count = s.forks_count;
                    languages[s.language.ToString()].Add(ghp);
                }
            }

            D3.Repository repo = new D3.Repository();
            repo.name = "GitHub-Projects";
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
                    d3prog.desc = ghproj.description;
                    d3prog.stars = ghproj.stargazers_count;
                    d3prog.forks = ghproj.forks_count;
                    d3prog.language = ghproj.language;
                    children.children.Add(d3prog);
                }

                Console.WriteLine("Language: {0} has {1} projects", lang, children.children.Count);
                repo.children.Add(children);
            }

            return repo;
        }
    }
}
