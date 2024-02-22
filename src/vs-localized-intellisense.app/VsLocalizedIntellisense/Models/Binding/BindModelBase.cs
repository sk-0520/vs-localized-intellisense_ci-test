using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Binding
{
    /// <summary>
    /// MVVM で使用するモデル基底。
    /// </summary>
    public abstract class BindModelBase : NotifyPropertyBase
    {
        protected void SetVariable<T>(ref T variable, T value, [CallerMemberName] string propertyName = "")
        {
            variable = value;
            OnPropertyChanged(propertyName);
        }

        public void SetProperty<T>(T value, [CallerMemberName] string propertyName = "")
        {
            var type = GetType();
            var prop = type.GetProperty(propertyName);

            prop.SetValue(this, value);
            OnPropertyChanged(propertyName);
        }
    }
}
