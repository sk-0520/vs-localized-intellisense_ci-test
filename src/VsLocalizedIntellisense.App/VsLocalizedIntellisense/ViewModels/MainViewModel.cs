using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VsLocalizedIntellisense.Models.Mvvm;
using VsLocalizedIntellisense.Models.Element;
using VsLocalizedIntellisense.Models.Logger;
using VsLocalizedIntellisense.ViewModels.Message;

namespace VsLocalizedIntellisense.ViewModels
{
    public class MainViewModel : SingleModelViewModelBase<MainElement>
    {
        #region variable

        DelegateCommand _selectInstallRootDirectoryPathCommand;

        #endregion

        public MainViewModel(MainElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        [Messenger]
        public Messenger Messenger { get; } = new Messenger();

        public string InstallRootDirectoryPath
        {
            get => Model.InstallRootDirectoryPath;
            set => SetModel(value);
        }

        #endregion

        #region command

        public ICommand SelectInstallRootDirectoryPathCommand
        {
            get
            {
                if (this._selectInstallRootDirectoryPathCommand == null)
                {
                    this._selectInstallRootDirectoryPathCommand = new DelegateCommand(
                        o =>
                        {
                            var message = new OpenFileDialogMessage()
                            {
                            };
                            Messenger.Send(message);
                        }
                    );
                }
                return this._selectInstallRootDirectoryPathCommand;
            }
        }

        #endregion
    }
}
