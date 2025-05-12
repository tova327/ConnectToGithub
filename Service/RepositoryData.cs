using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RepositoryData
    {
        public ICollection<RepositoryLanguage>? DevLanguage { get; set; }
        public GitHubCommit? LastCommit { get; set; }
        public int Starts { get; set; }
        public int PullRequests { get; set; }
        public string? Link { get; set; }
    }
}
