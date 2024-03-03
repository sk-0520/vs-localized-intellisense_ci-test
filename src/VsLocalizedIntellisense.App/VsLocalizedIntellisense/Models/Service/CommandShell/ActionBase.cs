using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandShell
{
    public abstract class ActionBase
    {
        #region property

        public virtual string CommandName { get; set; }

        #endregion

        #region function

        public abstract string GetStatement();

        public virtual string ToStatement(IndentContext indent)
        {
            return GetStatement();
        }

        #endregion
    }
}
