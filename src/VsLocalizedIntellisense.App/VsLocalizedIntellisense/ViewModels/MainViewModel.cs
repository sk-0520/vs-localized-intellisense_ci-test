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
using System.IO;
using VsLocalizedIntellisense.Models.Service.CommandShell;

namespace VsLocalizedIntellisense.ViewModels
{
    public class MainViewModel : SingleModelViewModelBase<MainElement>
    {
        #region variable

        private bool _isDownloading = false;
        private bool _isDownloaded = false;
        private bool _isExecuting = false;

        private string _installCommand = string.Empty;

        private DelegateCommand _selectInstallRootDirectoryPathCommand;
        private AsyncDelegateCommand _downloadCommand;
        private AsyncDelegateCommand _executeCommand;

        bool _filterTrace;
        bool _filterDebug;
        bool _filterInformation;
        bool _filterWarning;
        bool _filterError;
        bool _filterCritical;

        #endregion

        public MainViewModel(MainElement model, ObservableCollection<LogItemElement> stockLogItems, AppConfiguration configuration, ILoggerFactory loggerFactory)
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
                AddItems = p =>
                {
                    Messenger.Send(new ScrollMessage());
                }
            });

            StockLogItems = StockLogCollection.GetDefaultView();
            StockLogItems.Filter += StockLogItems_Filter;
        }

        #region property

        [Messenger]
        public Messenger Messenger { get; } = new Messenger();

        private AppConfiguration Configuration { get; }

        private Dictionary<DirectoryElement, IList<FileInfo>> InstallItems { get; } = new Dictionary<DirectoryElement, IList<FileInfo>>();
        private CommandShellEditor GeneratedCommandShellEditor { get; set; }

        public bool FilterTrace
        {
            get => this._filterTrace;
            set => SetVariable(ref this._filterTrace, value);
        }
        public bool FilterDebug
        {
            get => this._filterDebug;
            set => SetVariable(ref this._filterDebug, value);
        }
        public bool FilterInformation
        {
            get => this._filterInformation;
            set => SetVariable(ref this._filterInformation, value);
        }
        public bool FilterWarning
        {
            get => this._filterWarning;
            set => SetVariable(ref this._filterWarning, value);
        }
        public bool FilterError
        {
            get => this._filterError;
            set => SetVariable(ref this._filterError, value);
        }
        public bool FilterCritical
        {
            get => this._filterCritical;
            set => SetVariable(ref this._filterCritical, value);
        }


        public string InstallCommand
        {
            get => this._installCommand;
            set => SetVariable(ref this._installCommand, value);
        }

        [Required(ErrorMessageResourceName = nameof(Properties.Resources.UI_Validation_Required), ErrorMessageResourceType = typeof(Properties.Resources))]
        public string InstallRootDirectoryPath
        {
            get => Model.InstallRootDirectoryPath;
            set => SetModel(value);
        }

        public bool IsDownloading
        {
            get => this._isDownloading;
            set => SetVariable(ref this._isDownloading, value);
        }

        public bool IsDownloaded
        {
            get => this._isDownloaded;
            set
            {
                SetVariable(ref this._isDownloaded, value);
                this._executeCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsExecuting
        {
            get => this._isExecuting;
            set
            {
                SetVariable(ref this._isExecuting, value);
                this._executeCommand.RaiseCanExecuteChanged();
                this._downloadCommand.RaiseCanExecuteChanged();
            }
        }


        private ModelViewModelObservableCollectionManager<DirectoryElement, DirectoryViewModel> DirectoryCollection { get; }
        public ICollectionView DirectoryItems => DirectoryCollection.GetDefaultView();

        private ModelViewModelObservableCollectionManager<LogItemElement, LogItemViewModel> StockLogCollection { get; }
        public ICollectionView StockLogItems { get; }

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
                    this._downloadCommand = new AsyncDelegateCommand(
                        async _ =>
                        {
                            IsDownloaded = false;
                            IsDownloading = true;
                            try
                            {
                                var items = await Model.DownloadIntellisenseFilesAsync();
                                InstallItems.Clear();
                                foreach (var pair in items)
                                {
                                    InstallItems.Add(pair.Key, pair.Value);
                                }
                                GeneratedCommandShellEditor = Model.GenerateShellCommand(InstallItems);
                                InstallCommand = GeneratedCommandShellEditor.ToSourceCode();
                                IsDownloaded = true;
                            }
                            finally
                            {
                                IsDownloading = false;
                            }
                        },
                        _ => !IsExecuting
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
                    this._executeCommand = new AsyncDelegateCommand(
                        async _ =>
                        {
                            IsExecuting = true;
                            try
                            {
                                await Model.ExecuteCommandShellAsync(GeneratedCommandShellEditor);
                            }
                            finally
                            {
                                IsExecuting = false;
                            }
                        },
                        _ => IsDownloaded && !IsExecuting
                    );
                }
                return this._executeCommand;
            }
        }

        #endregion

        private bool StockLogItems_Filter(object obj)
        {
            var item = obj as LogItemViewModel;
            if (item == null)
            {
                return false;
            }

            if (FilterTrace)
            {
                return Logging.IsEnabled(LogLevel.Trace, item.Level);
            }
            if (FilterDebug)
            {
                return Logging.IsEnabled(LogLevel.Debug, item.Level);
            }
            if (FilterInformation)
            {
                return Logging.IsEnabled(LogLevel.Information, item.Level);
            }
            if (FilterWarning)
            {
                return Logging.IsEnabled(LogLevel.Warning, item.Level);
            }
            if (FilterError)
            {
                return Logging.IsEnabled(LogLevel.Error, item.Level);
            }
            if (FilterCritical)
            {
                return Logging.IsEnabled(LogLevel.Critical, item.Level);
            }

            return true;
        }

    }
}
