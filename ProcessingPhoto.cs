using System;
using System.Drawing;
using System.Windows.Forms;

namespace ASCII
{
    public static class ProcessingPhoto
    {
        public static void ConvertToGrad(this Bitmap bitmap)
        {
            for (int i = 0; i < bitmap.Height; i++)
                for (int j = 0; j < bitmap.Width; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    int avg = (pixel.R + pixel.G + pixel.B) / 3;
                    bitmap.SetPixel(j, i, Color.FromArgb(pixel.A, avg, avg, avg));
                }
        }

        public static Bitmap ResizeBitmap(Bitmap bitmap, ConfigConsole cf)
        {
            var MAXWidth = cf.MAXsize;
            var MAXHeight = bitmap.Height / 1.5 * MAXWidth / bitmap.Width;
            if (bitmap.Width > MAXWidth || bitmap.Width > MAXHeight)
                bitmap = new Bitmap(bitmap, new Size(MAXWidth, (int)MAXHeight));
            return bitmap;
        }


    }

    public class BitmapToASCII
    {
        public ConfigConsole cf = new ConfigConsole(150);

        private readonly Bitmap bitmap;
        private readonly string ASCII = " _.,-=+:;cba!?0123456789$W#@Ñ";
        public BitmapToASCII(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public void Procesing(string FileName)
        {
            var bitmap = new Bitmap(Menu.OpenFile());
            bitmap = ResizeBitmap(bitmap, cf);
            bitmap.ConvertToGrayscale();

            var converter = new BitmapToASCIIConventer(bitmap);
            var rows = converter.ConvertBitmap();

            foreach (var row in rows)
                Console.WriteLine(row);

            Console.SetCursorPosition(0, 0);
            string text = null;
            try
            {
                for (int i = 0; i < rows.Length; i++)
                    for (int j = 0; j < rows[i].Length; j++)
                        text = rows[i][j].ToString();
                Clipboard.SetText(text);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private char[] ConvertASCII()
        {
            char[] result = new char[ASCII.Length];
            for (int i = 0; i < ASCII.Length; i++)
                result[i] = ASCII[i];
            return result;
        }
        public char[][] ConvertBitmap()
        {
            var result = new char[bitmap.Height][];
            for (int i = 0; i < bitmap.Height; i++) // it's "y" position
            {
                result[i] = new char[bitmap.Width];
                for (int j = 0; j < bitmap.Width; j++) // it's "x" position
                {
                    int mapIndex = (int)Map(bitmap.GetPixel(j, i).R, 0, 255, 0, ASCII.Length - 1);
                    result[i][j] = ConvertASCII()[mapIndex];
                }
            }
            return result;
        }

        public float Map(float valueToMap , float start1 , float stop1 , float start2 , float stop2)
        {
            return ((valueToMap - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }
    }
}
