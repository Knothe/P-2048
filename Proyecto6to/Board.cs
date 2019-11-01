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
        List<Animation> animList = new List<Animation>();
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
        public void Draw(SpriteBatch spriteBatch)
        {
            if (moved)
            {
                bool hasMotion = false;
                for(int i = 0; i < animList.Count(); i++)
                {
                    bool tile1Moved = false;
                    bool tile2Moved = false;
                    tile1Moved = animList[i].MoveTile1();
                    spriteBatch.Draw(tileList[animList[i].val], new Vector2(367 + (197 * animList[i].tile1.X), 31 + (191 * animList[i].tile1.Y)));
                    if (animList[i].t3 == 1)
                    {
                        tile2Moved = animList[i].MoveTile2();
                        spriteBatch.Draw(tileList[animList[i].val], new Vector2(367 + (197 * animList[i].tile2.X), 31 + (191 * animList[i].tile2.Y)));
                    }
                    if (!hasMotion)
                    {
                        if (tile1Moved || tile2Moved)
                            hasMotion = true;
                    }
                        
                }
                if (!hasMotion)
                {
                    animList.Clear();
                    moved = false;
                }
                   
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (tileNumber[i, j] != -1)
                            spriteBatch.Draw(tileList[tileNumber[i, j]], new Vector2(367 + (197 * i), 31 + (191 * j)));
                    }
                }
            }
            
        }
        public int GetHighScore() {
            return highScore;
        }
        public int GetScore() {
            return score;
        }
        public bool GetLose() {
            return lose;
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
            if (moved)
                return;
            int[,] prevTileProt = new int[4, 4];
            EqualTileNumbers(ref prevTileProt, ref tileNumber);
            prevScore = score;
            animList.Clear();
            switch (move)
            {
                case 0: 
                    moved = Move0();
                    break;
                case 1: 
                    moved = Move1();
                    break;
                case 2:
                    moved = Move2();
                    break;
                case 3:
                    moved = Move3();
                    break;
                case 4:
                    moved = Move4();
                    break;
                case 5:
                    moved = Move5();
                    break;
                case 6:
                    moved = Move6();
                    break;
                case 7:
                    moved = Move7();
                    break;
            }
            if (moved)
            {
                EqualTileNumbers(ref prevTileNumber, ref prevTileProt);
                SetRandomTile();
                //moved = false;
                if (score > highScore)
                {
                    highScore = score;
                    ModifySaveFile.SaveHighScore(highScore);
                }
                if (isFull())
                {
                    if (hasLost())
                        lose = true;
                }
            }
        }
        // Derecha
        private bool Move0()
        {
            int k, tileVal;
            bool wasMoved = false; ;
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j >= 0; j--)
                {
                    if (tileNumber[j, i] != -1)
                    {
                        Animation anim = new Animation();
                        anim.tile1 = new Vector2(j, i);
                        anim.val = tileNumber[j, i];
                        if (j != 0)
                        {
                            for (k = j - 1; k >= 0; k--)
                            {
                                if (tileNumber[k, i] != -1)
                                {
                                    if (tileNumber[j, i] == tileNumber[k, i])
                                    {
                                        tileNumber[j, i]++;
                                        tileNumber[k, i] = -1;
                                        score += tileNumber[j, i];
                                        anim.tile2 = new Vector2(k, i);
                                        anim.t3 = 1;
                                        wasMoved = true;
                                    }
                                    k = -1;
                                }
                            }
                        }
                        tileVal = tileNumber[j, i];
                        anim.endingTile = anim.tile1;
                        for (k = j + 1; k < 4; k++)
                        {
                            if (tileNumber[k, i] == -1)
                            {
                                tileNumber[k, i] = tileVal;
                                tileNumber[k - 1, i] = -1;
                                anim.endingTile = new Vector2(k, i);
                                wasMoved = true;
                            }
                            else
                            {
                                k--;
                                break;
                            }
                        }
                        anim.SetMovement();
                        animList.Add(anim);
                    }
                }
            }
            return wasMoved;
        }
        // Arriba derecha
        private bool Move1()
        {
            bool wasMoved = false;
            int tileVal = 0;
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tileNumber[i, j] != -1)
                    {
                        Animation anim = new Animation();
                        anim.tile1 = new Vector2(i, j);
                        anim.val = tileNumber[i, j];
                        if (i != 0 && j != 3)
                        {
                            for (int k = 1; k < 4; k++)
                            {
                                try
                                {
                                    if (tileNumber[i - k, j + k] != -1)
                                    {
                                        if (tileNumber[i, j] == tileNumber[i - k, j + k])
                                        {
                                            tileNumber[i, j]++;
                                            tileNumber[i - k, j + k] = -1;
                                            score += tileNumber[i, j];
                                            anim.tile2 = new Vector2(i - k, j + k);
                                            anim.t3 = 1;
                                            wasMoved = true;
                                        }
                                        k = 5;
                                    }

                                }
                                catch (Exception e)
                                {
                                    k = 5;
                                }
                            }
                        }
                        anim.endingTile = anim.tile1;
                        if (i != 3 && j != 0)
                        {
                            tileVal = tileNumber[i, j];
                            for (int k = 1; k < 4; k++)
                            {
                                try
                                {
                                    if (tileNumber[i + k, j - k] == -1)
                                    {
                                        tileNumber[i + k, j - k] = tileVal;
                                        tileNumber[i + k - 1, j - k + 1] = -1;
                                        anim.endingTile = new Vector2(i + k, j - k);
                                        wasMoved = true;
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                                catch (Exception e)
                                {
                                    k = 5;
                                }
                            }
                        }
                        anim.SetMovement();
                        animList.Add(anim);
                    }

                }
            }
            return wasMoved;
        }
        // Arriba
        private bool Move2()
        {
            int k, tileVal;
            bool wasMoved = false; ;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tileNumber[i, j] != -1)
                    {
                        Animation anim = new Animation();
                        anim.tile1 = new Vector2(i, j);
                        anim.val = tileNumber[i, j];
                        if (j != 3)
                        {
                            for (k = j + 1; k < 4; k++)
                            {
                                if (tileNumber[i, k] != -1)
                                {
                                    if (tileNumber[i, j] == tileNumber[i, k])
                                    {
                                        tileNumber[i, j]++;
                                        tileNumber[i, k] = -1;
                                        score += tileNumber[i, j];
                                        anim.tile2 = new Vector2(i, k);
                                        anim.t3 = 1;
                                        wasMoved = true;
                                    }
                                    k = 5;
                                }
                            }
                        }
                        tileVal = tileNumber[i, j];
                        anim.endingTile = anim.tile1;
                        for (k = j - 1; k >= 0; k--)
                        {
                            if (tileNumber[i, k] == -1)
                            {
                                tileNumber[i, k] = tileVal;
                                tileNumber[i, k + 1] = -1;
                                anim.endingTile = new Vector2(i, k);
                                wasMoved = true;
                            }
                            else
                            {
                                k++;
                                break;
                            }
                        }
                        anim.SetMovement();
                        animList.Add(anim);
                    }
                }
            }
            return wasMoved;
        }
        // Arriba izquierda
        private bool Move3()
        {
            int unir, mover, tileVal, k;
            bool wasMoved = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tileNumber[i, j] != -1)
                    {
                        Animation anim = new Animation();
                        anim.tile1 = new Vector2(i, j);
                        anim.val = tileNumber[i, j];
                        if (i >= j)
                        {
                            unir = 3 - i;
                            mover = j;
                        }
                        else
                        {
                            unir = 3 - j;
                            mover = i;
                        }
                        for (k = 1; k <= unir; k++)
                        {
                            if (tileNumber[i + k, j + k] != -1)
                            {
                                if (tileNumber[i, j] == tileNumber[i + k, j + k])
                                {
                                    tileNumber[i, j]++;
                                    score += tileNumber[i, j];
                                    tileNumber[i + k, j + k] = -1;
                                    anim.tile2 = new Vector2(i + k, j + k);
                                    anim.t3 = 1;
                                    wasMoved = true;
                                }
                                k = 5;
                            }
                        }
                        tileVal = tileNumber[i, j];
                        anim.endingTile = anim.tile1;
                        for (k = 1; k <= mover; k++)
                        {
                            if (tileNumber[i - k, j - k] == -1)
                            {
                                tileNumber[i - k, j - k] = tileVal;
                                tileNumber[i - k + 1, j - k + 1] = -1;
                                anim.endingTile = new Vector2(i - k, j - k);
                                wasMoved = true;
                            }
                            else
                            {
                                break;
                            }
                        }
                        anim.SetMovement();
                        animList.Add(anim);
                    }
                }
            }
            return wasMoved;
        }
        // Izquierda
        private bool Move4()
        {
            int k, tileVal;
            bool wasMoved = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tileNumber[j, i] != -1)
                    {
                        Animation anim = new Animation();
                        anim.tile1 = new Vector2(j, i);
                        anim.val = tileNumber[j, i];
                        if (j != 3)
                        {
                            for (k = j + 1; k < 4; k++)
                            {
                                if (tileNumber[k, i] != -1)
                                {
                                    if (tileNumber[j, i] == tileNumber[k, i])
                                    {
                                        tileNumber[j, i]++;
                                        score += tileNumber[j, i];
                                        tileNumber[k, i] = -1;
                                        anim.tile2 = new Vector2(k, i);
                                        anim.t3 = 1;
                                        wasMoved = true;
                                    }
                                    k = 5;
                                }
                            }
                        }
                        tileVal = tileNumber[j, i];
                        anim.endingTile = anim.tile1;
                        for (k = j - 1; k >= 0; k--)
                        {
                            if (tileNumber[k, i] == -1)
                            {
                                tileNumber[k, i] = tileVal;
                                tileNumber[k + 1, i] = -1;
                                anim.endingTile = new Vector2(k, i);
                                wasMoved = true;
                            }
                            else
                            {
                                k++;
                                break;
                            }
                        }
                        anim.SetMovement();
                        animList.Add(anim);
                    }
                }
            }
            return wasMoved;
        }
        // Abajo izquierdo
        private bool Move5()
        {
            bool wasMoved = false;
            int tileVal = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 3; j >= 0; j--)
                {
                    if (tileNumber[i, j] != -1)
                    {
                        Animation anim = new Animation();
                        anim.tile1 = new Vector2(i, j);
                        anim.val = tileNumber[i, j];
                        if (i != 3 && j != 0)
                        {
                            for (int k = 1; k < 4; k++)
                            {
                                try
                                {
                                    if (tileNumber[i + k, j - k] != -1)
                                    {
                                        if (tileNumber[i, j] == tileNumber[i + k, j - k])
                                        {
                                            tileNumber[i, j]++;
                                            tileNumber[i + k, j - k] = -1;
                                            score += tileNumber[i, j];
                                            anim.tile2 = new Vector2(i + k, j - k);
                                            anim.t3 = 1;
                                            wasMoved = true;
                                        }
                                        k = 5;
                                    }

                                }
                                catch (Exception e)
                                {
                                    k = 5;
                                }
                            }
                        }
                        anim.endingTile = anim.tile1;
                        if (i != 0 && j != 3)
                        {
                            tileVal = tileNumber[i, j];
                            for (int k = 1; k < 4; k++)
                            {
                                try
                                {
                                    if (tileNumber[i - k, j + k] == -1)
                                    {
                                        tileNumber[i - k, j + k] = tileVal;
                                        tileNumber[i - k + 1, j + k - 1] = -1;
                                        anim.endingTile = new Vector2(i - k, j + k);
                                        wasMoved = true;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    k = 5;
                                }
                            }
                        }
                        anim.SetMovement();
                        animList.Add(anim);
                    }

                }
            }
            return wasMoved;
        }
        // Abajo
        private bool Move6()
        {
            int k, tileVal;
            bool wasMoved = false;
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j >= 0; j--)
                {
                    if (tileNumber[i, j] != -1)
                    {
                        Animation anim = new Animation();
                        anim.tile1 = new Vector2(i, j);
                        anim.val = tileNumber[i, j];
                        if (j != 0)
                        {
                            for (k = j - 1; k >= 0; k--)
                            {
                                if (tileNumber[i, k] != -1)
                                {
                                    if (tileNumber[i, j] == tileNumber[i, k])
                                    {
                                        tileNumber[i, j]++;
                                        score += tileNumber[i, j];
                                        tileNumber[i, k] = -1;
                                        anim.tile2 = new Vector2(i, k);
                                        anim.t3 = 1;
                                        wasMoved = true;
                                    }
                                    k = -1;
                                }
                            }
                        }
                        tileVal = tileNumber[i, j];
                        anim.endingTile = anim.tile1;
                        for (k = j + 1; k < 4; k++)
                        {
                            if (tileNumber[i, k] == -1)
                            {
                                tileNumber[i, k] = tileVal;
                                tileNumber[i, k - 1] = -1;
                                anim.endingTile = new Vector2(i, k);
                                wasMoved = true;
                            }
                            else
                            {
                                k--;
                                break;
                            }
                        }
                        anim.SetMovement();
                        animList.Add(anim);
                    }
                }
            }
            return wasMoved;
        }
        // Abajo derecha
        private bool Move7()
        {
            int unir, mover, tileVal, k;
            bool wasMoved = false;
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j >= 0; j--)
                {
                    if (tileNumber[i, j] != -1)
                    {
                        Animation anim = new Animation();
                        anim.tile1 = new Vector2(i, j);
                        anim.val = tileNumber[i, j];
                        if (i <= j)
                        {
                            unir = i;
                            mover = 3 - j;
                        }
                        else
                        {
                            unir = j;
                            mover = 3 - i;
                        }
                        for (k = 1; k <= unir; k++)
                        {
                            if (tileNumber[i - k, j - k] != -1)
                            {
                                if (tileNumber[i, j] == tileNumber[i - k, j - k])
                                {
                                    tileNumber[i, j]++;
                                    score += tileNumber[i, j];
                                    tileNumber[i - k, j - k] = -1;
                                    anim.tile2 = new Vector2(i - k, j - k);
                                    anim.t3 = 1;
                                    wasMoved = true;
                                }
                                k = 5;
                            }
                        }
                        tileVal = tileNumber[i, j];
                        anim.endingTile = anim.tile1;
                        for (k = 1; k <= mover; k++)
                        {
                            if (tileNumber[i + k, j + k] == -1)
                            {
                                tileNumber[i + k, j + k] = tileVal;
                                tileNumber[i + k - 1, j + k - 1] = -1;
                                anim.endingTile = new Vector2(i + k, j + k);
                                wasMoved = true;
                            }
                            else
                            {
                                break;
                            }
                        }
                        anim.SetMovement();
                        animList.Add(anim);
                    }
                }
            }
            return wasMoved;
        }
        private bool isFull() {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (tileNumber[i, j] == -1) {
                        return false;
                    }
                }
            }
            return true;
        }
        private bool hasLost() {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (i != 3 && j != 0) {
                        if (tileNumber[i, j] == tileNumber[i + 1, j - 1])
                            return false;
                    }
                    if (i != 3) {
                        if (tileNumber[i, j] == tileNumber[i + 1, j])
                            return false;
                    }
                    if (i != 3 && j != 3) {
                        if (tileNumber[i, j] == tileNumber[i + 1, j + 1])
                            return false;
                    }
                    if (j != 3) {
                        if (tileNumber[i, j] == tileNumber[i, j + 1])
                            return false;
                    }
                }
            }
            return true;
        }
        private void EqualTileNumbers(ref int[,] arr1, ref int[,] arr2) {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    arr1[i, j] = arr2[i, j];
                }
            }
        }
        public void SaveGame() {
            ModifySaveFile.SaveFile(score, tileNumber);
        }
        public void Back() {
            EqualTileNumbers(ref tileNumber, ref prevTileNumber);
            score = prevScore;
        }
    }
}
