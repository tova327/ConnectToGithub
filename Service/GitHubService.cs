using System.Collections.Generic;
using System.Linq; // Ensure this is included for FirstOrDefault
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Octokit;
using Service;

public class GitHubService:IGitHubService
{
    private readonly GitHubClient _client;
    private readonly GitHubIntegrationOptions _options;

    public GitHubService(IOptions<GitHubIntegrationOptions> options)
    {
        
        _options=options.Value;
        _client = new GitHubClient(new ProductHeaderValue("YourAppName"))
        {
            Credentials = new Credentials(_options.Token) // Use the user's token
        };
    }

    public async Task<List<RepositoryData>> GetPortfolio()
    {
        var allRepos = await GetUserRepositories(_options.UserName);
        var portfolio = new List<RepositoryData>();
        foreach (var repo in allRepos)
        {
            var tmp=new RepositoryData();
            tmp.Link = repo.Url;
            tmp.LastCommit =await GetLastCommit(repo.Id);
            (tmp.Starts,tmp.PullRequests) = await GetRepoStats(repo.Id);
            tmp.DevLanguage = await GetRepositoryLanguages(repo.Id);
            portfolio.Add(tmp);
        }
        return portfolio;
    }

    // פונקציה לחיפוש repositories ציבוריים

    public async Task<List<Repository>> SearchRepositories(string? repoName = null, Octokit.Language? language = null, string? username = null)
    {
        var searchRequest = new SearchRepositoriesRequest(repoName)
        {
            Language = language,
            User = username,
        };
        var result = await _client.Search.SearchRepo(searchRequest);
        return result.Items.ToList();
    }
    //=====================================================================================
    //=====================================================================================
    //=====================================================================================
    //=====================================================================================
    //=====================================================================================
    //=====================================================================================

    // Function to get all user repositories
    private async Task<IReadOnlyList<Repository>> GetUserRepositories(string username)
    {
        return await _client.Repository.GetAllForUser(username);
    }

    // Function to get specific repository details
    private async Task<Repository> GetRepositoryDetails(long id)
    {
        return await _client.Repository.Get(id);
    }

    // Function to get the last commit of a repository
    private async Task<GitHubCommit?> GetLastCommit(long id)
    {
        var commits = await _client.Repository.Commit.GetAll(id);
        return commits.FirstOrDefault(); // Safely get the first commit
    }

    // Function to get stars and pull requests
    private async Task<(int stars, int pullRequests)> GetRepoStats(long id)
    {
        var repo = await GetRepositoryDetails(id);
        var pullRequests = await _client.PullRequest.GetAllForRepository(id);
        return (repo.StargazersCount, pullRequests.Count);
    }

    // Function for general search in GitHub
    
    // Function to get languages used in a repository
    private async Task<ICollection<RepositoryLanguage>?> GetRepositoryLanguages(long id)
    {
        var languages = await _client.Repository.GetAllLanguages(id);
        return languages.ToList();
    }

}
