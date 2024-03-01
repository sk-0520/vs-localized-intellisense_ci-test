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
using VsLocalizedIntellisense.Models.Configuration;
using System.Collections.ObjectModel;

namespace VsLocalizedIntellisense.ViewModels
{
    public class MainViewModel : SingleModelViewModelBase<MainElement>
    {
        #region variable

        private bool _isDownloaded = false;

        private DelegateCommand _selectInstallRootDirectoryPathCommand;
        private DelegateCommand _downloadCommand;
        private DelegateCommand _executeCommand;

        #endregion

        public MainViewModel(MainElement model, ObservableCollection<LogItemElement> stockLogItems , AppConfiguration configuration, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Configuration = configuration;
            DirectoryCollection = new ModelViewModelObservableCollectionManager<DirectoryElement, DirectoryViewModel>(Model.IntellisenseDirectoryElements, new ModelViewModelObservableCollectionOptions<DirectoryElement, DirectoryViewModel>()
            {
                ToViewModel = m => new DirectoryViewModel(m, LoggerFactory),
            });

            StockLogCollection = new ModelViewModelObservableCollectionManager<LogItemElement, LogItemViewModel>(stockLogItems, new ModelViewModelObservableCollectionOptions<LogItemElement, LogItemViewModel>()
            {
                ToViewModel = m => new LogItemViewModel(m, LoggerFactory),
            });
        }

        #region property

        [Messenger]
        public Messenger Messenger { get; } = new Messenger();

        private AppConfiguration Configuration { get; }

        [Required(ErrorMessageResourceName = nameof(Properties.Resources.UI_Validation_Required), ErrorMessageResourceType = typeof(Properties.Resources))]
        public string InstallRootDirectoryPath
        {
            get => Model.InstallRootDirectoryPath;
            set => SetModel(value);
        }

        public bool IsDownloaded
        {
            get => this._isDownloaded;
            set => SetVariable(ref this._isDownloaded, value);
        }

        private ModelViewModelObservableCollectionManager<DirectoryElement, DirectoryViewModel> DirectoryCollection { get; }
        public ICollectionView DirectoryItems => DirectoryCollection.GetDefaultView();

        private ModelViewModelObservableCollectionManager<LogItemElement, LogItemViewModel> StockLogCollection { get; }
        public ICollectionView StockLogItems => StockLogCollection.GetDefaultView();

        public string AppVersion => Configuration.Replace("${APP:VERSION}");

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

        public ICommand DownloadCommand
        {
            get
            {
                if (this._downloadCommand == null)
                {
                    this._downloadCommand = new DelegateCommand(
                        _ =>
                        {

                        }
                    );
                }
                return this._downloadCommand;
            }
        }

        public ICommand ExecuteCommand
        {
            get
            {
                if (this._executeCommand == null)
                {
                    this._executeCommand = new DelegateCommand(
                        _ =>
                        {

                        },
                        _ => IsDownloaded
                    );
                }
                return this._executeCommand;
            }
        }

        #endregion
    }
}
