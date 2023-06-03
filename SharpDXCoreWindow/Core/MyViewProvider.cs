using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;

namespace SharpDXCoreWindow.Core
{
    /// <summary>
    /// The view provider class that will handle all the view operations (update/draw).
    /// </summary>
    internal class MyViewProvider : IFrameworkView
    {
        private CoreWindow window;
        private SharpDX.Direct3D11.Device1 device;
        private SharpDX.Direct3D11.DeviceContext1 context;
        private RenderTargetView backBuffer;
        private SwapChain1 swapChain;

        /// <summary>
        /// This function is called before SetWindow, so we can't do much yet.
        /// </summary>
        /// <param name="applicationView"></param>
        public void Initialize(CoreApplicationView applicationView)
        {
        }

        /// <summary>
        /// Now that we have a CoreWindow object, the DirectX device/context can be created.
        /// </summary>
        /// <param name="entryPoint"></param>
        public void Load(string entryPoint)
        {
            // Get the default hardware device and enable debugging. Don't care about the available feature level.
            SharpDX.Direct3D11.Device defaultDevice = new SharpDX.Direct3D11.Device(DriverType.Hardware, DeviceCreationFlags.Debug);

            // Query the default device for the supported device and context interfaces.
            device = defaultDevice.QueryInterface<SharpDX.Direct3D11.Device1>();
            context = device.ImmediateContext.QueryInterface<DeviceContext1>();

            // Query for the adapter and more advanced DXGI objects.
            SharpDX.DXGI.Device2 dxgiDevice2 = device.QueryInterface<SharpDX.DXGI.Device2>();
            SharpDX.DXGI.Adapter dxgiAdapter = dxgiDevice2.Adapter;
            SharpDX.DXGI.Factory2 dxgiFactory2 = dxgiAdapter.GetParent<SharpDX.DXGI.Factory2>();

            // Description for our swap chain settings.
            SwapChainDescription1 description = new SwapChainDescription1()
            {
                // 0 means to use automatic buffer sizing.
                Width = 0,
                Height = 0,
                // 32 bit RGBA color.
                Format = Format.B8G8R8A8_UNorm,
                // No stereo (3D) display.
                Stereo = false,
                // No multisampling.
                SampleDescription = new SampleDescription(1, 0),
                // Use the swap chain as a render target.
                Usage = Usage.RenderTargetOutput,
                // Enable double buffering to prevent flickering.
                BufferCount = 2,
                // No scaling.
                Scaling = Scaling.None,
                // Flip between both buffers.
                SwapEffect = SwapEffect.FlipSequential,
            };

            // Generate a swap chain for our window based on the specified description.
            // swapChain = dxgiFactory2.CreateSwapChainForCoreWindow(device, new ComObject(window), ref description, null);

            swapChain = new SwapChain1(dxgiFactory2, device, new ComObject(window), ref description);

            // Create the texture and render target that will hold our backbuffer.
            Texture2D backBufferTexture = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            backBuffer = new RenderTargetView(device, backBufferTexture);

            backBufferTexture.Dispose();
        }

        /// <summary>
        /// Run our application until the user quits.
        /// </summary>
        public void Run()
        {
            // Make window active and hide mouse cursor.
            window.PointerCursor = null;
            window.Activate();

            var clearColor = new RawColor4(1, 0, 0, 1);

            // Infinite loop to prevent the application from exiting.
            while (true)
            {
                // Dispatch all pending events in the queue.
                window.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);

                // Quit if the users presses Escape key.
                if (window.GetAsyncKeyState(VirtualKey.Escape) == CoreVirtualKeyStates.Down)
                {
                    return;
                }

                // Set the backbuffer as the active rendertarget and clear it with the selected color.
                context.OutputMerger.SetTargets(backBuffer);
                context.ClearRenderTargetView(backBuffer, clearColor);

                // Present the current buffer to the screen.
                swapChain.Present(1, PresentFlags.None);
            }
        }

        /// <summary>
        /// Sets the window where the app will be rendered.
        /// </summary>
        /// <param name="window">Our main window</param>
        public void SetWindow(CoreWindow window)
        {
            this.window = window;
        }

        /// <summary>
        /// Dispose all the created objects.
        /// </summary>
        public void Uninitialize()
        {
            swapChain.Dispose();
            backBuffer.Dispose();
            context.Dispose();
            device.Dispose();
        }
    }
}
