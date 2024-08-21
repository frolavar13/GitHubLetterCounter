using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubLetterCounter.Models
{
    //Class to deserialize the content of the GitHub API response
    public class RepositoryItem
    {
        public string name { get; set; }
        public string path { get; set; }
        public string type { get; set; }
        public string download_url { get; set; }
        public string url { get; set; }
    }
}
