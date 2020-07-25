using System;
using System.IO;

using Graphal.Tools.Abstractions.Application;

namespace Graphal.Tools.Services.Application
{
    public class ApplicationStandardPaths : IApplicationStandardPaths
    {
        public string UserApplicationSettings =>
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public string UserLocalStorage =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Graphal");
    }
}