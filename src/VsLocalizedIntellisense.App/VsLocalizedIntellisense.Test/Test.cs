using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VsLocalizedIntellisense.Test
{
    internal class Test
    {
        public static DirectoryInfo GetProjectDirectory()
        {
            var projectTestPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)/*!*/;

            return new DirectoryInfo(projectTestPath);
        }

        public static DirectoryInfo GetClassDirectory(object test)
        {
            var project = GetProjectDirectory();
            var classTestDirPath = Path.Combine(project.FullName, test.GetType().FullName ?? throw new Exception(test.ToString()));

            return new DirectoryInfo(classTestDirPath);
        }

        public static DirectoryInfo GetMethodDirectory(object test, string callerMemberName, int callerLineNumber)
        {
            var classTestDirPath = GetClassDirectory(test);
            var methodTestDirPath = Path.Combine(classTestDirPath.FullName, callerMemberName + "-L" + callerLineNumber.ToString(CultureInfo.InvariantCulture));

            return new DirectoryInfo(methodTestDirPath);
        }

    }
}
