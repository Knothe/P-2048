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
    class Button
    {
        private Texture2D button;
        private Texture2D mouseOverButton;
        private Texture2D clickOnButton;
        private Vector2 size;
        private Vector2 position;
        private Vector2 scale;
        private bool prevState = false;
        enum State
        {
            normal,
            over,
            pressed
        }

        State buttonState = State.normal;

        public Button(Vector2 pos, Vector2 s)
        {
            position = pos;
            scale = s;
        }

        public void Load(Game game, String b, String m, String c)
        {
            button = game.Content.Load<Texture2D>(b);
            mouseOverButton = game.Content.Load<Texture2D>(m);
            clickOnButton = game.Content.Load<Texture2D>(c);
            size.X = button.Width * scale.X;
            size.Y = button.Height * scale.Y;
        }

        public bool Update(Vector2 mousePosition, ButtonState b)
        {
            if(mousePosition.X >= position.X && mousePosition.X <= position.X + size.X &&
               mousePosition.Y >= position.Y && mousePosition.Y <= position.Y + size.Y)
            {
                if(b == ButtonState.Pressed)
                {
                    prevState = true;
                    buttonState = State.pressed;
                    return false;
                }
                else
                {
                    buttonState = State.over;
                    if (prevState)
                    {
                        prevState = false;
                        return true;
                    }
                }
            }
            else
            {
                prevState = false;
                buttonState = State.normal;
            }
            
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = button;
            switch (buttonState)
            {
                case State.over:
                    texture = mouseOverButton;
                    break;
                case State.pressed:
                    texture = clickOnButton;
                    break;
            }

            spriteBatch.Draw(texture, position, scale: scale);
        }

    }
}
