using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Service.CommandShell.Command;
using VsLocalizedIntellisense.Models.Service.CommandShell.Value;

namespace VsLocalizedIntellisense.Models.Service.CommandShell
{
    public class CommandShellEditor
    {
        #region property

        public CommandShellOptions Options { get; set; } = new CommandShellOptions();

        public List<ActionBase> Actions { get; } = new List<ActionBase>();

        #endregion

        #region function

        #region add-command

        private TCommand AddCommand<TCommand>()
            where TCommand : CommandBase, new()
        {
            var result = new TCommand()
            {
                SuppressCommand = Options.SuppressCommand,
                CommandNameIsUpper = Options.CommandNameIsUpper,
                IndentSpace = Options.IndentSpace,
            };

            Actions.Add(result);

            return result;
        }

        public ChangeCodePageCommand AddChangeCodePage(Encoding encoding)
        {
            var command = AddCommand<ChangeCodePageCommand>();
            command.Encoding = encoding;
            return command;
        }

        public ChangeDirectoryCommand AddChangeDirectory(Express path, bool withDrive = true)
        {
            var command = AddCommand<ChangeDirectoryCommand>();
            command.Path = path;
            command.WithDrive = withDrive;
            return command;
        }

        public ChangeSelfDirectoryCommand AddChangeSelfDirectory()
        {
            var command = AddCommand<ChangeSelfDirectoryCommand>();
            return command;
        }

        public CopyCommand AddCopy(Express source, Express destination, bool isForce = false)
        {
            var command = AddCommand<CopyCommand>();
            command.Source = source;
            command.Destination = destination;
            command.IsForce = isForce;
            return command;
        }

        public EchoCommand AddEcho(Express value)
        {
            var command = AddCommand<EchoCommand>();
            command.Value = value;
            return command;
        }

        public IfErrorLevelCommand AddIfErrorLevel(int level, bool isNot = false)
        {
            var command = AddCommand<IfErrorLevelCommand>();
            command.Level = level;
            command.IsNot = isNot;
            return command;
        }

        public IfExpressCommand AddIfExpress(Express left, Express right, bool isNot = false)
        {
            var command = AddCommand<IfExpressCommand>();
            command.Left = left;
            command.Right = right;
            command.IsNot = isNot;
            return command;
        }

        public IfExistCommand AddIfExist(Express path, bool isNot = false)
        {
            var command = AddCommand<IfExistCommand>();
            command.Path = path;
            command.IsNot = isNot;
            return command;
        }

        public RemarkCommand AddRemark(Express comment)
        {
            var command = AddCommand<RemarkCommand>();
            command.Comment = comment;
            return command;
        }

        public SetVariableCommand AddSetVariable(string variableName, Express value)
        {
            var command = AddCommand<SetVariableCommand>();
            command.VariableName = variableName;
            command.Value = value;
            return command;
        }

        public SwitchEchoCommand AddSwitchEcho(bool isOn)
        {
            var command = AddCommand<SwitchEchoCommand>();
            command.On = isOn;
            return command;
        }


        #endregion

        #endregion
    }
}
