using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.GitHub
{
    [DataContract]
    public class GitHubContentResponseItem
    {
        [DataMember(Name = "name")]
        public string name { get; set; }
        [DataMember(Name = "path")]
        public string path { get; set; }
        [DataMember(Name = "sha")]
        public string sha { get; set; }
        [DataMember(Name = "size")]
        public int size { get; set; }
        [DataMember(Name = "url")]
        public string url { get; set; }
        [DataMember(Name = "html_url")]
        public string html_url { get; set; }
        [DataMember(Name = "git_url")]
        public string git_url { get; set; }
        [DataMember(Name = "download_url")]
        public object download_url { get; set; }
        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "_links")]
        public GitHubResponseLink Link { get; set; }
    }



}
