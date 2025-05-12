using Microsoft.AspNetCore.Mvc;
using Octokit;
using Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConnectToGitHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubConnectionController : ControllerBase
    {

        public readonly IGitHubService _service;
        public GitHubConnectionController(IGitHubService service)
        {
            _service = service;
        }


        // GET: api/<GitHubConnectionController>
        [HttpGet]
        public async Task<IActionResult> GetPortfolio() {
            try
            {
                var result= await _service.GetPortfolio();
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRepositories(string? repoName = null, Language? language = null, string? username = null)
        {
            try
            {
                var repositories = await _service.SearchRepositories(repoName, language, username);
                 return Ok(repositories);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }


        
    }
}
