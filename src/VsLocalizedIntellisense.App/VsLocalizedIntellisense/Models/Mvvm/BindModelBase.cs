using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Mvvm
{
    /// <summary>
    /// MVVM で使用するモデル基底。
    /// </summary>
    public abstract class BindModelBase : NotifyPropertyBase
    {
        /// <summary>
        /// プロパティ値変更処理。
        /// <para>プロパティ対象となるフィールドを指定し、変更があれば変更通知を行う。</para>
        /// </summary>
        /// <typeparam name="T">プロパティの型。</typeparam>
        /// <param name="variable">プロパティの実体となるフィールド。</param>
        /// <param name="value">設定する値。</param>
        /// <param name="notifyPropertyName">プロパティ名。</param>
        /// <returns>変更されたか。</returns>
        protected bool SetVariable<T>(ref T variable, T value, [CallerMemberName] string notifyPropertyName = "")
        {
            if(EqualityComparer<T>.Default.Equals(variable, value))
            {
                return false;
            }

            variable = value;
            OnPropertyChanged(notifyPropertyName);

            return true;
        }
    }
}
