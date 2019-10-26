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
        private Texture2D backGround;
        private Texture2D icon;
        private Texture2D title;
        private Texture2D grid;
        private Texture2D[] tileList = new Texture2D[12];
        private int[,] tileNumber = new int[4, 4];
        private Button back;
        private Button reload;
        private Button save;
        private bool mousePressed = false;
        private Vector2 mousePos;
        private const double radToAngle = 180 / Math.PI;

        public override void Init()
        {
            
        }

        public GameState()
        {
            back = new Button(new Vector2(1136, 598), new Vector2(1, 1));
            reload = new Button(new Vector2(1288, 598), new Vector2(1, 1));
            save = new Button(new Vector2(30, 598), new Vector2(1, 1));
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    tileNumber[i, j] = -1;
                }
            }
            tileNumber[0, 0] = 0;
            tileNumber[0, 1] = 1;
            tileNumber[0, 2] = 0;
            tileNumber[0, 3] = 0;
            tileNumber[1, 0] = 0;
            tileNumber[1, 1] = 0;
            tileNumber[1, 2] = 1;
            tileNumber[1, 3] = 0;
            tileNumber[2, 0] = 0;
            tileNumber[2, 1] = 0;
            tileNumber[2, 2] = 0;
            tileNumber[2, 3] = 1;
            tileNumber[3, 0] = 0;
            tileNumber[3, 1] = 0;
            tileNumber[3, 2] = 0;
            tileNumber[3, 3] = 1;
            mousePos = new Vector2(0, 0);
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
            grid = game.Content.Load<Texture2D>("MainGame");
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
        public override int Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if(back.Update(mouseState.Position.ToVector2(), mouseState.LeftButton))
            {
                mousePressed = false;
                return 1;
            }
            if(reload.Update(mouseState.Position.ToVector2(), mouseState.LeftButton)){
                mousePressed = false;
                return 2;
            }
            if (save.Update(mouseState.Position.ToVector2(), mouseState.LeftButton))
            {
                mousePressed = false;
                return 3;
            }
            if(!mousePressed && mouseState.LeftButton == ButtonState.Pressed) {
                mousePressed = true;
                mousePos = mouseState.Position.ToVector2();
            }else if(mouseState.LeftButton == ButtonState.Released && mousePressed) {
                mousePressed = false;
                CheckMovement(mouseState.Position.ToVector2());
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
            back.Draw(spriteBatch);
            reload.Draw(spriteBatch);
            save.Draw(spriteBatch);
        }
        public override void Destroy()
        {

        }

        private void CheckMovement(Vector2 newMousePos) {
            newMousePos.Y = newMousePos.Y - mousePos.Y;
            newMousePos.X = newMousePos.X - mousePos.X;
            double angle = Math.Atan2(newMousePos.Y, newMousePos.X);
            if(newMousePos.Length() > 0)
            {
                angle *= radToAngle;
                if (angle <= 22.5 && angle >= -22.5) Move0(); //Derecha
                else if (angle <= -22.5 && angle >= -67.5) Move1(); //Arriba derecha
                else if (angle <= -67.5 && angle >= -112.5) Move2(); //Arriba 
                else if (angle <= -112.5 && angle >= -157.5) Move3(); //Arriba izquierda
                else if (angle <= -157.5 || angle >= 157.5) Move4(); //Izquierda
                else if (angle <= 157.5 && angle >= 112.5) Move5(); //Izquierda abajo
                else if (angle <= 112.5 && angle >= 67.5) Move6(); //Abajo
                else Move7(); //Izquierda derecha
            }
            
        }
        // Derecha
        private void Move0() { 
            for (int i = 3; i >= 0; i--) {
                for (int j = 3; j >= 0; j--) {
                    if (tileNumber[j, i] != -1)
                    {
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (tileNumber[k, i] != -1)
                            {
                                if (tileNumber[j, i] == tileNumber[k, i])
                                {
                                    tileNumber[j, i]++;
                                    tileNumber[k, i] = -1;
                                }
                                k = -1;
                            }
                        }
                    }
                }
                for(int j = 2; j >= 0; j--) {
                    if(tileNumber[j, i] != -1) {
                        for (int k = 3; k > j; k--) {
                            if (tileNumber[k, i] == -1) {
                                tileNumber[k, i] = tileNumber[j, i];
                                tileNumber[j, i] = -1;
                            }
                        }
                    }
                }
            }
        }
        // Arriba derecha
        private void Move1() {
        }
        // Arriba
        private void Move2() { 
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 3; j++) {
                    if (tileNumber[i, j] != -1) {
                        for (int k = j + 1; k < 4; k++) {
                            if (tileNumber[i, k] != -1) {
                                if (tileNumber[i, j] == tileNumber[i, k]) {
                                    tileNumber[i, j]++;
                                    tileNumber[i, k] = -1;
                                }
                                k = 5;
                            }
                        }
                    }
                }
                for (int j = 1; j < 4; j++) {
                    if (tileNumber[i, j] != -1) {
                        for (int k = 0; k < j; k++) {
                            if (tileNumber[i, k] == -1) {
                                tileNumber[i, k] = tileNumber[i, j];
                                tileNumber[i, j] = -1;
                            }
                        }
                    }
                }
            }
        }
        // Arriba izquierda
        private void Move3() {
        }
        // Izquierda
        private void Move4() { 
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 3; j++) {
                    if(tileNumber[j,i] != -1) {
                        for(int k = j + 1; k < 4; k++) {
                            if(tileNumber[k, i] != -1) {
                                if (tileNumber[j, i] == tileNumber[k, i]) {
                                    tileNumber[j, i]++;
                                    tileNumber[k, i] = -1;
                                }
                                k = 5;
                            }
                        }
                    }
                }
                for (int j = 1; j < 4; j++)  {
                    if (tileNumber[j, i] != -1)
                    {
                        for (int k = 0; k < j; k++)
                        {
                            if (tileNumber[k, i] == -1)
                            {
                                tileNumber[k, i] = tileNumber[j, i];
                                tileNumber[j, i] = -1;
                            }
                        }
                    }
                }
            }
        }
        // Abajo izquierdo
        private void Move5() {
        }
        // Abajo
        private void Move6() { 
            for (int i = 3; i >= 0; i--) {
                for (int j = 3; j > 0; j--) {
                    if (tileNumber[i, j] != -1) {
                        for (int k = j - 1; k >= 0; k--)  {
                            if (tileNumber[i, k] != -1) {
                                if (tileNumber[i, j] == tileNumber[i, k])
                                {
                                    tileNumber[i, j]++;
                                    tileNumber[i, k] = -1;
                                }
                                k = -1;
                            }
                        }
                    }
                }
                for (int j = 2; j >= 0; j--) {
                    if (tileNumber[i, j] != -1) {
                        for (int k = 3; k > j; k--) {
                            if (tileNumber[i, k] == -1) {
                                tileNumber[i, k] = tileNumber[i, j];
                                tileNumber[i, j] = -1;
                            }
                        }
                    }
                }
            }
        }
        // Abajo derecha
        private void Move7() {
            for(int i = 3; i >= 0; i--)
            {
                for(int j = 3; j >= 0; j--)
                {
                    if(tileNumber[i,j] != -1) {
                        int dif;
                        if (i <= j)
                            dif = i;
                        else
                            dif = j;
                        for (int k = 1; k <= dif; k++) {
                            if (tileNumber[i - k, j - k] != -1) {
                                if (tileNumber[i, j] == tileNumber[i - k, j - k]) {
                                    tileNumber[i, j]++;
                                    tileNumber[i - k, j - k] = -1;
                                }
                                k = 5;
                            }
                        }
                    }
                }
            }
        }

        /*
         0,0    1,0    2,0    3,0
         0,1    1,1    2,1    3,1
         0,2    1,2    2,2    3,2
         0,3    1,3    2,3    3,3
         */
    }
}
