using System;
using Graphal.Tools.Abstractions.Application;

namespace Graphal.Tools.Services.Application
{
    public class ApplicationStandardPaths : IApplicationStandardPaths
    {
        public string UserApplicationSettings =>
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }
}