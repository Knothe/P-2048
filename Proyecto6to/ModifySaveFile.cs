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
        private static string _fileName = "gameSaved";
        private static string _highScore = "highScore   ";
        public static bool FileExists()
        {
            return File.Exists(_fileName);
        }

        public static void SaveHighScore(int highScore)
        {
            string newText = "" + highScore;
            File.WriteAllText(_highScore, newText);
        }
        public static int ReadHighScore()
        {
            if (File.Exists(_highScore))
            {
                string content = File.ReadAllText(_highScore);
                return Int32.Parse(content);
            }
            return 0;   
        }
        public static void SaveFile(int score, int [,] tileNumber)
        {
            string newText = "" + score + " ";
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    newText += tileNumber[i, j] + " ";
                }
            }
            File.WriteAllText(_fileName, newText);
        }

        public static void ReadFile(ref int score, ref int[,] tileNumber)
        {
            string content = File.ReadAllText(_fileName);
            string[] values = content.Split(' ', '\n');
            int cont = 0;
            score = Int32.Parse(values[cont]);
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    cont++;
                    tileNumber[i, j] = Int32.Parse(values[cont]);
                }
            }
        }

        public static void DeleteFile()
        {
            File.Delete(_fileName);
        }
    }
}
