using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MyMonocle
{
    using Celeste = CelesteExtractor.Celeste.Celeste;

    public class VirtualTexture
    {
        /// <summary>
        /// 
        /// The length here is to keep the same with original assembly
        /// 
        /// </summary>
        internal static readonly byte[] buffer = new byte[67108864];

        internal static readonly byte[] bytes = new byte[524288];

        public string Path { get; private set; }

        public Texture2D Texture { get; private set; }

        internal VirtualTexture(string path)
        {
            this.Path = path;
            this.Reload();
        }

        public void SaveAsPng(string path)
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            using (FileStream fileStream = File.OpenWrite(path))
            {
                this.Texture.SaveAsPng(fileStream, this.Texture.Width, this.Texture.Height);
            }
        }

        internal unsafe void Reload()
        {
            /*
            * 
            * I remove some irrelevant codes in original assembly here.
            * 
            */
            string extension = System.IO.Path.GetExtension(this.Path);
            if (extension == ".data")
            {
                using (FileStream fileStream = File.OpenRead(System.IO.Path.Combine(Celeste.ContentDirectory, this.Path)))
                {
                    fileStream.Read(VirtualTexture.bytes, 0, 524288);
                    int num = 0;
                    int num2 = BitConverter.ToInt32(VirtualTexture.bytes, num);
                    int num3 = BitConverter.ToInt32(VirtualTexture.bytes, num + 4);
                    bool flag = VirtualTexture.bytes[num + 8] == 1;
                    num += 9;
                    int num4 = num2 * num3 * 4;
                    int j = 0;
                    try
                    {
                        byte[] array3 = VirtualTexture.bytes;
                        fixed (byte* ptr2 = &array3[0])
                        {
                            byte[] array4 = VirtualTexture.buffer;
                            fixed (byte* ptr3 = &array4[0])
                            {
                                while (j < num4)
                                {
                                    int num5 = (int)(ptr2[num] * 4);
                                    if (flag)
                                    {
                                        byte b = ptr2[num + 1];
                                        if (b > 0)
                                        {
                                            ptr3[j] = ptr2[num + 4];
                                            ptr3[j + 1] = ptr2[num + 3];
                                            ptr3[j + 2] = ptr2[num + 2];
                                            ptr3[j + 3] = b;
                                            num += 5;
                                        }
                                        else
                                        {
                                            ptr3[j] = 0;
                                            ptr3[j + 1] = 0;
                                            ptr3[j + 2] = 0;
                                            ptr3[j + 3] = 0;
                                            num += 2;
                                        }
                                    }
                                    else
                                    {
                                        ptr3[j] = ptr2[num + 3];
                                        ptr3[j + 1] = ptr2[num + 2];
                                        ptr3[j + 2] = ptr2[num + 1];
                                        ptr3[j + 3] = byte.MaxValue;
                                        num += 4;
                                    }
                                    if (num5 > 4)
                                    {
                                        int k = j + 4;
                                        int num6 = j + num5;
                                        while (k < num6)
                                        {
                                            ptr3[k] = ptr3[j];
                                            ptr3[k + 1] = ptr3[j + 1];
                                            ptr3[k + 2] = ptr3[j + 2];
                                            ptr3[k + 3] = ptr3[j + 3];
                                            k += 4;
                                        }
                                    }
                                    j += num5;
                                    if (num > 524256)
                                    {
                                        int num7 = 524288 - num;
                                        for (int l = 0; l < num7; l++)
                                        {
                                            ptr2[l] = ptr2[num + l];
                                        }
                                        fileStream.Read(VirtualTexture.bytes, num7, 524288 - num7);
                                        num = 0;
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        byte[] array3 = null;
                        byte[] array4 = null;
                    }
                    this.Texture = new Texture2D(Celeste.graphicsDevice, num2, num3);
                    this.Texture.SetData<byte>(VirtualTexture.buffer, 0, num4);
                }
            }
            else if (extension == ".png")
            {
                using (FileStream fileStream2 = File.OpenRead(System.IO.Path.Combine(Celeste.ContentDirectory, this.Path)))
                {
                    this.Texture = Texture2D.FromStream(Celeste.graphicsDevice, fileStream2);
                }
                int num8 = this.Texture.Width * this.Texture.Height;
                Color[] array5 = new Color[num8];
                this.Texture.GetData<Color>(array5, 0, num8);
                Color[] array2 = array5;
                fixed (Color* ptr4 = &array2[0])
                {
                    for (int m = 0; m < num8; m++)
                    {
                        ptr4[m].R = (byte)((float)ptr4[m].R * ((float)ptr4[m].A / 255f));
                        ptr4[m].G = (byte)((float)ptr4[m].G * ((float)ptr4[m].A / 255f));
                        ptr4[m].B = (byte)((float)ptr4[m].B * ((float)ptr4[m].A / 255f));
                    }
                    array2 = null;
                    this.Texture.SetData<Color>(array5, 0, num8);
                }
            }
            else if (extension == ".xnb")
            {
                string assetName = this.Path.Replace(".xnb", "");
                this.Texture = Celeste.Content.Load<Texture2D>(assetName);
            }
            else
            {
                using (FileStream fileStream3 = File.OpenRead(System.IO.Path.Combine(Celeste.ContentDirectory, this.Path)))
                {
                    this.Texture = Texture2D.FromStream(Celeste.graphicsDevice, fileStream3);
                }
            }
        }
            
    }
}
