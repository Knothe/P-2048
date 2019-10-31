using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Proyecto6to
{
    class Board
    {
        private const double radToAngle = 180 / Math.PI;
        private bool lose = false;
        private int highScore, score, prevScore;
        private bool moved;
        private int[,] tileNumber = new int[4, 4];
        private int[,] prevTileNumber = new int[4, 4];
        private Texture2D[] tileList = new Texture2D[12];

        public Board()
        {
            highScore = ModifySaveFile.ReadHighScore();
            moved = false;
        }

        public void Load(Game game)
        {
            tileList[0] = game.Content.Load<Texture2D>("Tile1");
            tileList[1] = game.Content.Load<Texture2D>("Tile2");
            tileList[2] = game.Content.Load<Texture2D>("Tile3");
            tileList[3] = game.Content.Load<Texture2D>("Tile4");
            tileList[4] = game.Content.Load<Texture2D>("Tile5");
            tileList[5] = game.Content.Load<Texture2D>("Tile6");
            tileList[6] = game.Content.Load<Texture2D>("Tile7");
            tileList[7] = game.Content.Load<Texture2D>("Tile8");
            tileList[8] = game.Content.Load<Texture2D>("Tile9");
            tileList[9] = game.Content.Load<Texture2D>("Tile10");
            tileList[10] = game.Content.Load<Texture2D>("Tile11");
            tileList[11] = game.Content.Load<Texture2D>("Tile12");
        }

        public void SetNewGame()
        {
            ModifySaveFile.DeleteFile();
            ModifySaveFile.DeleteFile();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tileNumber[i, j] = -1;
                    prevTileNumber[i, j] = -1;
                }
            }
            prevScore = 0;
            score = 0;
            lose = false;
            SetRandomTile();
        }
        public void OpenSavedGame()
        {
            ModifySaveFile.ReadFile(ref score, ref tileNumber);
            EqualTileNumbers(ref prevTileNumber, ref tileNumber);
            prevScore = score;
        }
        private void SetRandomTile()
        {
            int tileN = 0;
            tileN = score / 200;
            Random r = new Random();
            bool wasAdded = false;
            int x, y;
            while (!wasAdded)
            {
                x = r.Next(0, 4);
                y = r.Next(0, 4);
                if (tileNumber[x, y] == -1)
                {
                    tileNumber[x, y] = r.Next(0, tileN + 1);
                    wasAdded = true;
                }
            }
        }
        public void MakeMovement(int move)
        {
            int[,] prevTileProt = new int[4, 4];
            EqualTileNumbers(ref prevTileProt, ref tileNumber);
            prevScore = score;
            switch (move)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
            }
        }
        private void EqualTileNumbers(ref int[,] arr1, ref int[,] arr2)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    arr1[i, j] = arr2[i, j];
                }
            }
        }
    }
}
