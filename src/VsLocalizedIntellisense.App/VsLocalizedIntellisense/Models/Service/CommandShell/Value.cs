using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace VsLocalizedIntellisense.Models.Service.CommandShell
{
    public abstract class ValueBase
    {
        #region function

        public abstract string Expression { get; }

        #endregion

        #region operator

        public static Value operator +(ValueBase a, ValueBase b)
        {
            var result = new Value();

            result.Values.Add(a);
            result.Values.Add(b);

            return result;
        }

        #endregion
    }

    public class Text : ValueBase
    {
        public Text(string data)
        {
            Data = data;
        }

        #region property

        public string Data { get; set; }

        #endregion

        #region ValueBase

        public override string Expression => Data;

        #endregion
    }

    public class Variable : ValueBase
    {
        #region variable

        private string _name;

        #endregion

        public Variable(string name, bool isReadOnly = false)
        {
            Name = name;
            IsReadOnly = isReadOnly;
        }

        #region property

        public bool IsReadOnly { get; }

        public string Name
        {
            get => this._name;
            set
            {
                if (IsReadOnly)
                {
                    throw new InvalidOperationException("readonly");
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                this._name = value;
            }
        }

        public bool DelayedExpansion { get; set; }

        #endregion


        #region Value

        public override string Expression
        {
            get
            {
                if (DelayedExpansion)
                {
                    return $"!{Name}!";
                }

                return $"%{Name}%";
            }
        }

        #endregion
    }

    public class Value : ValueBase
    {
        #region proeprty

        public IList<ValueBase> Values { get; } = new List<ValueBase>();

        #endregion

        #region operator

        public static implicit operator Value(string text)
        {
            var result = new Value();
            result.Values.Add(new Text(text));

            return result;
        }

        #endregion

        #region ValueBase

        public override string Expression
        {
            get
            {
                return string.Join(string.Empty, Values.Select(a => a.Expression));
            }
        }

        #endregion
    }
}
