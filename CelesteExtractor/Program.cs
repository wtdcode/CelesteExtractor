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
using MyMonocle;
using System.IO;

namespace CelesteExtractor
{
    class Extrator
    {
        private static string _output_dir = @"I:\Temps\Images\Celeste2";

        struct AtlasInfo
        {
            public string Path { get;private set; }
            public Atlas.AtlasDataFormat Format { get;private set; }
            public AtlasInfo(string path, Atlas.AtlasDataFormat format)
            {
                this.Path = path;
                this.Format = format;
            }
        }

        private static readonly Dictionary<string, AtlasInfo> _components = new Dictionary<string, AtlasInfo> {
            { "Mountains", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Mountain"), Atlas.AtlasDataFormat.PackerNoAtlas) },
            { "CheckPoints", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Checkpoints"), Atlas.AtlasDataFormat.Packer)},
            { "Openning", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Opening"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "Gui", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Gui"), Atlas.AtlasDataFormat.Packer)},
            { "Journal", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Journal"), Atlas.AtlasDataFormat.Packer)},
            { "Misc", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Misc"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "GamePlay", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Gameplay"), Atlas.AtlasDataFormat.Packer)},
            { "OverWorld", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Overworld"), Atlas.AtlasDataFormat.PackerNoAtlas)}
        };

        private static void _extract(string type)
        {
            Console.WriteLine("Start to extract {0}", type);
            AtlasInfo info = _components[type];
            Atlas atlas = Atlas.FromAtlas(info.Path, info.Format);
            foreach (VirtualTexture texture in atlas.Sources)
            {
                Console.WriteLine("Writing {0} to specified output directory...", Path.GetFileName(texture.Path));
                texture.SaveAsPng(Path.Combine(_output_dir, texture.Path) + ".png");
            }
        }

        static void WaitExit()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            if(args.Length != 2)
            {
                Console.WriteLine("Celeste Extractor : Extracting all atlas in Celeste.");
                Console.WriteLine("Usage: CelesteExtractor [ContentDirectory] [OutputDirectory]");
                WaitExit();
            }
            else
            {
                if (!Directory.Exists(args[0]))
                {
                    Console.WriteLine("Content directory doesn't exist!");
                    WaitExit();
                }
                else
                {
                    Celeste.Celeste.ContentDirectory = args[0];
                }
                _output_dir = args[1];
            }
            foreach(string type in _components.Keys)
            {
                try
                {
                    _extract(type);
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Directory not found. Please check the input directory!");
                    WaitExit();
                    return;
                }
            }
            Console.WriteLine("Done!");
            WaitExit();

        }
    }
}
