using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Element;
using VsLocalizedIntellisense.Models.Logger;
using VsLocalizedIntellisense.Models.Mvvm.Binding;
using VsLocalizedIntellisense.Models.Mvvm.Binding.Collection;

namespace VsLocalizedIntellisense.ViewModels
{
    public class IntellisenseDirectoryViewModel : SingleModelViewModelBase<IntellisenseDirectoryElement>
    {
        public IntellisenseDirectoryViewModel(IntellisenseDirectoryElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            VersionCollection = new ModelViewModelObservableCollectionManager<IntellisenseVersionElement, IntellisenseVersionViewModel>(Model.VersionItems, new ModelViewModelObservableCollectionOptions<IntellisenseVersionElement, IntellisenseVersionViewModel>()
            {
                ToViewModel = m => new IntellisenseVersionViewModel(m, LoggerFactory),
            });
        }

        #region property

        public string DirectoryName => Model.Directory.Name;

        private ModelViewModelObservableCollectionManager<IntellisenseVersionElement, IntellisenseVersionViewModel> VersionCollection { get; }
        public ICollectionView VersionItems => VersionCollection.GetDefaultView();

        public IntellisenseVersionViewModel SelectedVersion
        {
            get
            {
                return VersionCollection.GetViewModel(Model.SelectedVersion);
            }
            set
            {
                var model = VersionCollection.GetModel(value);
                SetModel(model);
            }
        }

        #endregion
    }
}
