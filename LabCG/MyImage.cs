using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NUnit.Framework;

namespace LabCG
{
    internal class MyImage
    {
        private Bitmap input;
        private int[,] arrayInputImage;
        public int width;
        public int height;
        public MyImage(String pathname)
        {
            input = new Bitmap(pathname);
            width = input.Width;
            height = input.Height;
            arrayInputImage = new int[width,height];
            for(int i = 0; i < width;i++)
            {
                for(int j=0; j < height;j++)
                {
                    Color pixel = input.GetPixel(i, j);
                    int color = pixel.R;
                    arrayInputImage[i, j] = color;
                }
            }
        }

        public int getWidth(int[,] image)
        {
            return image.GetLength(0);
        }

        public int getHeight(int[,] image)
        {
            return image.GetLength(1);
        }

        public int[,] NearestNeighborInterpolation(double sizeChange)
        {
            int oldWidth = width;
            int oldHeight = height;
            int newWidth = Convert.ToInt32(oldWidth * sizeChange);
            int newHeight = Convert.ToInt32(oldHeight * sizeChange);

            float coeffWidth = (float)(oldWidth - 1) / (float)(newWidth - 1);
            float coeffHeight = (float)(oldHeight - 1) / (float)(newHeight - 1);

            int[,] result = new int[newWidth, newHeight];
            int x0, y0;

            for (int y = 0; y < newHeight; y++)
            {
                y0 = Convert.ToInt32(y * coeffHeight);
                for (int x = 0; x < newWidth; x++)
                {
                    x0 = Convert.ToInt32(x * coeffWidth);
                    result[x, y] = arrayInputImage[x0, y0];
                }
            }
            return result;
        }

        public int[,] BilinearInterpolation(double sizeChange)
        {
            int oldWidth = width;
            int oldHeight = height;
            int newWidth = Convert.ToInt32(oldWidth * sizeChange);
            int newHeight = Convert.ToInt32(oldHeight * sizeChange);

            float coeffWidth = (float)(oldWidth - 1) / (float)(newWidth - 1);
            float coeffHeight = (float)(oldHeight - 1) / (float)(newHeight - 1);

            int[,] result = new int[newWidth, newHeight];
            int x1, y1, x2, y2;
            float x, y;
            for (int j = 0; j < newHeight; j++)
            {
                for (int i = 0; i < newWidth; i++)
                {
                    x1 = Convert.ToInt32(Math.Floor(i * coeffWidth));
                    y1 = Convert.ToInt32(Math.Floor(j * coeffHeight));
                    if (x1 > oldWidth - 2) 
                        x1 = oldWidth - 2;
                    if (y1 > oldHeight - 2) 
                        y1 = oldHeight - 2;
                    x2 = x1 + 1;
                    y2 = y1 + 1;
                    x = i * coeffWidth;
                    y = j * coeffHeight;
                    int Q11 = arrayInputImage[x1, y1];
                    int Q12 = arrayInputImage[x1, y2];
                    int Q21 = arrayInputImage[x2, y1];
                    int Q22 = arrayInputImage[x2, y2];
                    int R1, R2, P;
                    R1 = Convert.ToInt32((x2 - x) / (x2 - x1) * Q11 + (x - x1) / (x2 - x1) * Q21);
                    R2 = Convert.ToInt32((x2 - x) / (x2 - x1) * Q12 + (x - x1) / (x2 - x1) * Q22);
                    P = Convert.ToInt32((y2 - y) / (y2 - y1) * R1 + (y - y1) / (y2 - y1) * R2);
                    if (P < 0)
                        P = 0;
                    if (P > 255)
                        P = 255;
                    result[i, j] = P;
                }
            }
            return result;
        }

