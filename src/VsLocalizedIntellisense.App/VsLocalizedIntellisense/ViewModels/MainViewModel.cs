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
using VsLocalizedIntellisense.Models.Mvvm.Binding;
using VsLocalizedIntellisense.Models.Mvvm.Command;
using VsLocalizedIntellisense.Models.Mvvm.Message;
using VsLocalizedIntellisense.Models;
using System.ComponentModel.DataAnnotations;
using VsLocalizedIntellisense.Models.Mvvm.Binding.Collection;
using System.ComponentModel;

namespace VsLocalizedIntellisense.ViewModels
{
    public class MainViewModel : SingleModelViewModelBase<MainElement>
    {
        #region variable

        DelegateCommand _selectInstallRootDirectoryPathCommand;

        #endregion

        public MainViewModel(MainElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            IntellisenseDirectoryCollection = new ModelViewModelObservableCollectionManager<IntellisenseDirectoryElement, IntellisenseDirectoryViewModel>(Model.IntellisenseDirectoryElements, new ModelViewModelObservableCollectionOptions<IntellisenseDirectoryElement, IntellisenseDirectoryViewModel>()
            {
                ToViewModel = m => new IntellisenseDirectoryViewModel(m, LoggerFactory),
            });
        }

        #region property

        [Messenger]
        public Messenger Messenger { get; } = new Messenger();

        [Required(ErrorMessageResourceName = nameof(Properties.Resources.UI_Validation_Required), ErrorMessageResourceType = typeof(Properties.Resources))]
        public string InstallRootDirectoryPath
        {
            get => Model.InstallRootDirectoryPath;
            set => SetModel(value);
        }

        private ModelViewModelObservableCollectionManager<IntellisenseDirectoryElement, IntellisenseDirectoryViewModel> IntellisenseDirectoryCollection { get; }
        public ICollectionView IntellisenseDirectoryItems => IntellisenseDirectoryCollection.GetDefaultView();

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
                                Kind = OpenFileDialogKind.Directory,
                                CurrentDirectory = IOHelper.GetPhysicalDirectory(InstallRootDirectoryPath),
                            };
                            Messenger.Send(message);

                            if (message.ResultDirectory != null)
                            {
                                InstallRootDirectoryPath = message.ResultDirectory.FullName;
                            }
                        }
                    );
                }
                return this._selectInstallRootDirectoryPathCommand;
            }
        }

        #endregion
    }
}
