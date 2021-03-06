﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;

namespace ImageOperations
{
    public static class Operations
    {
        /// <summary>
        /// Resizes image using provided parameters.
        /// </summary>
        public static Bitmap Resize(this Image img, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, result.Width, result.Height);
                return result;
            }
        }

        /// <summary>
        /// Rotates image using provided angle.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Bitmap Rotate(this Image img, int angle)
        {
            Bitmap result = new Bitmap(img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.TranslateTransform(img.Width / 2, img.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-img.Width / 2, -img.Height / 2);
                g.DrawImage(img, 0, 0, img.Width, img.Height);
                return result;
            }
        }

        /// <summary>
        /// Converts all pixels of Bitmap to grayscale.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>

        public static Bitmap ToGrayscale(this Bitmap bitmap)
        {
            Bitmap result = new Bitmap(bitmap.Width, bitmap.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                // Grayscale matrix
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                   0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);
            }

            return result;
        }

        public static Bitmap ChangeAttributes(this Bitmap bitmap, int brightness, int contrast)
        {
            BrightnessCorrection bfilter = new BrightnessCorrection(brightness);
            ContrastCorrection cfilter = new ContrastCorrection(contrast);
            
            return bfilter.Apply(cfilter.Apply(bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format24bppRgb)));
        }
    }
}
