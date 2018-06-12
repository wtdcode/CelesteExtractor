using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using WinFormsGraphicsDevice;
using System.Windows.Forms;

namespace CelesteExtractor
{
    namespace Celeste
    {
        /// <summary>
        /// 
        /// Similar to the Game class in original assembly.
        /// 
        /// </summary>
        class Celeste
        {
            public static string ContentDirectory { get; set; }
            public static string RootDirectory { get; set; }

            private static GraphicsDevice _graphicsDevice = null;

            private static ContentManager _content = null;

            /// <summary>
            /// This property is a bit hacking to get a GraphicsDevice.
            /// 
            /// Extracting textures in Celeste (or in XNA) needs to construct Texture2D objects
            /// which requires a valid GraphicsDevice.
            /// 
            /// Since it is a command-line app, in order to get the GraphicsDevice,
            /// I have to create an invisible form to acquire a handle and get the device.
            /// </summary>
            private static void Initialize()
            {
                Form form = new Form(); // Invisible form to get a valid GraphicsDevice.
                GraphicsDeviceService gds = GraphicsDeviceService.AddRef(form.Handle, form.ClientSize.Width, form.ClientSize.Height);
                ServiceContainer services = new ServiceContainer();
                services.AddService<IGraphicsDeviceService>(gds);
                _content = new ContentManager(services, "Content");
                IGraphicsDeviceService graphicsDeviceService = services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
                _graphicsDevice = graphicsDeviceService.GraphicsDevice;
            }

            
            public static GraphicsDevice graphicsDevice
            {
                get
                {
                    if (_graphicsDevice == null)
                    {
                        Initialize();
                    }
                    return _graphicsDevice;
                }
            }

            public static ContentManager Content
            {
                get
                {
                    if(_content == null)
                    {
                        Initialize();
                    }
                    return _content;
                }
            }
        }
    }
}
