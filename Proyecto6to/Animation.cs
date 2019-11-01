using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Proyecto6to
{
    class Animation
    {
        public Vector2 tile1, tile2, endingTile, movement;
        public int val, t3, val2;
        public Animation()
        {
            tile1 = new Vector2(-1, -1);
            tile2 = new Vector2(-1, -1);
            endingTile = new Vector2(-1, -1);
            movement = new Vector2(-1, -1);
            val = -1;
            t3 = -1;
        }
        public void SetMovement()
        {
            movement = endingTile - tile1;
            if (movement == new Vector2(0, 0) && t3 != -1)
                movement = endingTile - tile2;
            movement.Normalize();
            movement *= .2f;
        }
        public bool MoveTile1()
        {
            bool wasMoved = false;
            if (tile1 != endingTile)
            {
                tile1 += movement;
                tile1.X = (float)Math.Round(tile1.X, 1, MidpointRounding.ToEven);
                tile1.Y = (float)Math.Round(tile1.Y, 1, MidpointRounding.ToEven);
                wasMoved = true;
            }
            
            return wasMoved;
        }
        public bool MoveTile2()
        {
            bool wasMoved = false;
            if (tile2 != endingTile)
            {
                val2 = val + 1;
                tile2 += movement;
                tile2.X = (float)Math.Round(tile2.X, 1, MidpointRounding.ToEven);
                tile2.Y = (float)Math.Round(tile2.Y, 1, MidpointRounding.ToEven);
                wasMoved = true;
            }
            else
                val = val2;
            return wasMoved;
        }
    }
}

/*
    Animation anim = new Animation();
    anim.tile1 = new Vector2(i, j);
    anim.val = tileNumber[i, j];


    anim.tile2 = new Vector2(i - k, j - k);
    anim.t3 = 1;
     
    anim.endingTile = anim.tile1;
    
    anim.endingTile = new Vector2(i + k, j + k);

    anim.SetMovement();
    animList.Add(anim);

     */
