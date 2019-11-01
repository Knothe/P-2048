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
        
        private Button back;
        private Button reload;
        private Button save;
        private bool mousePressed = false;
        private Vector2 mousePos;
        private const double radToAngle = 180 / Math.PI;
        private Board board;
        public GameState()
        {
            board = new Board();
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
            board.Load(game);
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
            
        }
        public override int Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (!board.GetLose())
            {
                if (save.Update(mouseState.Position.ToVector2(), mouseState.LeftButton))
                {
                    mousePressed = false;
                    board.SaveGame();
                }
                if (back.Update(mouseState.Position.ToVector2(), mouseState.LeftButton))
                {
                    mousePressed = false;
                    board.Back();
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
                board.SetNewGame();
            }
            
            
            return 0;
        }
        public override void Draw(SpriteBatch spriteBatch) { 
            spriteBatch.Draw(backGround, new Vector2(0, 0), scale: new Vector2(.75f, .75f));
            spriteBatch.Draw(grid, new Vector2(336, 0));
            board.Draw(spriteBatch);
            spriteBatch.Draw(title, new Vector2(25, 75), scale: new Vector2(.3f, .3f));
            spriteBatch.DrawString(font, "High\nScore", new Vector2(1136, 50), Color.White);
            spriteBatch.DrawString(smallFont, "" + board.GetHighScore(), new Vector2(1136, 160), Color.White);
            spriteBatch.DrawString(font, "\nScore", new Vector2(1288, 50), Color.White);
            spriteBatch.DrawString(smallFont, "" + board.GetScore(), new Vector2(1288, 160), Color.White);
            back.Draw(spriteBatch);
            save.Draw(spriteBatch);
            if (board.GetLose())
            {
                spriteBatch.Draw(gameOver2, new Vector2(0, 0), scale: new Vector2(2,2));
                spriteBatch.Draw(gameOver, new Vector2(240, 128));
            }
            reload.Draw(spriteBatch);
        }
        public override void Destroy()
        {

        }
        private void CheckMovement(Vector2 newMousePos) {
            int[,] prevTileProt = new int[4, 4];
            newMousePos.Y = newMousePos.Y - mousePos.Y;
            newMousePos.X = newMousePos.X - mousePos.X;
            double angle = Math.Atan2(newMousePos.Y, newMousePos.X);
            if(newMousePos.Length() > 0)
            {
                angle *= radToAngle;
                if (angle <= 22.5 && angle >= -22.5)            board.MakeMovement(0); //Derecha
                else if (angle <= -22.5 && angle >= -67.5)      board.MakeMovement(1); //Arriba derecha
                else if (angle <= -67.5 && angle >= -112.5)     board.MakeMovement(2); //Arriba 
                else if (angle <= -112.5 && angle >= -157.5)    board.MakeMovement(3); //Arriba izquierda
                else if (angle <= -157.5 || angle >= 157.5)     board.MakeMovement(4); //Izquierda
                else if (angle <= 157.5 && angle >= 112.5)      board.MakeMovement(5); //Izquierda abajo
                else if (angle <= 112.5 && angle >= 67.5)       board.MakeMovement(6); //Abajo
                else                                            board.MakeMovement(7); //Izquierda derecha
            }
        }
        public void StartGame(int op)
        {
            switch (op)
            {
                case 0:
                    board.SetNewGame();
                    break;
                case 1:
                    board.OpenSavedGame();
                    break;
            }
        }
    }
}
