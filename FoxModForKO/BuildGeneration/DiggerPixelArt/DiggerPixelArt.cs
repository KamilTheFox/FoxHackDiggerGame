using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dithering;

namespace DiggerPixelArt
{
    class ColorConvert
    {
        private static void SimilarColorRGB(int R, int G, int B, out int RR, out int GR, out int BR, out int ID)
        {
            int[,] blocksRGB = { { 1, 67, 53, 25 }, { 4, 28, 71, 13 }, { 5, 87, 85, 65 }, { 6, 76, 55, 27 }, { 7, 84, 58, 26 }, { 8, 85, 82, 79 }, { 10, 139, 95, 38 }, { 11, 75, 75, 63 }, { 12, 133, 93, 42 }, { 13, 135, 87, 34 }, { 14, 117, 121, 118 }, { 15, 78, 73, 66 }, { 17, 97, 94, 88 }, { 19, 30, 29, 29 }, { 20, 200, 200, 200 }, { 21, 182, 69, 69 }, { 22, 192, 123, 75 }, { 23, 192, 164, 75 }, { 24, 122, 158, 84 }, { 25, 70, 158, 105 }, { 26, 79, 129, 167 }, { 27, 157, 102, 164 }, { 28, 94, 68, 47 }, { 29, 117, 102, 85 }, { 30, 134, 109, 74 }, { 31, 79, 78, 77 }, { 33, 65, 73, 54 }, { 34, 66, 74, 60 }, { 35, 96, 97, 96 }, { 36, 110, 95, 70 }, { 37, 91, 79, 99 }, { 38, 92, 103, 75 }, { 40, 102, 80, 73 }, { 43, 84, 92, 108 }, { 44, 93, 66, 64 }, { 45, 99, 84, 57 }, { 47, 95, 95, 95 }, { 49, 115, 98, 118 }, { 50, 90, 112, 87 }, { 51, 138, 85, 62 }, { 52, 156, 123, 62 }, { 53, 72, 72, 71 }, { 60, 166, 128, 65 }, { 62, 102, 75, 40 }, { 63, 167, 118, 66 }, { 64, 153, 119, 83 }, { 65, 89, 57, 29 }, { 66, 48, 33, 18 }, { 84, 78, 75, 46 }, { 85, 46, 86, 131 }, { 86, 131, 46, 38 }, { 87, 78, 64, 51 }, { 88, 60, 66, 58 }, { 89, 70, 49, 29 }, { 90, 97, 48, 29 }, { 91, 59, 53, 42 }, { 92, 75, 79, 78 }, { 93, 76, 79, 79 }, { 94, 64, 66, 65 }, { 95, 57, 58, 53 }, { 96, 77, 79, 78 }, { 97, 62, 50, 31 }, { 99, 70, 71, 72 }, { 100, 77, 79, 80 }, { 101, 89, 89, 75 }, { 102, 58, 65, 39 }, { 103, 87, 90, 74 }, { 104, 75, 79, 61 }, { 105, 50, 53, 39 }, { 106, 67, 73, 49 }, { 107, 71, 75, 54 }, { 108, 63, 57, 27 }, { 109, 58, 53, 24 }, { 111, 106, 108, 90 }, { 112, 67, 74, 46 }, { 113, 76, 77, 59 }, { 114, 63, 71, 53 }, { 115, 66, 74, 56 }, { 116, 64, 74, 58 }, { 117, 66, 76, 60 }, { 118, 68, 74, 52 }, { 119, 76, 82, 57 }, { 120, 66, 74, 57 }, { 122, 55, 50, 21 }, { 123, 55, 51, 21 }, { 124, 64, 55, 19 }, { 125, 73, 55, 20 }, { 126, 72, 54, 20 }, { 127, 61, 44, 17 }, { 128, 77, 56, 20 }, { 129, 76, 50, 20 }, { 130, 68, 53, 22 }, { 131, 74, 51, 25 }, { 132, 143, 132, 116 }, { 133, 67, 48, 19 }, { 134, 74, 53, 22 }, { 135, 72, 40, 19 }, { 136, 77, 60, 45 }, { 137, 73, 44, 22 }, { 138, 70, 39, 19 }, { 139, 79, 56, 27 }, { 140, 83, 54, 25 }, { 141, 82, 53, 24 }, { 142, 104, 78, 52 }, { 143, 59, 53, 39 }, { 144, 66, 71, 45 }, { 145, 64, 71, 42 }, { 146, 112, 108, 98 }, { 147, 69, 71, 61 }, { 148, 81, 83, 74 }, { 149, 89, 87, 73 }, { 150, 77, 72, 15 }, { 151, 55, 55, 45 }, { 152, 52, 53, 47 }, { 153, 69, 70, 66 }, { 155, 91, 96, 51 }, { 158, 68, 56, 41 }, { 159, 77, 60, 43 }, { 160, 82, 64, 42 }, { 161, 76, 59, 42 }, { 162, 65, 62, 57 }, { 163, 74, 62, 36 }, { 164, 66, 66, 66 }, { 165, 64, 63, 63 }, { 166, 57, 56, 56 }, { 167, 79, 45, 25 }, { 168, 33, 29, 36 }, { 169, 32, 29, 32 }, { 170, 36, 32, 38 }, { 171, 33, 30, 33 }, { 172, 38, 30, 35 }, { 173, 35, 29, 32 }, { 174, 33, 30, 34 }, { 175, 34, 32, 31 }, { 176, 53, 30, 21 }, { 177, 37, 34, 33 }, { 180, 48, 30, 28 }, { 181, 48, 44, 51 }, { 182, 40, 37, 44 }, { 183, 39, 29, 27 }, { 184, 51, 32, 26 }, { 185, 26, 24, 26 }, { 186, 27, 25, 28 }, { 187, 26, 23, 26 }, { 190, 182, 207, 195 }, { 191, 169, 202, 186 }, { 192, 106, 92, 62 }, { 193, 85, 76, 52 }, { 194, 87, 77, 52 }, { 195, 213, 213, 210 }, { 196, 191, 192, 182 }, { 197, 81, 76, 64 }, { 198, 56, 54, 45 }, { 199, 191, 186, 165 }, { 200, 72, 43, 28 }, { 201, 187, 181, 158 }, { 202, 186, 180, 156 }, { 203, 119, 92, 63 }, { 204, 53, 65, 82 }, { 205, 83, 49, 46 }, { 206, 61, 65, 44 }, { 207, 136, 190, 204 }, { 208, 160, 211, 228 }, { 209, 205, 214, 221 }, { 210, 44, 73, 96 }, { 211, 74, 74, 69 }, { 212, 57, 57, 54 }, { 213, 97, 98, 89 }, { 214, 80, 80, 76 }, { 215, 91, 91, 81 }, { 216, 81, 62, 49 }, { 217, 77, 61, 49 }, { 218, 93, 60, 46 }, { 219, 125, 111, 91 }, { 220, 122, 108, 88 }, { 221, 77, 79, 67 }, { 222, 77, 79, 67 }, { 223, 116, 90, 73 }, { 224, 78, 81, 68 }, { 225, 128, 99, 79 }, { 226, 104, 81, 66 }, { 227, 83, 79, 75 }, { 228, 139, 135, 120 }, { 230, 39, 37, 34 }, { 235, 79, 81, 69 }, { 239, 181, 178, 162 }, { 240, 182, 180, 163 }, { 241, 113, 198, 224 }, { 242, 99, 99, 99 }, { 243, 63, 63, 63 }, { 244, 125, 83, 52 }, { 245, 191, 154, 124 }, { 246, 104, 93, 37 } };

            int R1 = R;
            int G1 = G;
            int B1 = B;

            int R2 = 0;
            int G2 = 0;
            int B2 = 0;

            double distance = 0;
            int nn = 0;

            for (int n = 0; n < blocksRGB.GetLength(0); n++)
            {
                int id = blocksRGB[n, 0];
                R2 = blocksRGB[n, 1];
                G2 = blocksRGB[n, 2];
                B2 = blocksRGB[n, 3];

                double dr = Math.Abs(R1 - R2);
                double dg = Math.Abs(G1 - G2);
                double db = Math.Abs(B1 - B2);

                double d = Math.Sqrt(dr * dr + dg * dg + db * db);

                if (n == 0)
                {
                    distance = d;
                }

                if (distance > d)
                {
                    distance = d;
                    nn = n;
                }
            }

            ID = Convert.ToInt32(blocksRGB[nn, 0]);
            RR = blocksRGB[nn, 1];
            GR = blocksRGB[nn, 2];
            BR = blocksRGB[nn, 3];

        }
        public static Tuple<Bitmap,string> ConvertRGB(Bitmap bmp, int widthNew, int heightNew)
        {
            bmp = Resize(bmp, widthNew, heightNew, true);
            Bitmap bmpNew = new Bitmap(bmp);
            StringBuilder blocksBuilder = new StringBuilder("[");
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color color = bmp.GetPixel(x, y);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    int RR, GR, BR;
                    int ID;
                    SimilarColorRGB(R, G, B, out RR, out GR, out BR, out ID);
                    bmpNew.SetPixel(x, y, Color.FromArgb(RR, GR, BR));
                    blocksBuilder.Append($"{{\"x\":{bmp.Width - x},\"y\":{bmp.Height - y}\"z\":{0},\"blockType\":{ID},\"blockKind\":{0}}}");
                    if (x != bmp.Width - 1 || y != bmp.Height - 1)
                    {
                        blocksBuilder.Append(",");
                    }
                }
            }
            blocksBuilder.Append("]");
            return new Tuple<Bitmap, string>(bmpNew, blocksBuilder.ToString());
        }
        public static Bitmap Resize(Bitmap bitmap, int Width, int Height, bool isBuild = false)
        {
            try
            {
                Bitmap b = new Bitmap(Width, Height);
                using (Graphics g = Graphics.FromImage(b))
                {
                    if (isBuild)
                    { 
                        g.InterpolationMode = (InterpolationMode)PixelArt.settingsImage[0];
                        g.CompositingQuality = (CompositingQuality)PixelArt.settingsImage[1];
                        g.SmoothingMode = (SmoothingMode)PixelArt.settingsImage[2];
                        g.PixelOffsetMode = (PixelOffsetMode)PixelArt.settingsImage[3];
                    }
                    else
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bitmap, 0, 0, Width, Height);
                }
                if(isBuild)
                    b = Transformed.Start(b, (TypeDithering)PixelArt.settingsImage[4]);
                return b;
            }
            catch
            {
                UnityEngine.Debug.LogWarning("Bitmap could not be resized");
                return bitmap;
            }
        }
    }
}
