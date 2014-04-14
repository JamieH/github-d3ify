using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace github_d3ify
{
    class D3
    {
        public class Project
        {
            public string name { get; set; }
            public int size { get; set; }
            public string language { get; set; }
            public int stars { get; set; }
            public int forks { get; set; }
            public string desc { get; set; }
        }

        public class Language
        {
            public string name { get; set; }
            public List<Project> children { get; set; }
        }

        public class Repository
        {
            public string name { get; set; }
            public List<Language> children { get; set; }
        }
    }
}
