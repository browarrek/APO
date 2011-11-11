﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace APO
{
    public partial class Picture : Form
    {
        private int[] histogram;
        public Bitmap bitmap;

        public Picture()
        {
            InitializeComponent();
        }

        public Picture(Picture picture)
        {
            InitializeComponent();

            bitmap = new Bitmap(picture.bitmap);
            pictureBox1.Image = bitmap;

            int height = bitmap.Size.Height;
            int width = bitmap.Size.Width;
            pictureBox1.Width = width * 420 / height;
        }

        public void loadImage(String path)
        {
            bitmap = CreateNonIndexedImage(new Bitmap(path));

            pictureBox1.Image = bitmap;

            int height = bitmap.Size.Height;
            int width = bitmap.Size.Width;
            pictureBox1.Width = width * 420 / height;
        }

        public Bitmap CreateNonIndexedImage(Image src)
        {
            Bitmap newBMP = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(newBMP);
            gfx.DrawImage(src, 0, 0);
            return newBMP;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private int[] GetHistogram(Bitmap picture)
        {
            int[] myHistogram = new int[256];

            for (int i = 0; i < picture.Size.Width; i++)
                for (int j = 0; j < picture.Size.Height; j++)
                {
                    System.Drawing.Color c = picture.GetPixel(i, j);

                    int Temp = 0;
                    Temp += c.R;
                    Temp += c.G;
                    Temp += c.B;

                    Temp = (int)Temp / 3;
                    myHistogram[Temp]++;
                }

            return myHistogram;
        }

        private void drawHistogram()
        {
            histogram = GetHistogram(bitmap);
            Graphics graphicsObj = panel3.CreateGraphics();
            Pen myPen = new Pen(System.Drawing.Color.Black, 1);

            long max = histogram.Max();
            graphicsObj.Clear(panel3.BackColor);
            for (int i = 0; i < 256; i++)
            {
                graphicsObj.DrawLine(myPen, i, 150, i, 150 - histogram[i] * 150 / max); 
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            drawHistogram();
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.Text = e.X.ToString();
            label4.Text = histogram[e.X].ToString();
        }

        public void MakeGrayscale3()
        {
            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(bitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
                  {
                     new float[] {.3f, .3f, .3f, 0, 0},
                     new float[] {.59f, .59f, .59f, 0, 0},
                     new float[] {.11f, .11f, .11f, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {0, 0, 0, 0, 1}
                  });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
               0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void Metoda1()
        {
            int R=0, Hint=0, Havg=0;
            int[] H = new int[256];
            int[] New = new int[256];
            int[] left = new int[256];
            int[] right = new int[256];

            //Zerowanie

            for (int i = 0; i < 256; i++)
            {
                H[i] = New[i] = left[i] = right[i] = 0;
            }

            //Srednia

            for (int Z = 0; Z < 256; Z++)
            {
                Havg = Havg + histogram[Z];
            }

            Havg = Havg / 256;

            for (int Z = 0; Z < 256; Z++)
            {
                left[Z] = R;
                Hint = Hint + histogram[Z];

                while (Hint > Havg)
                {
                    Hint = Hint - Havg;
                    R++;
                }

                right[Z] = R;

                New[Z] = ((left[Z] + right[Z]) / 2);
            }

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);

                    if (left[c.R] == right[c.R])
                    {
                        if (left[c.R] <= 255)
                        {
                            bitmap.SetPixel(j, i, Color.FromArgb(left[c.R], left[c.R], left[c.R]));
                        }
                        else
                        {
                            bitmap.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        }

                    }
                    else
                    {
                        if (New[c.R] <= 255)
                        {
                            bitmap.SetPixel(j, i, Color.FromArgb(New[c.R], New[c.R], New[c.R]));
                        }
                        else
                        {
                            bitmap.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        }
                    }
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void Metoda2()
        {
            int R = 0, Hint = 0, Havg = 0;
            int[] H = new int[256];
            int[] New = new int[256];
            int[] left = new int[256];
            int[] right = new int[256];
            Random rand = new Random();

            //Zerowanie

            for (int i = 0; i < 256; i++)
            {
                H[i] = New[i] = left[i] = right[i] = 0;
            }

            for (int Z = 0; Z < 256; Z++)
            {
                Havg = Havg + histogram[Z];
            }

            Havg = Havg / 256;

            for (int Z = 0; Z < 256; Z++)
            {
                left[Z] = R;
                Hint = Hint + histogram[Z];

                while (Hint > Havg)
                {
                    Hint = Hint - Havg;
                    R++;
                }

                right[Z] = R;

                New[Z] = (right[Z] - left[Z]);
            }

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);

                    if (left[c.R] == right[c.R])
                    {
                        if (left[c.R] <= 255)
                        {
                            bitmap.SetPixel(j, i, Color.FromArgb(left[c.R], left[c.R], left[c.R]));
                        }
                        else
                        {
                            bitmap.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        }

                    }
                    else
                    {
                        int x = rand.Next(0, New[c.R]);

                        if (left[c.R] + x <= 255)
                        {
                            bitmap.SetPixel(j, i, Color.FromArgb(left[c.R] + x, left[c.R] + x, left[c.R] + x));
                        }
                        else
                        {
                            bitmap.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        }
                    }
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void Metoda3()
        {
            int R = 0, Hint = 0, Havg = 0;
            int[] H = new int[256];
            int[] New = new int[256];
            int[] left = new int[256];
            int[] right = new int[256];
            Random rand = new Random();

            for (int Z = 0; Z < 256; Z++)
            {
                Havg = Havg + histogram[Z];
            }

            Havg = Havg / 256;

            for (int Z = 0; Z < 256; Z++)
            {
                left[Z] = R;
                Hint = Hint + histogram[Z];

                while (Hint > Havg)
                {
                    Hint = Hint - Havg;
                    R++;
                }

                right[Z] = R;
            }


            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);

                    if (left[c.R] == right[c.R])
                    {
                        if (left[c.R] <= 255)
                        {
                            bitmap.SetPixel(j, i, Color.FromArgb(left[c.R], left[c.R], left[c.R]));
                        }
                        else
                        {
                            bitmap.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                        }

                    }
                    else
                    {
                        Color c1 = new Color();
                        int aj = j, ai = i, srednia = 0;

                        if (aj > 0 && aj + 1 < bitmap.Width && ai > 0 && ai + 1 < bitmap.Height)
                        {
                            c1 = bitmap.GetPixel(aj + 1, ai + 1);
                            srednia = srednia + c1.R;

                            c1 = bitmap.GetPixel(aj - 1, ai - 1);
                            srednia = srednia + c1.R;

                            c1 = bitmap.GetPixel(aj - 1, ai);
                            srednia = srednia + c1.R;

                            c1 = bitmap.GetPixel(aj + 1, ai);
                            srednia = srednia + c1.R;

                            c1 = bitmap.GetPixel(aj, ai - 1);
                            srednia = srednia + c1.R;

                            c1 = bitmap.GetPixel(aj, ai + 1);
                            srednia = srednia + c1.R;

                            c1 = bitmap.GetPixel(aj + 1, ai - 1);
                            srednia = srednia + c1.R;

                            c1 = bitmap.GetPixel(aj - 1, ai + 1);
                            srednia = srednia + c1.R;

                            srednia = srednia / 8;

                            if (srednia > right[c.R])
                            {
                                bitmap.SetPixel(j, i, Color.FromArgb(right[c.R], right[c.R], right[c.R]));
                            }
                            else
                            {
                                if (srednia < left[c.R])
                                {
                                    if (left[c.R] <= 255)
                                    {
                                        bitmap.SetPixel(j, i, Color.FromArgb(left[c.R], left[c.R], left[c.R]));
                                    }
                                    else
                                    {
                                        bitmap.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                                    }
                                }
                                else
                                {
                                    bitmap.SetPixel(j, i, Color.FromArgb(srednia, srednia, srednia));
                                }
                            }
                        }
                    }
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void negacja()
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);

                    bitmap.SetPixel(j, i, Color.FromArgb(255 - c.R, 255 - c.R, 255 - c.R));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void progowanie(int prog)
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);

                    int color = (c.R < prog ? 0 : 255);

                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void redukcjaPoziomowSzarosci(int poziomy)
        {
            double prog = 256 / (poziomy-1);

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);

                    int color = 255;

                    for (int k = 0; k < poziomy-1; k++)
                    {
                        if (c.R < prog * (k + 0.5))
                        {
                            color = (int)prog * k;
                            break;
                        }
                    }

                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void rozciaganie(int start, int koniec)
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);
                    int color = 0;

                    if (c.R >= start && c.R <= koniec)
                    {
                        color = 255 - (255 * (koniec - c.R) / (koniec - start));                        
                    }
                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void jasnosc(int procent)
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);

                    int color = c.R + (255 * procent / 100);
                    if (color > 255) color = 255;
                    else if (color < 0) color = 0;

                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void kontrast(int procent)
        {
            if (procent == 0) return;
            double kontrast = Math.Pow(((100.0 + (double)procent) / 100.0), 2.0);
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);
                    double col = c.R / 255.0;
                    col -= 0.5;
                    col *= kontrast;
                    col += 0.5;
                    col *= 255.0;

                    if (col > 255) col = 255.0;
                    else if (col < 0) col = 0.0;

                    int color = (int)col;

                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void gamma(double gamma)
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);

                    double col = 255.0 * Math.Pow((c.R / 255.0), (1.0 / gamma));
                    int color = (int)col;

                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void add(Bitmap bitmap2)
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);
                    Color d = bitmap2.GetPixel(j, i);

                    int color = (c.R + d.R) / 2;

                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void sub(Bitmap bitmap2)
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);
                    Color d = bitmap2.GetPixel(j, i);

                    int color = Math.Abs(c.R - d.R);

                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void and(Bitmap bitmap2)
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);
                    Color d = bitmap2.GetPixel(j, i);

                    int color = c.R & d.R;

                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void or(Bitmap bitmap2)
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);
                    Color d = bitmap2.GetPixel(j, i);

                    int color = c.R | d.R;

                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void xor(Bitmap bitmap2)
        {
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    Color c = bitmap.GetPixel(j, i);
                    Color d = bitmap2.GetPixel(j, i);

                    int color = c.R ^ d.R;

                    bitmap.SetPixel(j, i, Color.FromArgb(color, color, color));
                }
            }
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void ApplyMask(int[,] mask, int divisor)
        {
            FastBitmap bmp = new FastBitmap(bitmap);
            FastBitmap bmp2 = new FastBitmap((Bitmap)bmp.Bitmap.Clone());

            if (divisor == 0)
                divisor = 1;

            int size = mask.GetLength(0) / 2;
            Point[,] temp = new Point[mask.GetLength(0), mask.GetLength(0)];

            for (int i = -size; i <= size; ++i)
                for (int j = -size; j <= size; ++j)
                    temp[i + size, j + size] = new Point(i, j);

            for (int i = size; i < bmp.Width - size; ++i)
            {
                for (int j = size; j < bmp.Height - size; ++j)
                {
                    int newColor = 0;
                    for (int k = 0; k < mask.GetLength(0); ++k)
                    {
                        for (int l = 0; l < mask.GetLength(0); ++l)
                        {
                            Color color = bmp[i + temp[k, l].X, j + temp[k, l].Y];
                            newColor += mask[k, l] * color.R;
                        }
                    }
                    newColor /= divisor;

                    newColor = Math.Max(0, Math.Min(newColor, 255));
                    bmp2[i, j] = Color.FromArgb(255, newColor, newColor, newColor);
                }
            }

            bmp2.Unlock();
            bitmap = bmp2.Bitmap;
            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();
            drawHistogram();
        }

        public void FiltracjaMedianowa(int value)
        {
            FastBitmap bmp = new FastBitmap(bitmap);

            int filterSize = value;
            for (int i = 0; i < bmp.Size.Width; ++i)
            {
                for (int j = 0; j < bmp.Size.Height; ++j)
                {
                    byte[] neighbours = new byte[filterSize * filterSize];
                    int a = 0;
                    for (int k = -filterSize / 2; k <= filterSize / 2; ++k)
                    {
                        for (int l = -filterSize / 2; l <= filterSize / 2; ++l)
                        {
                            neighbours[a++] = bmp[i + k, j + l].R;
                        }
                    }

                    Color color = bmp[i, j];
                    byte newColor;
                    Array.Sort(neighbours);
                    if (neighbours.Length % 2 == 1)
                        newColor = neighbours[neighbours.Length / 2];
                    else
                        newColor = (byte)((neighbours[neighbours.Length / 2] + neighbours[(neighbours.Length / 2) + 1]) / 2);
                    bmp[i, j] = Color.FromArgb(color.A, newColor, newColor, newColor);
                }
            }
            bmp.Unlock();
            bitmap = bmp.Bitmap;
            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();
            drawHistogram();

        }
        public void Szkieletyzacja()
        {
            FastBitmap bmp = new FastBitmap(bitmap);

            int[] dx = { 0, 1, 1, 1, 0, -1, -1, -1 };
            int[] dy = { 1, 1, 0, -1, -1, -1, 0, 1 };

            bool[,] img = new bool[bmp.Width, bmp.Height];
            int W = bmp.Width;
            int H = bmp.Height;
            for (int i = 0; i < bmp.Width; ++i)
            {
                for (int j = 0; j < bmp.Height; ++j)
                {
                    img[i, j] = bmp[i, j].B < 128;
                }
            }


            bool pass = false;
            LinkedList<Point> list;
            do
            {
                pass = !pass;
                list = new LinkedList<Point>();

                for (int x = 1; x < W - 1; ++x)
                {
                    for (int y = 1; y < H - 1; ++y)
                    {
                        if (img[x, y])
                        {
                            int cnt = 0;
                            int hm = 0;
                            bool prev = img[x - 1, y + 1];
                            for (int i = 0; i < 8; ++i)
                            {
                                bool cur = img[x + dx[i], y + dy[i]];
                                hm += cur ? 1 : 0;
                                if (prev && !cur) ++cnt;
                                prev = cur;
                            }
                            if (hm > 1 && hm < 7 && cnt == 1)
                            {
                                if (pass && (!img[x + 1, y] || !img[x, y + 1] || !img[x, y - 1] && !img[x - 1, y]))
                                {
                                    list.AddLast(new Point(x, y));
                                }
                                if (!pass && (!img[x - 1, y] || !img[x, y - 1] || !img[x, y + 1] && !img[x + 1, y]))
                                {
                                    list.AddLast(new Point(x, y));
                                }
                            }
                        }

                    }
                }
                foreach (Point p in list)
                {
                    img[p.X, p.Y] = false;
                }
            } while (list.Count != 0);

            for (int x = 0; x < W; ++x)
            {
                for (int y = 0; y < H; ++y)
                {
                    bmp[x, y] = img[x, y] ? Color.Black : Color.White;
                }
            }
            bmp.Unlock();
            bitmap = bmp.Bitmap;
            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();
            drawHistogram();
        }
    }
}
