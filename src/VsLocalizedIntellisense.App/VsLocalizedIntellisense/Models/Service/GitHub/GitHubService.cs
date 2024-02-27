using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.GitHub
{
    public class GitHubService
    {
        #region function

        public Task GetTreeAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
