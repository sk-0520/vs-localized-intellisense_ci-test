using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandPrompt
{
    public class IndentContext
    {
        public IndentContext(string space = "\t", int level = 0)
        {
            Space = space;
            Level = level;
        }

        #region property

        public string Space { get; }
        public int Level { get; }

        #endregion

        #region function

        public IndentContext Nest()
        {
            return new IndentContext(Space, Level + 1);
        }

        #endregion
    }
}
