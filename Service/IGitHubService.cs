using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IGitHubService
    {
        public  Task<List<RepositoryData>> GetPortfolio();
        public Task<List<Repository>> SearchRepositories(string? repoName = null, Octokit.Language? language = null, string? username = null);
    }
}
