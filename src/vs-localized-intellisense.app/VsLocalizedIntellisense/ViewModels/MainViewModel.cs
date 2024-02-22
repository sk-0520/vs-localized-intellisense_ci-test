using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Element;
using VsLocalizedIntellisense.Models.Logger;

namespace VsLocalizedIntellisense.ViewModels
{
    public class MainViewModel : SingleModelViewModelBase<MainElement>
    {
        public MainViewModel(MainElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public string InstallRootDirectoryPath
        {
            get => Model.InstallRootDirectoryPath;
            set => SetModel(value);
        }

        #endregion
    }
}
