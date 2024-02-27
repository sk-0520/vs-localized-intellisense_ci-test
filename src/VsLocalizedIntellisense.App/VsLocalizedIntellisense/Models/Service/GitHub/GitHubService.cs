using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Configuration;

namespace VsLocalizedIntellisense.Models.Service.GitHub
{
    public class GitHubService
    {
        public GitHubService(GitHubRepository repository, GitHubOptions options)
        {
            Repository = repository;
            Options = options;
        }

        #region proeprty

        private HttpClient HttpClient { get; } = new HttpClient();
        private GitHubRepository Repository { get; }
        private GitHubOptions Options { get; }

        #endregion

        #region function

        //"https://api.github.com/repos/sk-0520/vs-localized-intellisense/contents/intellisense"

        private HttpRequestMessage CreateRequestMessage(HttpMethod method, Uri uri)
        {
            var request = new HttpRequestMessage(method, uri);

            foreach (var pair in Options.RequestHeaders)
            {
                request.Headers.Add(pair.Key, pair.Value);
            }

            return request;
        }

        private HttpRequestMessage CreateRequestMessage(HttpMethod method, string uri)
            => CreateRequestMessage(method, new Uri(uri));

        private async Task<T> RequestAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await HttpClient.SendAsync(request, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync();
            var serializer = new DataContractJsonSerializer(typeof(T));
            var rawContent = serializer.ReadObject(stream);
            return (T)rawContent;
        }

        public async Task<IReadOnlyList<GitHubContentResponseItem>> GetContentsAsync(string path, CancellationToken cancellationToken = default)
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
            var request = CreateRequestMessage(HttpMethod.Get, url);
            var response = await RequestAsync<GitHubContentResponseItem[]>(request, cancellationToken);
            return response;
        }

        #endregion
    }
}
