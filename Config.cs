using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCII
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ConsoleFont
    {
        public uint Index;
        public short SizeX, SizeY;
    }
    public static class ConsoleEdit
    {
        [DllImport("kernel32")]
        private extern static bool SetConsoleFont(IntPtr hOutput, uint index);
        private enum StdHandle
        {
            OutputHandle = -11
        }

        [DllImport("kernel32")]
        private static extern IntPtr GetStdHandle(StdHandle index);
        public static bool SetConsoleFont(uint index)
        {
            return SetConsoleFont(GetStdHandle(StdHandle.OutputHandle), index);
        }
    }
    public class ConfigConsole
    {
        public int MAXsize;
        public ConfigConsole(int size)
        {
            this.MAXsize = size; 
        }
    }

    public static class Menu
    {
        public static void menu()
        {
            ConsoleKeyInfo Key;
            while(true)
            {
                ConsoleEdit.SetConsoleFont(2);
                Console.SetWindowSize(58, 58);
                Console.Clear();
                StartText();
                Console.WriteLine("\nPlease send key '1'");
                Key = Console.ReadKey(true); 
                if(Key.Key == ConsoleKey.D1)
                {
                    while(true)
                    {
                        Console.WriteLine("O - Conver image to ASCII\n\t!!!no save!!!");
                        Key = Console.ReadKey(true);
                        if (Key.Key == ConsoleKey.O)
                        {
                            var bitmap = new Bitmap(OpenFile());
                            BitmapToASCII BtASCII = new BitmapToASCII(bitmap);
                            Console.Clear();
                            BtASCII.Procesing();
                        }
                        if (Key.Key == ConsoleKey.Escape)
                            break;
                    }    
                }
                if (Key.Key == ConsoleKey.Escape)
                    break;
            }
        }

        public static void StartText()
        {
            string[] StartText = { 
                "░█████╗░░██████╗░█████╗░██╗██╗\t░█████╗░██████╗░████████╗",
                "██╔══██╗██╔════╝██╔══██╗██║██║\t██╔══██╗██╔══██╗╚══██╔══╝",
                "███████║╚█████╗░██║░░╚═╝██║██║\t███████║██████╔╝░░░██║░░░",
                "██╔══██║░╚═══██╗██║░░██╗██║██║\t██╔══██║██╔══██╗░░░██║░░░",
                "██║░░██║██████╔╝╚█████╔╝██║██║\t██║░░██║██║░░██║░░░██║░░░",
                "╚═╝░░╚═╝╚═════╝░░╚════╝░╚═╝╚═╝\t╚═╝░░╚═╝╚═╝░░╚═╝░░░╚═╝░░░"
            };

            foreach (string t in StartText)
            {
                Console.WriteLine(t);
            }
        }

        public static int ConfigEdit()
        {
            return 0;
        }

        public static string LanguageSelection()
        {
            return "en-EN";
        }

        public static string OpenFile()
        {
            var OpenFile = new OpenFileDialog
            {
                Filter = "Images | *.jpg; *.png; *.bmp; *.JPEG"
            };
            if (OpenFile.ShowDialog() != DialogResult.OK)
                return null;
            return OpenFile.FileName;
        }

        public static void SaveFile(string text)
        {
            Stream stream;
            string LocalFilePath;
            var SaveFile = new SaveFileDialog
            {
                Filter = "Text | *.txt"
            };
            
            if (SaveFile.ShowDialog() == DialogResult.OK)
                if ((stream = SaveFile.OpenFile())!= null)
                {
                    LocalFilePath = SaveFile.FileName.ToString();
                    
                    //FileName = LocalFilePath.Substring(LocalFilePath.LastIndexOf("\\") + 1);
                    //FilePath = LocalFilePath.Substring(0, LocalFilePath.LastIndexOf("\\"));

                    stream.Close();

                    using (StreamWriter sw = new StreamWriter(LocalFilePath))
                    {
                        foreach (var result in text)
                        {
                            sw.Write(result);
                        }
                        sw.Close();
                    };
                }
        }
    }
}
