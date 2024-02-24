using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using VsLocalizedIntellisense.Models.Mvvm;
using VsLocalizedIntellisense.Models.Logger;

namespace VsLocalizedIntellisense.Models.Mvvm.Binding
{
    /// <summary>
    /// ビューモデルの基底。
    /// </summary>
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

        /// <summary>
        /// オブジェクトのプロパティに値設定。
        /// <para>変更があれば変更通知を行う。</para>
        /// </summary>
        /// <typeparam name="TObject">対象オブジェクトの型。</typeparam>
        /// <typeparam name="TValue">設定値の型。</typeparam>
        /// <param name="obj">対象オブジェクト。</param>
        /// <param name="value">設定値。</param>
        /// <param name="objectProperty"><paramref name="obj"/>のプロパティ。</param>
        /// <param name="notifyPropertyName">変更通知としてのプロパティ名。</param>
        /// <returns>変更されたか。</returns>
        private protected bool ChangePropertyValue<TObject, TValue>(TObject obj, TValue value, PropertyInfo objectProperty, string notifyPropertyName)
        {
            var propertyValue = (TValue)objectProperty.GetValue(obj);
            if (EqualityComparer<TValue>.Default.Equals(propertyValue, value))
            {
                return false;
            }

            objectProperty.SetValue(obj, value);
            OnPropertyChanged(notifyPropertyName);

            return true;

        }

        /// <summary>
        /// オブジェクトのプロパティに値設定。
        /// <para>変更があれば変更通知を行う。</para>
        /// </summary>
        /// <typeparam name="TObject">対象オブジェクトの型。</typeparam>
        /// <typeparam name="TValue">設定値の型。</typeparam>
        /// <param name="model">対象オブジェクト。</param>
        /// <param name="value">設定値。</param>
        /// <param name="propertyName">プロパティ名。</param>
        /// <param name="notifyPropertyName">変更通知のプロパティ名。基本的に呼び出し側のメンバ名となる想定。</param>
        /// <returns>変更されたか。</returns>
        protected bool SetProperty<TObject, TValue>(TObject model, TValue value, [CallerMemberName] string propertyName = "", [CallerMemberName] string notifyPropertyName = "")
        {
            var type = model.GetType();
            var prop = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // 動的な割当ては考慮していないのでデバッグ時にここで死ぬ場合は何か間違ってる。
            Debug.Assert(prop != null);

            return ChangePropertyValue(model, value, prop, notifyPropertyName);
        }

        #endregion
    }

}
