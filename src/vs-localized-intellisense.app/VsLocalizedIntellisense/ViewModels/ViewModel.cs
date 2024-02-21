using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsLocalizedIntellisense.Models.Binding;

namespace VsLocalizedIntellisense.ViewModels
{
    public abstract class ViewModelBase: BindModelBase
    {
    }

    public abstract class SingleModelViewModelBase<TModel>
    {
        protected SingleModelViewModelBase(TModel model)
        {
            Model = model;
        }

        #region property

        protected TModel Model { get; }

        #endregion
    }
}
