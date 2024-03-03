using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandPrompt.Command
{
    public abstract class CommandBase : ActionBase
    {
        #region variable

        private readonly string _commandName;

        #endregion

        public CommandBase(string name)
        {
            this._commandName = name;
        }

        #region property

        public bool? SuppressCommand { get; set; }
        public bool? CommandNameIsUpper { get; set; }

        #endregion

        #region function

        public string GetStatementCommandName()
        {
            var commandName = CommandNameIsUpper.GetValueOrDefault()
                ? this._commandName.ToUpperInvariant()
                : this._commandName
            ;

            return SuppressCommand.GetValueOrDefault()
                ? "@" + commandName
                : commandName
            ;
        }

        #endregion

        #region function


        #endregion

        #region ActionBase

        public override string CommandName
        {
            get => this._commandName;
            set => throw new NotSupportedException(nameof(CommandName));
        }

        #endregion
    }
}
