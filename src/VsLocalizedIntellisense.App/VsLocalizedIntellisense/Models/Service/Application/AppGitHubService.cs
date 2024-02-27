using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Configuration;
using VsLocalizedIntellisense.Models.Service.GitHub;

namespace VsLocalizedIntellisense.Models.Service.Application
{
    public class AppGitHubService : GitHubService
    {
        public AppGitHubService(AppConfiguration configuration)
            : base(
                 new GitHubRepository(configuration.GetRepositoryOwner(), configuration.GetRepositoryName()),
                 new GitHubAuthentication(),
                 new GitHubOptions()
                 {
                     RequestHeaders = new Dictionary<string, string>()
                     {
                         ["User-Agent"] = configuration.GetHttpUserAgent(),
                         ["Accept"] = "application/vnd.github+json",
                         ["X-GitHub-Api-Version"] = "2022-11-28",
                     }
                 }
            )
        { }
    }
}
