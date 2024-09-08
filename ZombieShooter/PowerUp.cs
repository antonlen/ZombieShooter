using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieShooter
{
    //public enum PowerUpTypes
    //{
    //    TripleShot,
    //    Nuke
    //}
    internal class PowerUp : Sprite
    {
        int XSpeed = 0;
        int YSpeed = 0;
        //public PowerUpTypes CurrentPowerUp;

        public override Vector2 ScaledSize { get => new Vector2(Texture.Width * Scale.X, Texture.Height * Scale.Y); }

        public PowerUp(Texture2D texture, Vector2 origin, Vector2 position, Vector2 scale, Color color, SpriteEffects spriteEffects, float rotation, float layerDepth, int xSpeed, int ySpeed)// PowerUpTypes currentPowerUp)
            : base(texture, origin, position, scale, color, spriteEffects, rotation, layerDepth)
        {
            XSpeed = xSpeed;
            YSpeed = ySpeed;
            //CurrentPowerUp = currentPowerUp;
        }

        public void Update()
        {
            Position = new Vector2(Position.X + XSpeed, Position.Y + YSpeed);
        }
    }
}
