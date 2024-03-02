using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Service.CommandPrompt
{
    public class CommandValue
    {
        public CommandValue()
        { }

        public CommandValue(IEnumerable<string> value)
        {
            Arguments.Add(string.Empty, value.ToList());
        }

        public CommandValue(string key, IEnumerable<string> value)
        {
            Arguments.Add(key, value.ToList());
        }

        public CommandValue(IReadOnlyDictionary<string, IEnumerable<string>> arguments)
        {
            foreach (var pair in arguments)
            {
                Arguments.Add(pair.Key, pair.Value.ToList());
            }
        }




        #region property

        public IDictionary<string, IList<string>> Arguments { get; } = new Dictionary<string, IList<string>>();

        #endregion

        #region function

        public string ToValue()
        {
            if (Arguments.Count == 0)
            {
                return string.Empty;
            }

            var ss = Arguments
                .Select(a => $"{a.Key}={a.Value}")
            ;

            return string.Join(" ", ss);
        }

        #endregion
    }
}
