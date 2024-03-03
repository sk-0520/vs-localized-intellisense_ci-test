using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandShell
{
    public class CommandShellOptions
    {
        #region property

        public bool SuppressCommand { get; set; }

        public string IndentSpace { get; set; } = "\t";

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        #endregion
    }
}
