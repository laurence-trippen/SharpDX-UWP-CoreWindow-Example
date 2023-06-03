using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;

namespace SharpDXCoreWindow.Core
{
    internal class MyViewProviderFactory : IFrameworkViewSource
    {
        public IFrameworkView CreateView()
        {
            return new MyViewProvider();
        }
    }
}
