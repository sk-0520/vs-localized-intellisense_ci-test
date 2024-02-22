using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        #region function



        #endregion
    }

    public abstract class SingleModelViewModelBase<TModel> : ViewModelBase
        where TModel : BindModelBase
    {
        protected SingleModelViewModelBase(TModel model, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Model = model;
        }

        #region property

        protected TModel Model { get; }

        #endregion

        #region function

        protected void SetModel<T>(T value, [CallerMemberName] string propertyName = "")
        {
            Model.SetProperty(value, propertyName);
            OnPropertyChanged(propertyName);
        }

        #endregion
    }
}
