using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Proyecto6to.Scenes
{
    abstract class Scene
    {
        abstract public void Init();
        abstract public void Load(Game game);
        abstract public int Update(GameTime gameTime);
        abstract public void Draw(SpriteBatch spriteBatch);
        abstract public void Destroy();
    }
}
