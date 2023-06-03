using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using SharpDX.Mathematics.Interop;
using SharpDXCoreWindow.Core;

namespace SharpDXCoreWindow
{
    internal static class Program
    {
        [MTAThread]
        private static void Main()
        {
            // Tell CoreApplication to create a view instance through the factory.
            MyViewProviderFactory factory = new MyViewProviderFactory();
            CoreApplication.Run(factory);
        }
    }
}
