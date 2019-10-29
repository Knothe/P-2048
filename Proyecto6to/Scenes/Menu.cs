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
    class Menu : Scene
    {

        private Texture2D backGround;
        private Texture2D title;
        private Texture2D notLoad;
        private Button load;
        private Button start;
        private bool hasLoad;

        public Menu(bool hasFile)
        {
            start = new Button(new Vector2(480, 384), new Vector2(.5f, .5f));
            load = new Button(new Vector2(480, 565), new Vector2(.5f, .5f));
            hasLoad = hasFile;
        }

        public override void Init()
        {
            
        }

        public override void Load(Game game)
        {
            backGround = game.Content.Load<Texture2D>("Background");
            title = game.Content.Load<Texture2D>("MainTitle");
            if (hasLoad)
                load.Load(game, "Load", "Load2", "Load3");
            else
                notLoad = game.Content.Load<Texture2D>("LoadGRay");
            start.Load(game, "Start", "Start2", "Start3");
        }
        public override int Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            if (hasLoad) {
                if (load.Update(mouseState.Position.ToVector2(), mouseState.LeftButton))
                    return 2;
            }
            if(start.Update(mouseState.Position.ToVector2(), mouseState.LeftButton))
                return 1;
            
            return 0;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Vector2(-720, -384), scale: new Vector2(1.5f, 1.5f));
            spriteBatch.Draw(title, new Vector2(240, 30));
            if (hasLoad)
                load.Draw(spriteBatch);
            else
                spriteBatch.Draw(notLoad, new Vector2(480, 565), scale: new Vector2(.5f, .5f));
            start.Draw(spriteBatch);
        }
        public override void Destroy()
        {

        }
    }
}
