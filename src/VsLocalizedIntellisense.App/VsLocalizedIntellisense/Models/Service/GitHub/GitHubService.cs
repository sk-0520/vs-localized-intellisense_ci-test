using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Configuration;

namespace VsLocalizedIntellisense.Models.Service.GitHub
{
    public class GitHubService
    {
        public GitHubService(GitHubRepository repository, AppConfiguration configuration)
        {
            Repository = repository;
            Configuration = configuration;
        }

        #region proeprty

        private GitHubRepository Repository { get; }
        private AppConfiguration Configuration { get; }

        #endregion

        #region function

        //"https://api.github.com/repos/sk-0520/vs-localized-intellisense/contents/intellisense"

        public async Task<GitHubContentResponseItem[]> GetTreeAsync(string path)
        {
            var url = Strings.ReplaceFromDictionary(
                "https://api.github.com/repos/${OWNER}/${NAME}/contents/${PATH}",
                new Dictionary<string, string>()
                {
                    ["OWNER"] = Repository.Owner,
                    ["NAME"] = Repository.Name,
                    ["PATH"] = path.Trim('/'),
                }
            );
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", Configuration.GetHttpUserAgent());
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("X-GitHub-Api-Version", "2022-11-28");

            var response = await client.SendAsync(request);
            var rawContent = await response.Content.ReadAsStreamAsync();

            var serializer = new DataContractJsonSerializer(typeof(GitHubContentResponseItem[]));
            var content = serializer.ReadObject(rawContent);

            return (GitHubContentResponseItem[])content;
        }

        #endregion
    }
}
