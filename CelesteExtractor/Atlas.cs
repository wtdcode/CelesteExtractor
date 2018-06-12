using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMonocle
{
    using Celeste = CelesteExtractor.Celeste.Celeste;
    class Atlas
    {
        
        public List<VirtualTexture> Sources;

        public enum AtlasDataFormat
        {
            TexturePacker_Sparrow,
            CrunchXml,
            CrunchBinary,
            CrunchXmlOrBinary,
            CrunchBinaryNoAtlas,
            Packer,
            PackerNoAtlas
        }
        public static Atlas FromAtlas(string path, Atlas.AtlasDataFormat format)
        {
            Atlas atlas = new Atlas();
            atlas.Sources = new List<VirtualTexture>();
            Atlas.ReadAtlasData(atlas, path, format);
            return atlas;
        }
        private static void ReadAtlasData(Atlas atlas, string path, Atlas.AtlasDataFormat format)
        {
            switch (format)
            {
                case Atlas.AtlasDataFormat.TexturePacker_Sparrow:
                case Atlas.AtlasDataFormat.CrunchXml:
                case Atlas.AtlasDataFormat.CrunchBinary:
                case Atlas.AtlasDataFormat.CrunchXmlOrBinary:
                case Atlas.AtlasDataFormat.CrunchBinaryNoAtlas:
                    // Celeste never use these types.
                    throw new NotImplementedException();
                    break;
                case Atlas.AtlasDataFormat.Packer:
                    goto IL_521;
                case Atlas.AtlasDataFormat.PackerNoAtlas:
                    goto IL_67A;
                default:
                    throw new NotImplementedException();
                    IL_521:
                    using (FileStream fileStream3 = File.OpenRead(Path.Combine(Celeste.ContentDirectory, path + ".meta")))
                    {
                        BinaryReader binaryReader3 = new BinaryReader(fileStream3);
                        binaryReader3.ReadInt32();
                        binaryReader3.ReadString();
                        binaryReader3.ReadInt32();
                        short num9 = binaryReader3.ReadInt16();
                        for (int m = 0; m < (int)num9; m++)
                        {
                            string str3 = binaryReader3.ReadString();
                            VirtualTexture virtualTexture5 = new VirtualTexture(Path.Combine(Path.GetDirectoryName(path), str3 + ".data"));
                            atlas.Sources.Add(virtualTexture5);
                            short num10 = binaryReader3.ReadInt16();
                            for (int n = 0; n < (int)num10; n++)
                            {
                                string text5 = binaryReader3.ReadString().Replace('\\', '/');
                                short x2 = binaryReader3.ReadInt16();
                                short y2 = binaryReader3.ReadInt16();
                                short width3 = binaryReader3.ReadInt16();
                                short height3 = binaryReader3.ReadInt16();
                                short num11 = binaryReader3.ReadInt16();
                                short num12 = binaryReader3.ReadInt16();
                                short width4 = binaryReader3.ReadInt16();
                                short height4 = binaryReader3.ReadInt16();
                            }
                        }
                        return;
                    }
                    IL_67A:
                    using (FileStream fileStream4 = File.OpenRead(Path.Combine(Celeste.ContentDirectory, path + ".meta")))
                    {
                        BinaryReader binaryReader4 = new BinaryReader(fileStream4);
                        binaryReader4.ReadInt32();
                        binaryReader4.ReadString();
                        binaryReader4.ReadInt32();
                        short num13 = binaryReader4.ReadInt16();
                        for (int num14 = 0; num14 < (int)num13; num14++)
                        {
                            string path5 = binaryReader4.ReadString();
                            string path6 = Path.Combine(Path.GetDirectoryName(path), path5);
                            short num15 = binaryReader4.ReadInt16();
                            for (int num16 = 0; num16 < (int)num15; num16++)
                            {
                                string text6 = binaryReader4.ReadString().Replace('\\', '/');
                                binaryReader4.ReadInt16();
                                binaryReader4.ReadInt16();
                                binaryReader4.ReadInt16();
                                binaryReader4.ReadInt16();
                                short num17 = binaryReader4.ReadInt16();
                                short num18 = binaryReader4.ReadInt16();
                                short frameWidth2 = binaryReader4.ReadInt16();
                                short frameHeight2 = binaryReader4.ReadInt16();
                                VirtualTexture virtualTexture6 = new VirtualTexture(Path.Combine(path6, text6 + ".data"));
                                atlas.Sources.Add(virtualTexture6);
                            }
                        }
                        return;
                    }
            }
        }
    }
}
