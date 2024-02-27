using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.GitHub
{
    [DataContract]
    public class GitHubResponseLink
    {
        [DataMember(Name = "self")]
        public string self { get; set; }
        [DataMember(Name = "git")]
        public string git { get; set; }
        [DataMember(Name = "html")]
        public string html { get; set; }
    }
}