        public int[,] BicubicInterpolation(double sizeChange)
        {
            int oldWidth = width;
            int oldHeight = height;
            int newWidth = Convert.ToInt32(oldWidth * sizeChange);
            int newHeight = Convert.ToInt32(oldHeight * sizeChange);

            float coeffWidth = (float)(oldWidth - 1) / (float)(newWidth - 1);
            float coeffHeight = (float)(oldHeight - 1) / (float)(newHeight - 1);

            int x0, y0, x1, y1, x2, y2, x3, y3;
            float x, y;

            int[,] result = new int[newWidth, newHeight];
            for (int j = 0; j < newHeight; j++)
            {
                for (int i = 0; i < newWidth; i++)
                {
                    x1 = Convert.ToInt32(Math.Floor(i * coeffWidth));
                    y1 = Convert.ToInt32(Math.Floor(j * coeffHeight));

                    x0 = x1 - 1;
                    if (x0 < 0)
                        x0 = 0;
                    y0 = y1 - 1;
                    if (y0 < 0)
                        y0 = 0;
                    x2 = x1 + 1;
                    if (x2 > width - 1)
                        x2 = width - 1;
                    y2 = y1 + 1;
                    if (y2 > height - 1)
                    {
                        y2 = height - 1;
                    }
                    x3 = x1 + 2;
                    if (x3 > width - 1)
                        x3 = width - 1;
                    y3 = y1 + 2;
                    if (y3 > height - 1)
                    {
                        y3 = height - 1;
                    }
                    int Q00 = arrayInputImage[x0, y0];
                    int Q01 = arrayInputImage[x0, y1];
                    int Q02 = arrayInputImage[x0, y2];
                    int Q03 = arrayInputImage[x0, y3];
                    int Q10 = arrayInputImage[x1, y0];
                    int Q11 = arrayInputImage[x1, y1];
                    int Q12 = arrayInputImage[x1, y2];
                    int Q13 = arrayInputImage[x1, y3];
                    int Q20 = arrayInputImage[x2, y0];
                    int Q21 = arrayInputImage[x2, y1];
                    int Q22 = arrayInputImage[x2, y2];
                    int Q23 = arrayInputImage[x2, y3];
                    int Q30 = arrayInputImage[x3, y0];
                    int Q31 = arrayInputImage[x3, y1];
                    int Q32 = arrayInputImage[x3, y2];
                    int Q33 = arrayInputImage[x3, y3];
                    x = (float)(i * coeffWidth-Math.Floor(i * coeffWidth));
                    y = (float)(j * coeffHeight - Math.Floor(j * coeffHeight));
                    float b1 = 0.25f*(x - 1)*(x - 2)*(x + 1)*(y - 1)*(y - 2)*(y + 1);
                    float b2 = -0.25f * x * (x + 1)*(x - 2)*(y - 1)*(y - 2)*(y + 1);
                    float b3 = -0.25f * y * (x - 1) * (x - 2) * (x + 1) * (y + 1)*(y - 2);
                    float b4 = 0.25f * x * y * (x + 1) * (x - 2) * (y + 1) * (y - 2);
                    float b5 = -1f / 12 * x * (x - 1) * (x - 2) * (y - 1) * (y - 2) * (y + 1);
                    float b6 = -1f / 12 * y * (x - 1) * (x - 2) * (x + 1) * (y - 1) * (y - 2);
                    float b7 = 1f / 12 * x * y * (x - 1) * (x - 2) * (y + 1) * (y - 2);
                    float b8 = 1f / 12 * x * y * (x + 1) * (x - 2) * (y - 1) * (y - 2);
                    float b9 = 1f / 12 * x * (x - 1) * (x + 1) * (y - 1) * (y - 2) * (y + 1);
                    float b10 = 1f / 12 * y * (x - 1) * (x - 2) * (x + 1) * (y - 1) * (y + 1);
                    float b11 = 1f / 36 * x * y * (x - 1) * (x - 2) * (y - 1) * (y - 2);
                    float b12 = -1f / 12 * x * y * (x - 1) * (x + 1) * (y + 1) * (y - 2);
                    float b13 = -1f / 12 * x * y * (x + 1) * (x - 2) * (y - 1) * (y + 1);
                    float b14 = -1f / 36 * x * y * (x - 1) * (x + 1) * (y - 1) * (y - 2);
                    float b15 = -1f / 36 * x * y * (x - 1) * (x - 2) * (y - 1) * (y + 1);
                    float b16 = 1f / 36 * x * y * (x - 1) * (x + 1) * (y - 1) * (y + 1);
                    int P;
                      P = Convert.ToInt32(b1 * Q11 + b2 * Q21 + b3 * Q12 + b4 * Q22 + b5 * Q01 + b6 * Q10 + b7 * Q02 + b8 * Q20 + b9 * Q31 + b10 * Q13 + b11 * Q00 + b12 * Q32 + b13 * Q23 + b14 * Q30 + b15 * Q03 + b16 * Q33);
                    if (P < 0)
                        P = 0;
                    if (P > 255)
                        P = 255;
                    result[i, j] = P;
                }
            }
            return result;
        }

        public void newImageJpgNNI(double sizeChange, String pathname)
        {
            int[,] newImage = NearestNeighborInterpolation(sizeChange);
            int newWidth = getWidth(newImage);
            int newHeight = getHeight(newImage);
            Bitmap bitmap=new Bitmap(newWidth,newHeight);
            
            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    int color = newImage[i, j];
                    Color pixel = Color.FromArgb(color, color, color);
                    bitmap.SetPixel(i, j, pixel);
                }
            }
            bitmap.Save(pathname);
        }

        public void newImageJpgBil(double sizeChange, String pathname)
        {
            int[,] newImage = BilinearInterpolation(sizeChange);
            int newWidth = getWidth(newImage);
            int newHeight = getHeight(newImage);
            Bitmap bitmap = new Bitmap(newWidth, newHeight);

            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    int color = newImage[i, j];
                    Color pixel = Color.FromArgb(color, color, color);
                    bitmap.SetPixel(i, j, pixel);
                }
            }
            bitmap.Save(pathname);
        }

        public void newImageJpgBicubic(double sizeChange, String pathname)
        {
            int[,] newImage = BicubicInterpolation(sizeChange);
            int newWidth = getWidth(newImage);
            int newHeight = getHeight(newImage);
            Bitmap bitmap = new Bitmap(newWidth, newHeight);

            for (int i = 0; i < newWidth; i++)
            {
                for (int j = 0; j < newHeight; j++)
                {
                    int color = newImage[i, j];
                    Color pixel = Color.FromArgb(color, color, color);
                    bitmap.SetPixel(i, j, pixel);
                }
            }
            bitmap.Save(pathname);
        }

    }
}

