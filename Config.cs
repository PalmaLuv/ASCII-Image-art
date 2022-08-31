using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
            ConfigConsole cf = new ConfigConsole(150);
            ConsoleKeyInfo Key;
            ConsoleEdit.SetConsoleFont(2);
            Console.SetWindowSize(58, 58);
            Console.Clear();
            StartText();
            while(true)
            {
                Console.WriteLine("O - Conver image to ASCII\n\t!!!no save!!!\n" +
                                  "S - Saving the picture in *.txt format\n" +
                                  $"I - Setting (Image resizing. Size : {cf.MAXsize}\n\nEsc - Exit");
                Key = Console.ReadKey(true);
                if (Key.Key == ConsoleKey.O)
                {
                    Console.Clear();
                    Execution.Procesing(cf);
                }
                if (Key.Key == ConsoleKey.I)
                {
                    ConfigEdit(cf);
                    Console.Clear();
                }
                if (Key.Key == ConsoleKey.S)
                {
                    SaveFile(Execution.text);
                    Console.Clear();
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

        public static void ConfigEdit(ConfigConsole cf)
        {
            ConsoleKeyInfo Key;
            Console.Clear();
            while (true)
            {
                StartText();
                Console.WriteLine($"\n\nSetting [size : {cf.MAXsize}]:" +
                                  "\n\t1 - Edit image size.\n\nEsc - Exit");
                Key = Console.ReadKey(true);
                if(Key.Key == ConsoleKey.D1)
                {
                    Console.Write("Enter a new image size :");
                    while(!int.TryParse(Console.ReadLine(), out cf.MAXsize))
                    {
                        Console.Write("The number must be of data type \"int\". Enter a new image size : ");
                    }
                }
                if (Key.Key == ConsoleKey.Escape)
                    break;
                Console.Clear();
            }
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

                    stream.Close();

                    using (StreamWriter sw = new StreamWriter(new FileStream(LocalFilePath, FileMode.Create, FileAccess.Write), Encoding.ASCII))
                    {
                        foreach (var result in text)
                        {
                            sw.Write(result);
                        }
                        sw.Close();
                    };
                    var file = new Process();
                    file.StartInfo = new ProcessStartInfo(LocalFilePath)
                    {
                        UseShellExecute = true
                    };
                    file.Start();
                }
        }
    }
}
