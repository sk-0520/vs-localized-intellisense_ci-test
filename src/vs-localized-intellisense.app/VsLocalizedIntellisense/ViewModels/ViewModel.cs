using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Binding;
using VsLocalizedIntellisense.Models.Logger;

namespace VsLocalizedIntellisense.ViewModels
{
    public abstract class ViewModelBase : BindModelBase
    {
        protected ViewModelBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        #endregion
    }

    public abstract class SingleModelViewModelBase<TModel> : ViewModelBase
    {
        protected SingleModelViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Model = model;
        }

        #region property

        protected TModel Model { get; }

        #endregion
    }
}
