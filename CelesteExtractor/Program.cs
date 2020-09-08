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
            // OVR
            { "Overworld", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Overworld"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            // GFX
            { "GamePlay", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Gameplay"), Atlas.AtlasDataFormat.Packer)},
            { "Openning", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Opening"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "Gui", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Gui"), Atlas.AtlasDataFormat.Packer)},
            { "Misc", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Misc"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "Portraits", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Portraits"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            // MTN
            { "FileSelect", new AtlasInfo(Path.Combine("Graphics", "Atlases", "FileSelect"), Atlas.AtlasDataFormat.Packer)},
            { "Journal", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Journal"), Atlas.AtlasDataFormat.Packer)},
            { "Mountains", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Mountain"), Atlas.AtlasDataFormat.PackerNoAtlas) },
            { "CheckPoints", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Checkpoints"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            // CS10_Ending
            { "Farewell", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Farewell"), Atlas.AtlasDataFormat.PackerNoAtlas) },
            // WaveDash
            { "WaveDash", new AtlasInfo(Path.Combine("Graphics", "Atlases", "WaveDashing"), Atlas.AtlasDataFormat.Packer) },
            // From my guessing
            { "CelestialResort", new AtlasInfo(Path.Combine("Graphics", "Atlases", "CelestialResort"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "Core", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Core"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "Cliffside", new AtlasInfo(Path.Combine("Graphics", "Atlases", "Cliffside"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "ForsakenCity", new AtlasInfo(Path.Combine("Graphics", "Atlases", "ForsakenCity"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "MirrorTemple", new AtlasInfo(Path.Combine("Graphics", "Atlases", "MirrorTemple"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "OldSite", new AtlasInfo(Path.Combine("Graphics", "Atlases", "OldSite"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "SummitEnd", new AtlasInfo(Path.Combine("Graphics", "Atlases", "SummitEnd"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "SummitIntro", new AtlasInfo(Path.Combine("Graphics", "Atlases", "SummitIntro"), Atlas.AtlasDataFormat.PackerNoAtlas)},
            { "TheFall", new AtlasInfo(Path.Combine("Graphics", "Atlases", "TheFall"), Atlas.AtlasDataFormat.PackerNoAtlas)},
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
                return;
            }
            else
            {
                if (!Directory.Exists(args[0]))
                {
                    Console.WriteLine("Content directory doesn't exist!");
                    WaitExit();
                    return;
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
            return;
        }
    }
}
