using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Proyecto6to
{
    class ModifySaveFile
    {
        private static string _fileName = "scores";
        public static bool FileExists()
        {
            return File.Exists(_fileName);
        }

        public static void SaveFile(int highScore, int [,] tileNumber)
        {
            string newText = "" + highScore + "\n";
            for(int j = 0; j < 4; j++)
            {
                for(int i = 0; i < 4; i++)
                {
                    newText += tileNumber[i, j] + " ";
                }
                newText += "\n";
            }
            File.WriteAllText(_fileName, newText);
        }

        public static void ReadFile(ref int highScore, ref int[,] tileNumber)
        {
            string content = File.ReadAllText(_fileName);
        }
    }
}
