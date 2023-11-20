using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyotek.Drawing;
using Cyotek.Drawing.Imaging.ColorReduction;
using Cyotek.DitheringTest.Transforms;
using Cyotek.DitheringTest.Helpers;
using System.Drawing;

namespace Dithering
{
    public enum TypeDithering
    {
        None,
        AtkinsonDithering,
        BurksDithering,
        OrderedDithering,
        SierraLiteDithering,
        JarvisJudiceNinkeDithering
    }
    
    public class Transformed
    {
        public static Bitmap Start(Bitmap bitmap, TypeDithering typeDithering)
        {
            WorkerData workerData;
            IPixelTransform transform;
            IErrorDiffusion ditherer = null;
            Bitmap image;
            if (typeDithering == TypeDithering.None)
                return bitmap;
            transform = GetPixelTransform(2);
            switch(typeDithering)
            {
                case TypeDithering.AtkinsonDithering:
                ditherer = new AtkinsonDithering();
                break;
                case TypeDithering.BurksDithering:
                    ditherer = new BurksDithering();
                    break;
                case TypeDithering.JarvisJudiceNinkeDithering:
                    ditherer = new JarvisJudiceNinkeDithering();
                    break;
                case TypeDithering.SierraLiteDithering:
                    ditherer = new SierraLiteDithering();
                    break;
            }
            

            image = bitmap.Copy();
            workerData = new WorkerData
            {
                Image = image,
                Transform = transform,
                Dither = ditherer,
                ColorCount = 1
            };

            return GetTransformedImage(workerData);

        }
        private static IPixelTransform GetPixelTransform(int id)
        {
            IPixelTransform result;

            result = null;
            switch (id)
            {
                case 0:
                    result = new SimpleIndexedPalettePixelTransform8();
                    break;
                case 1:
                    result = new SimpleIndexedPalettePixelTransform16();
                    break;
                case 2:
                    result = new SimpleIndexedPalettePixelTransform256();
                    break;
            }
            return result;
        }
        private static Bitmap GetTransformedImage(WorkerData workerData)
        {
            Bitmap image;
            Bitmap result;
            ArgbColor[] pixelData;
            Size size;
            IPixelTransform transform;
            IErrorDiffusion dither;

            transform = workerData.Transform;
            dither = workerData.Dither;
            image = workerData.Image;
            size = image.Size;
            pixelData = image.GetPixelsFrom32BitArgbImage();

            if (dither != null && dither.Prescan)
            {
                // perform the dithering on the source data before
                // it is transformed
                ProcessPixels(pixelData, size, null, dither);
                dither = null;
            }

            // scan each pixel, apply a transform the pixel
            // and then dither it
            ProcessPixels(pixelData, size, transform, dither);

            // create the final bitmap
            result = pixelData.ToBitmap(size);

            return result;
        }
        internal sealed class WorkerData
        {
            public Bitmap Image { get; set; }

            public IErrorDiffusion Dither { get; set; }

            public IPixelTransform Transform { get; set; }

            public int ColorCount { get; set; }
        }
        private static void ProcessPixels(ArgbColor[] pixelData, Size size, IPixelTransform pixelTransform, IErrorDiffusion dither)
        {
            for (int row = 0; row < size.Height; row++)
            {
                for (int col = 0; col < size.Width; col++)
                {
                    int index;
                    ArgbColor current;
                    ArgbColor transformed;

                    index = row * size.Width + col;

                    current = pixelData[index];

                    // transform the pixel
                    if (pixelTransform != null)
                    {
                        transformed = pixelTransform.Transform(pixelData, current, col, row, size.Width, size.Height);
                        pixelData[index] = transformed;
                    }
                    else
                    {
                        transformed = current;
                    }

                    // apply a dither algorithm to this pixel
                    // assuming it wasn't done before
                    dither?.Diffuse(pixelData, current, transformed, col, row, size.Width, size.Height);
                }
            }
        }
    }
}
