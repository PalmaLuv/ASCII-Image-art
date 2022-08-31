using System;
using System.IO;
using System.Windows.Forms;

namespace ASCII
{
    public class ConfigConsole
    {
        public int MAXsize {get; set;}
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
                Key = Console.ReadKey(true); 
                if(Key.Key == ConsoleKey.D1)
                {
                    while(true)
                    {
                        Key = Console.ReadKey(true);
                        if (Key.Key == ConsoleKey.O)
                        {
                            BitmapToASCII.d();
                        }
                        if (Key.Key == ConsoleKey.Escape)
                            break;
                    }    
                }
                if (Key.Key == ConsoleKey.Escape)
                    break;
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
