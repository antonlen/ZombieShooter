using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieShooter
{
    internal class Obstacles : Sprite
    {
        Vector2 Speed;
        public Obstacles(Texture2D texture, Vector2 origin, Vector2 position, Vector2 scale, Color color, SpriteEffects effects, float rotation, float layerDepth, Vector2 speed) 
            : base(texture, origin, position, scale, color, effects, rotation, layerDepth)
        {
        }
    }
}
