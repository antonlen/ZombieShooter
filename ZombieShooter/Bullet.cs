using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieShooter
{
    public enum BulletAnimationTypes
    {
        Shoot,
        Shothit
    }
    internal class Bullet : AnimatingSprite
    {
        public Vector2 Speed;
        public BulletAnimationTypes currentBulletAnimation;
        Dictionary<BulletAnimationTypes, FrameHelper> BulletFrames;
        public override Rectangle Hitbox => BoundingBox;
        public override FrameHelper currentFrames { get => BulletFrames[currentBulletAnimation]; }
        public Keys ShootKey;
        public bool bulletState;
        public bool powerUpBullets;

        public Bullet(Texture2D texture, Vector2 origin, Vector2 position, Vector2 scale, Color color, SpriteEffects spriteEffects, float rotation, float layerDepth,
            Dictionary<BulletAnimationTypes, FrameHelper> bulletFrames, BulletAnimationTypes chosenFrames, float speed, bool bulletState)
            : base(texture, origin, position, scale, color, spriteEffects, rotation, layerDepth)
        {
            currentBulletAnimation = chosenFrames;
            //  Speed = speed;
            BulletFrames = bulletFrames;
            //if (Rotation == MathHelper.ToRadians(0))
            //{
            //    Speed = new Vector2(0, speed);
            //}

            //if (Rotation == MathHelper.ToRadians(90))
            //{
            //    Speed = new Vector2(-speed, 0);
            //}

            //if (Rotation == MathHelper.ToRadians(180))
            //{
            //    Speed = new Vector2(0, -speed);
            //}

            //if (Rotation == MathHelper.ToRadians(270))
            //{
            //    Speed = new Vector2(speed, 0);
            //}

            Speed = new Vector2(MathF.Cos(Rotation + MathHelper.ToRadians(90)), MathF.Sin(Rotation + MathHelper.ToRadians(90))) * speed;

            this.bulletState = bulletState;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            base.Update(gameTime);
          
            Origin = SourceRectangle.Size.ToVector2() / 2;
            Position += Speed;


        }

    }
}
