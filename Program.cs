using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = new DirectoryInfo(@"C:\Users\stu\Desktop\EDD\Jungheinrich");
            var files = dir.EnumerateFiles("*.jpg").Select(_ => _.FullName).ToArray();
            var bmp = CombineBitmap(files, 1);
            var desc = Path.Combine(dir.FullName, dir.Name + ".png");            
            bmp.Save(desc, ImageFormat.Png);
        }

        public static Bitmap CombineBitmap(string[] files, int cc)
        {
            //read all images into memory
            List<Bitmap> images = new List<Bitmap>();
            Bitmap finalImage = null;

            try
            {   
                var cnt = files.Count();

                foreach (string image in files)
                {
                    //create a Bitmap from the file and add it to the list
                    Bitmap bitmap = new Bitmap(image);                                        
                    images.Add(bitmap);
                }

                int ih = 735;
                int iw = 591;
                int height = ih * (int)Math.Ceiling((double)cnt / (double)cc);
                int width = iw * cc;

                //create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.White);

                    //go through each image and draw it on the final image                    
                    int col = 0;
                    int row = 0;

                    foreach (Bitmap image in images)
                    {                        
                        g.DrawImage(image, new Rectangle(col * iw, row * ih, iw, ih));

                        col += 1;
                        if (col == cc)
                        {
                            col = 0;
                            row += 1;
                        }                        
                    }
                }

                return finalImage;
            }
            catch (Exception)
            {
                if (finalImage != null)
                    finalImage.Dispose();                
                throw;
            }
            finally
            {
                //clean up memory
                foreach (Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }
    }
}