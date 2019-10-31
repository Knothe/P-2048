using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Proyecto6to.Scenes
{
    class GameState : Scene
    {
        private SpriteFont font;
        private SpriteFont smallFont;
        private Texture2D backGround;
        private Texture2D icon;
        private Texture2D title;
        private Texture2D grid;
        private Texture2D gameOver;
        private Texture2D gameOver2;
        private Texture2D[] tileList = new Texture2D[12];
        private int[,] tileNumber = new int[4, 4];
        private int[,] prevTileNumber = new int[4, 4];
        
        private Button back;
        private Button reload;
        private Button save;
        private bool mousePressed = false;
        private Vector2 mousePos;
        private const double radToAngle = 180 / Math.PI;
        private bool moved;
        private int highScore, score, prevScore;
        private bool lose = false;

        public GameState()
        {
            highScore = ModifySaveFile.ReadHighScore();
            moved = false;
            back = new Button(new Vector2(1136, 598), new Vector2(1, 1));
            reload = new Button(new Vector2(1288, 598), new Vector2(1, 1));
            save = new Button(new Vector2(30, 598), new Vector2(1, 1));
            mousePos = new Vector2(0, 0);
        }
        public override void Init()
        {
            
        }
        public override void Load(Game game)
        {
            backGround = game.Content.Load<Texture2D>("Background");
            icon = game.Content.Load<Texture2D>("IconTest");
            back.Load(game, "Return", "Return2", "Return3");
            reload.Load(game, "Restart", "Restart2", "Restart3");
            save.Load(game, "Save", "Save2", "Save3");
            title = game.Content.Load<Texture2D>("MainTitle");
            font = game.Content.Load<SpriteFont>("File");
            smallFont = game.Content.Load<SpriteFont>("TextoPequeño");
            grid = game.Content.Load<Texture2D>("MainGame");
            gameOver = game.Content.Load<Texture2D>("GameOverScreen");
            gameOver2 = game.Content.Load<Texture2D>("GameOverScreen2");
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
            while (!wasAdded){
                x = r.Next(0, 4); 
                y = r.Next(0, 4);
                if (tileNumber[x, y] == -1)
                {
                    tileNumber[x, y] = r.Next(0, tileN + 1);
                    wasAdded = true;
                }
            }
            
        }
        public override int Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (!lose)
            {
                if (save.Update(mouseState.Position.ToVector2(), mouseState.LeftButton))
                {
                    mousePressed = false;
                    ModifySaveFile.SaveFile(score, tileNumber);
                }
                if (back.Update(mouseState.Position.ToVector2(), mouseState.LeftButton))
                {
                    mousePressed = false;
                    EqualTileNumbers(ref tileNumber, ref prevTileNumber);
                    score = prevScore;
                }
                if (!mousePressed && mouseState.LeftButton == ButtonState.Pressed)
                {
                    mousePressed = true;
                    mousePos = mouseState.Position.ToVector2();
                }
                else if (mouseState.LeftButton == ButtonState.Released && mousePressed)
                {
                    mousePressed = false;
                    CheckMovement(mouseState.Position.ToVector2());
                }
            }
            
            if(reload.Update(mouseState.Position.ToVector2(), mouseState.LeftButton)){
                mousePressed = false;
                SetNewGame();
            }
            
            
            return 0;
        }
        public override void Draw(SpriteBatch spriteBatch)
        { 
            
            spriteBatch.Draw(backGround, new Vector2(0, 0), scale: new Vector2(.75f, .75f));
            spriteBatch.Draw(grid, new Vector2(336, 0));

            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if(tileNumber[i,j] != -1)   
                        spriteBatch.Draw(tileList[tileNumber[i,j]], new Vector2(367 + (197 * i), 31 + (191 * j)));
                }
            }
            spriteBatch.Draw(title, new Vector2(25, 75), scale: new Vector2(.3f, .3f));
            spriteBatch.DrawString(font, "High\nScore", new Vector2(1136, 50), Color.White);
            spriteBatch.DrawString(smallFont, "" + highScore, new Vector2(1136, 160), Color.White);
            spriteBatch.DrawString(font, "\nScore", new Vector2(1288, 50), Color.White);
            spriteBatch.DrawString(smallFont, "" + score, new Vector2(1288, 160), Color.White);
            back.Draw(spriteBatch);
            save.Draw(spriteBatch);
            if (lose)
            {
                spriteBatch.Draw(gameOver2, new Vector2(0, 0), scale: new Vector2(2,2));
                spriteBatch.Draw(gameOver, new Vector2(240, 128));
            }
            reload.Draw(spriteBatch);
        }
        public override void Destroy()
        {

        }
        private bool isFull()
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if(tileNumber[i,j] == -1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private bool hasLost()
        {
            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(i != 3 && j != 0)
                    {
                        if (tileNumber[i, j] == tileNumber[i + 1, j - 1])
                            return false;
                    }
                    if(i != 3)
                    {
                        if (tileNumber[i, j] == tileNumber[i + 1, j])
                            return false;
                    }
                    if(i != 3 && j != 3)
                    {
                        if (tileNumber[i, j] == tileNumber[i + 1, j + 1])
                            return false;
                    }
                    if(j != 3)
                    {
                        if (tileNumber[i, j] == tileNumber[i, j + 1])
                            return false;
                    }

                }
            }
            return true;
        }
        
        private void CheckMovement(Vector2 newMousePos) {
            int[,] prevTileProt = new int[4, 4];
            newMousePos.Y = newMousePos.Y - mousePos.Y;
            newMousePos.X = newMousePos.X - mousePos.X;
            double angle = Math.Atan2(newMousePos.Y, newMousePos.X);
            if(newMousePos.Length() > 0)
            {
                EqualTileNumbers(ref prevTileProt, ref tileNumber);
                prevScore = score;
                angle *= radToAngle;
                if (angle <= 22.5 && angle >= -22.5) moved = Move0(); //Derecha
                else if (angle <= -22.5 && angle >= -67.5) moved = Move1(); //Arriba derecha
                else if (angle <= -67.5 && angle >= -112.5) moved = Move2(); //Arriba 
                else if (angle <= -112.5 && angle >= -157.5) moved = Move3(); //Arriba izquierda
                else if (angle <= -157.5 || angle >= 157.5) moved = Move4(); //Izquierda
                else if (angle <= 157.5 && angle >= 112.5) moved = Move5(); //Izquierda abajo
                else if (angle <= 112.5 && angle >= 67.5) moved = Move6(); //Abajo
                else moved = Move7(); //Izquierda derecha
            }
            if (moved) {
                EqualTileNumbers(ref prevTileNumber, ref prevTileProt);
                SetRandomTile();
                moved = false;
                if(score > highScore)
                {
                    highScore = score;
                    ModifySaveFile.SaveHighScore(highScore);
                }
                if (isFull())
                {
                    if(hasLost())
                        lose = true;
                }
            }
        }
        // Derecha
        private bool Move0() {
            int k, tileVal;
            bool wasMoved = false; ;
            for (int i = 3; i >= 0; i--) {
                for (int j = 3; j >= 0; j--) {
                    if (tileNumber[j, i] != -1) {
                        if (j != 0) {
                            for (k = j - 1; k >= 0; k--) {
                                if (tileNumber[k, i] != -1){
                                    if (tileNumber[j, i] == tileNumber[k, i]) {
                                        tileNumber[j, i]++;
                                        tileNumber[k, i] = -1;
                                        score += tileNumber[j, i];
                                        wasMoved = true;
                                    }
                                    k = -1;
                                }
                            }
                        }
                        tileVal = tileNumber[j, i];
                        for(k = j + 1; k < 4; k++) {
                            if (tileNumber[k, i] == -1) {
                                tileNumber[k, i] = tileVal;
                                tileNumber[k - 1, i] = -1;
                                wasMoved = true;
                            }
                            else {
                                k--;
                                break;
                            }  
                        }
                    }
                }
            }
            return wasMoved;
        }
        // Arriba derecha
        private bool Move1() {
            bool wasMoved = false;
            int tileVal = 0;
            for(int i = 3; i >= 0; i--) {
                for(int j = 0; j < 4; j++) {
                    if(tileNumber[i,j] != -1) {
                        if (i != 0 && j != 3) {
                            for (int k = 1; k < 4; k++) {
                                try  {
                                    if (tileNumber[i - k, j + k] != -1) {
                                        if (tileNumber[i, j] == tileNumber[i - k, j + k]) {
                                            tileNumber[i, j]++;
                                            tileNumber[i - k, j + k] = -1;
                                            score += tileNumber[i, j];
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
                        if(i != 3 && j != 0) {
                            tileVal = tileNumber[i, j];
                            for (int k = 1; k < 4; k++) {
                                try {
                                    if (tileNumber[i + k, j - k] == -1) {
                                        tileNumber[i + k, j - k] = tileVal;
                                        tileNumber[i + k - 1, j - k + 1] = -1;
                                        wasMoved = true;
                                    }
                                    else {
                                        break;
                                    }

                                }
                                catch (Exception e) {
                                    k = 5;
                                }
                            }
                        }
                    }
                    
                }
            }
            return wasMoved;
        }
        // Arriba
        private bool Move2() {
            int k, tileVal;
            bool wasMoved = false; ;
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (tileNumber[i, j] != -1) {
                        if (j != 3) {
                            for (k = j + 1; k < 4; k++) {
                                if (tileNumber[i, k] != -1) {
                                    if (tileNumber[i, j] == tileNumber[i, k]) {
                                        tileNumber[i, j]++;
                                        tileNumber[i, k] = -1;
                                        score += tileNumber[i, j];
                                        wasMoved = true;
                                    }
                                    k = 5;
                                }
                            }
                        }
                        tileVal = tileNumber[i, j];
                        for (k = j - 1; k >= 0; k--) {
                            if (tileNumber[i, k] == -1) {
                                tileNumber[i, k] = tileVal;
                                tileNumber[i, k + 1] = -1;
                                wasMoved = true;
                            }
                            else {
                                k++;
                                break;
                            }
                        }
                    }
                }
            }
            return wasMoved;
        }
        // Arriba izquierda
        private bool Move3() {
            int unir, mover, tileVal, k;
            bool wasMoved = false;
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (tileNumber[i, j] != -1) {
                        if (i >= j) {
                            unir = 3 - i;
                            mover = j;
                        }
                        else {
                            unir = 3 - j;
                            mover = i;
                        }
                        for (k = 1; k <= unir; k++) {
                            if (tileNumber[i + k, j + k] != -1) {
                                if (tileNumber[i, j] == tileNumber[i + k, j + k]) {
                                    tileNumber[i, j]++;
                                    score += tileNumber[i, j];
                                    tileNumber[i + k, j + k] = -1;
                                    wasMoved = true;
                                }
                                k = 5;
                            }
                        }
                        tileVal = tileNumber[i, j];
                        for (k = 1; k <= mover; k++) {
                            if (tileNumber[i - k, j - k] == -1) {
                                tileNumber[i - k, j - k] = tileVal;
                                tileNumber[i - k + 1, j - k + 1] = -1;
                                wasMoved = true;
                            }
                            else {
                                break;
                            }
                        }
                    }
                }
            }
            return wasMoved;
        }
        // Izquierda
        private bool Move4() {
            int k, tileVal;
            bool wasMoved = false;
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (tileNumber[j, i] != -1) {
                        if (j != 3) {
                            for (k = j + 1; k < 4; k++) {
                                if (tileNumber[k, i] != -1) {
                                    if (tileNumber[j, i] == tileNumber[k, i]) {
                                        tileNumber[j, i]++;
                                        score += tileNumber[j, i];
                                        tileNumber[k, i] = -1;
                                        wasMoved = true;
                                    }
                                    k = 5;
                                }
                            }
                        }
                        tileVal = tileNumber[j, i];
                        for (k = j - 1; k >= 0; k--) {
                            if (tileNumber[k, i] == -1) {
                                tileNumber[k, i] = tileVal;
                                tileNumber[k + 1, i] = -1;
                                wasMoved = true;
                            }
                            else {
                                k++;
                                break;
                            }
                        }
                    }
                }
            }
            return wasMoved;
        }
        // Abajo izquierdo
        private bool Move5() {
            bool wasMoved = false;
            int tileVal = 0;
            for (int i = 0; i < 4; i++) {
                for (int j = 3; j >= 0; j--) {
                    if (tileNumber[i, j] != -1) {
                        if (i != 3 && j != 0) {
                            for (int k = 1; k < 4; k++) {
                                try {
                                    if (tileNumber[i + k, j - k] != -1) {
                                        if (tileNumber[i, j] == tileNumber[i + k, j - k]) {
                                            tileNumber[i, j]++;
                                            tileNumber[i + k, j - k] = -1;
                                            score += tileNumber[i, j];
                                            wasMoved = true;
                                        }
                                        k = 5;
                                    }

                                }
                                catch (Exception e) {
                                    k = 5;
                                }
                            }
                        }
                        if (i != 0 && j != 3) {
                            tileVal = tileNumber[i, j];
                            for (int k = 1; k < 4; k++) {
                                try {
                                    if (tileNumber[i - k, j + k] == -1) {
                                        tileNumber[i - k, j + k] = tileVal;
                                        tileNumber[i - k + 1, j + k - 1] = -1;
                                        wasMoved = true;
                                    }
                                    else {
                                        break;
                                    }
                                }
                                catch (Exception e) {
                                    k = 5;
                                }
                            }
                        }
                    }

                }
            }
            return wasMoved;
        }
        // Abajo
        private bool Move6() {
            int k, tileVal;
            bool wasMoved = false;
            for (int i = 3; i >= 0; i--){
                for (int j = 3; j >= 0; j--) {
                    if (tileNumber[i, j] != -1) {
                        if (j != 0) {
                            for (k = j - 1; k >= 0; k--) {
                                if (tileNumber[i, k] != -1)  {
                                    if (tileNumber[i, j] == tileNumber[i, k]) {
                                        tileNumber[i, j]++;
                                        score += tileNumber[i, j];
                                        tileNumber[i, k] = -1;
                                        wasMoved = true;
                                    }
                                    k = -1;
                                }
                            }
                        }
                        tileVal = tileNumber[i, j];
                        for (k = j + 1; k < 4; k++) {
                            if (tileNumber[i, k] == -1) {
                                tileNumber[i, k] = tileVal;
                                tileNumber[i, k - 1] = -1;
                                wasMoved = true;
                            }
                            else {
                                k--;
                                break;
                            }
                        }
                    }
                }
            }
            return wasMoved;
        }
        // Abajo derecha
        private bool Move7() {
            int unir, mover, tileVal, k;
            bool wasMoved = false;
            for (int i = 3; i >= 0; i--) {
                for(int j = 3; j >= 0; j--) {
                    if(tileNumber[i,j] != -1) {
                        if (i <= j) {
                            unir = i;
                            mover = 3 - j;
                        }
                        else {
                            unir = j;
                            mover = 3 - i;
                        }
                        for (k = 1; k <= unir; k++) {
                            if (tileNumber[i - k, j - k] != -1) {
                                if (tileNumber[i, j] == tileNumber[i - k, j - k]) {
                                    tileNumber[i, j]++;
                                    score += tileNumber[i, j];
                                    tileNumber[i - k, j - k] = -1;
                                    wasMoved = true;
                                }
                                k = 5;
                            }
                        }
                        tileVal = tileNumber[i, j];
                        for(k = 1; k <= mover; k++) {
                            if(tileNumber[i+k, j + k] == -1) {
                                tileNumber[i + k, j + k] = tileVal;
                                tileNumber[i + k - 1, j + k - 1] = -1;
                                wasMoved = true;
                            }
                            else {
                                break;
                            }
                        }
                    }
                }
            }
            return wasMoved;
        }
        private void EqualTileNumbers(ref int[,] arr1, ref int[,] arr2)
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    arr1[i, j] = arr2[i, j];
                }
            }
        }
    }
}
