using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Models.Logger
{
    public interface ILoggerFactory : IDisposable
    {
        #region function

        ILogger CreateLogger(string categoryName);

        #endregion
    }

    public static class ILoggerFactoryExtensions
    {
        #region function

        public static ILogger CreateLogger(this ILoggerFactory factory, Type type)
        {
            return factory.CreateLogger(type.FullName);
        }

        public static ILogger CreateLogger<T>(this ILoggerFactory factory)
        {
            return CreateLogger(factory, typeof(T));
        }

        #endregion
    }
}
