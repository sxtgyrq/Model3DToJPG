using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Maker
    {
        string obj;
        string mtl;
        string remark;
        Image image;


        byte[] objByte
        {
            get
            {
                return Encoding.UTF8.GetBytes(obj);
            }
        }
        byte[] mtlByte
        {
            get
            {
                return Encoding.UTF8.GetBytes(mtl);
            }
        }

        internal void Read()
        {
            string pngPath;
            {
                Console.WriteLine("input jpg file path");
                var input = Console.ReadLine();
                if (input != null)
                {
                    pngPath = input;
                    image = Image.FromFile(pngPath);
                    Detail(pngPath);
                }

            }

        }

        private void Detail(string pngPath)
        {
            var img = new Bitmap(pngPath);
            for (int i = 0; i < image.Width; i++)
            {
                //for (int j = 0; j < img.Height; j++)
                //{
                //    Color pixel = img.GetPixel(i, j);
                //}
            }
            var LengthOfObj = getIntFromColor(img.GetPixel(0, img.Width));
            var LengthOfMtl = getIntFromColor(img.GetPixel(1, img.Width));
            var LengthOfRemart = getIntFromColor(img.GetPixel(2, img.Width));
            // object p = img.GetPixel(1, image.Width);
            byte[] Data = new byte[img.Width * (img.Height - img.Width - 1) * 4];
            for (int i = 0; i < img.Height - img.Width - 1; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    var index = i * img.Width + j;
                    var x = j;
                    var y = i + img.Width + 1;

                    var c = img.GetPixel(x, y);
                    Data[index * 4 + 0] = c.A;
                    Data[index * 4 + 1] = c.R;
                    Data[index * 4 + 2] = c.G;
                    Data[index * 4 + 3] = c.B;
                }
            }

            {
                this.obj = Encoding.UTF8.GetString(Data, 0, LengthOfObj);
                this.mtl = Encoding.UTF8.GetString(Data, LengthOfObj, LengthOfMtl);
                this.remark = Encoding.UTF8.GetString(Data, LengthOfObj + LengthOfMtl, LengthOfRemart);


            }
            {
                Console.WriteLine($"{obj}");
                Console.WriteLine($"+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
                Console.WriteLine($"{mtl}");
                Console.WriteLine($"+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
                Console.WriteLine($"image.Width:{image.Width},image.Height:{image.Height}");
                Console.WriteLine($"+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
                Console.WriteLine($"{this.remark}");
                Console.WriteLine($"按E退出,按任意键继续");
                var inputKey = Console.ReadLine();
                if (inputKey == "E" || inputKey == "e")
                {
                    return;
                }
            }
        }

        private int getIntFromColor(Color color)
        {
            return color.A * 256 * 256 * 256 + color.R * 256 * 256 + color.G * 256 + color.B;
        }

        byte[] remarkByte
        {
            get
            {
                return Encoding.UTF8.GetBytes(remark);
            }
        }
        //int newImageLength()
        //{
        //    //int width = 4;
        //    //while (true)
        //    //{
        //    //    int dataWidth = width - 2;
        //    //    var data1 = this.image.Width * this.image.Height;
        //    //    var data2 = (objByte.Length + mtlByte.Length + remarkByte.Length + 99) / 3;
        //    //    if (dataWidth * dataWidth >= data1 + data2)
        //    //    {
        //    //        return width;
        //    //    }
        //    //}
        //}

        internal void Input()
        {
            {
                Console.WriteLine("input obj file path");
                var path = Console.ReadLine();
                obj = File.ReadAllText(path);
            }
            {
                Console.WriteLine("input mtl file path");
                var path = Console.ReadLine();
                mtl = File.ReadAllText(path);
            }
            {
                Console.WriteLine("input jpg file path");
                var path = Console.ReadLine();
                image = Image.FromFile(path);
            }
            {
                Console.WriteLine("input remarks");
                remark = Console.ReadLine();
            }

            Console.WriteLine($"{obj}");
            Console.WriteLine($"+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
            Console.WriteLine($"{mtl}");
            Console.WriteLine($"+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
            Console.WriteLine($"image.Width:{image.Width},image.Height:{image.Height}");
            Console.WriteLine($"按E退出,按任意键继续");
            var inputKey = Console.ReadLine();
            if (inputKey == "E" || inputKey == "e")
            {
                return;
            }
            Output();
        }

        internal void Output()
        {
            int sumLength = this.objByte.Length + this.mtlByte.Length + this.remarkByte.Length;
            while (sumLength % 4 != 0)
            {
                sumLength++;
            }
            while ((sumLength / 4) % this.image.Width != 0)
            {
                sumLength += 4;
            }
            int newHeight;
            newHeight =
                this.image.Height +//图片高度 
                1 + //备注行
                sumLength / 4 / this.image.Width;//content内容
                                                 //  while()
            Bitmap bm = new Bitmap(this.image.Width, newHeight);
            byte[] data = new byte[sumLength];
            this.objByte.CopyTo(data, 0);
            this.mtlByte.CopyTo(data, this.objByte.Length);
            this.remarkByte.CopyTo(data, this.objByte.Length + this.mtlByte.Length);
            using (Graphics g = Graphics.FromImage(bm))
            {
                g.Clear(Color.White);
                g.DrawImage(this.image, 0, 0);
            }
            {
                var material = objByte;
                var a = (material.Length / 16777216) % 256;
                var r = (material.Length / 65536) % 256;
                var g = (material.Length / (256)) % 256;
                var b = material.Length % 256;
                bm.SetPixel(0, this.image.Height, Color.FromArgb(a, r, g, b));
            }
            {
                var material = mtlByte;
                var a = (material.Length / 16777216) % 256;
                var r = (material.Length / 65536) % 256;
                var g = (material.Length / (256)) % 256;
                var b = material.Length % 256;
                bm.SetPixel(1, this.image.Height, Color.FromArgb(a, r, g, b));
            }
            {
                var material = remark;
                var a = (material.Length / 16777216) % 256;
                var r = (material.Length / 65536) % 256;
                var g = (material.Length / (256)) % 256;
                var b = material.Length % 256;
                bm.SetPixel(2, this.image.Height, Color.FromArgb(a, r, g, b));
            }
            var pixesLength = sumLength / 4;
            for (int i = 0; i < pixesLength; i++)
            {
                var a = data[i * 4 + 0];
                var r = data[i * 4 + 1];
                var g = data[i * 4 + 2];
                var b = data[i * 4 + 3];
                bm.SetPixel(
                    i % this.image.Width,
                    i / this.image.Width + this.image.Height + 1,
                    Color.FromArgb(a, r, g, b));
            }
            bm.Save("result.png", System.Drawing.Imaging.ImageFormat.Png);
        }
        /// <summary>
        /// 从维基百科上下载的。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        //int int_sqrt(int s)
        //{
        //    int x0 = s / 2;            // Initial estimate
        //                               // Avoid overflow when s is the maximum representable value
        //                               // Sanity check
        //    if (x0 != 0)
        //    {
        //        int x1 = (x0 + s / x0) / 2;    // Update

        //        while (x1 < x0)             // This also checks for cycle
        //        {
        //            x0 = x1;
        //            x1 = (x0 + s / x0) / 2;
        //        }
        //        return x0;
        //    }
        //    else
        //    {
        //        return s;
        //    }
        //}

    }
}
