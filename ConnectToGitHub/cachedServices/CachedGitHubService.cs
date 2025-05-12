using Microsoft.Extensions.Caching.Memory;
using Octokit;
using Service;

namespace ConnectToGitHub.cachedServices
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;
        private const string userPortfolioKey = "userPortfolioKey";
        private const string searchPublicRepositories = "searchPublicRepositories";
        public CachedGitHubService(IGitHubService gitHubService,IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache= memoryCache;
        }

        public async Task<List<RepositoryData>> GetPortfolio()
        {
            if (_memoryCache.TryGetValue(userPortfolioKey, out List<RepositoryData>? result))
                return result;
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                .SetSlidingExpiration(TimeSpan.FromSeconds(10));
            result=await _gitHubService.GetPortfolio();
            _memoryCache.Set(userPortfolioKey, result,cacheOptions);
            return result;
        }

        public async Task<List<Repository>> SearchRepositories(string? repoName = null, Language? language = null, string? username = null)
        {
            if (_memoryCache.TryGetValue(searchPublicRepositories, out List<Repository> result))
                return result;
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                .SetSlidingExpiration(TimeSpan.FromSeconds(10));
            result=await _gitHubService.SearchRepositories(repoName, language, username);
            _memoryCache.Set(searchPublicRepositories,result,cacheOptions);
            return result;
        }
    }
}
