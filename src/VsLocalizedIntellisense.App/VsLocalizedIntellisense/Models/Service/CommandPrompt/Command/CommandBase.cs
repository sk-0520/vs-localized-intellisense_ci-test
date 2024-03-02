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

        public bool? SuppressOutput { get; set; }
        public bool? SuppressCommand { get; set; }

        public CommandValue Value { get; set; } = new CommandValue();

        #endregion

        #region function

        public string GetStatementCommandName()
        {
            return SuppressCommand.GetValueOrDefault()
                ? "@" + this._commandName
                : this._commandName
            ;
        }

        #endregion

        #region function

        public virtual string GetValues()
        {
            if (Value.Arguments.Count == 0)
            {
                return GetStatementCommandName();
            }

            return $"{GetStatementCommandName()} {Value.ToValue()}";
        }

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
