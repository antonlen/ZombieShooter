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
    public enum AnimationTypes
    {
        Idle,
        Walk,
        Shoot,
        //ShotHit
    }
    class Player : AnimatingSprite
    {
        public AnimationTypes currentAnimation;
        public Vector2 Speed;
        public int Health { get; set; }
        public Keys LeftKey;
        public Keys RightKey;
        public Keys UpKey;
        public Keys DownKey;
        public Keys ShootKey;
        public bool tripleShotPowerUpActive;
        //public bool nukePowerUpActive;

        //public Keys ShootKey;
        Dictionary<AnimationTypes, FrameHelper> Frames;
        public List<Bullet> bullets = new List<Bullet>();
        Dictionary<BulletAnimationTypes, FrameHelper> BulletFrames;
        public override Rectangle Hitbox => BoundingBox;
        float BulletSpeed;
        TimeSpan bulletCooldown;

        public override FrameHelper currentFrames { get => Frames[currentAnimation]; }
        public Player(Texture2D texture, Vector2 origin, Vector2 position, Vector2 scale, Color color, SpriteEffects spriteEffects, float rotation, float layerDepth,
            Dictionary<AnimationTypes, FrameHelper> frames, AnimationTypes chosenFrames, Vector2 speed, Keys leftKey,
                         Keys rightKey, Keys upKey, Keys downKey, Keys shootKey, Dictionary<BulletAnimationTypes, FrameHelper> bulletFrames, float bulletSpeed, int health)
            : base(texture, origin, position, scale, color, spriteEffects, rotation, layerDepth)
        {
            Speed = speed;
            Health = health;
            currentAnimation = chosenFrames;
            LeftKey = leftKey;
            RightKey = rightKey;
            UpKey = upKey;
            DownKey = downKey;
            //ShootKey = shootKey;
            Frames = frames;
            ShootKey = shootKey;
            BulletFrames = bulletFrames;
            BulletSpeed = bulletSpeed;
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {

            int count = 0;
            KeyboardState keyboardState = Keyboard.GetState();
            base.Update(gameTime);
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gameTime);
            }

            if (keyboardState.IsKeyDown(LeftKey) && Position.X > 0)
            {
                currentAnimation = AnimationTypes.Walk;
                Position.X += Speed.X;
                Position.Y += Speed.Y;
                Speed.X = -3;
                Speed.Y = 0;
                Rotation = MathHelper.ToRadians(90);
            }

            else if (keyboardState.IsKeyDown(RightKey) && Position.X < graphicsDevice.Viewport.Width)
            {

                currentAnimation = AnimationTypes.Walk;
                Position.X += Speed.X;
                Position.Y += Speed.Y;
                Speed.X = 3;
                Speed.Y = 0;
                Rotation = MathHelper.ToRadians(270);
            }

            else if (keyboardState.IsKeyDown(UpKey) && Position.Y > 0)
            {
                currentAnimation = AnimationTypes.Walk;
                Position.X += Speed.X;
                Position.Y += Speed.Y;
                Speed.X = 0;
                Speed.Y = -3;
                Rotation = MathHelper.ToRadians(180);
            }

            else if (keyboardState.IsKeyDown(DownKey) && Position.Y < graphicsDevice.Viewport.Height)
            {
                currentAnimation = AnimationTypes.Walk;
                Position.X += Speed.X;
                Position.Y += Speed.Y;
                Speed.X = 0;
                Speed.Y = 3;
                Rotation = MathHelper.ToRadians(0);
            }



            else
            {
                currentIndex = 0;
                currentAnimation = AnimationTypes.Idle;
            }

            if (keyboardState.IsKeyDown(ShootKey))
            {
                Vector2 leftBulletPos, rightBulletPos = leftBulletPos = Position;
 

                bulletCooldown += gameTime.ElapsedGameTime;

                if (bulletCooldown.TotalSeconds >= 0.1)
                {
                    bullets.Add(new Bullet(Texture, Origin, Position, Scale, Color, SpriteEffects.None, Rotation, 0, BulletFrames, BulletAnimationTypes.Shoot, BulletSpeed, false));
                    bulletCooldown = TimeSpan.Zero;
                    if (tripleShotPowerUpActive == true)
                    {
                        int rotationNumber1 = 45;
                      
                      
                        if (Rotation == 0)
                        {
                            leftBulletPos.X += 5;
                            rightBulletPos.X -= 5;
                            
                            
                        }

                        if (Rotation == 90)
                        {
                            leftBulletPos.Y += 5;
                            rightBulletPos.Y -= 5;
                        }

                        if (Rotation == 180)
                        {
                            leftBulletPos.X -= 5;
                            rightBulletPos.Y += 5;
                        }

                        if (Rotation == 270)
                        {
                            leftBulletPos.Y -= 5;
                            rightBulletPos.Y += 5;

                        }
                        bullets.Add(new Bullet(Texture, Origin, new Vector2(leftBulletPos.X, leftBulletPos.Y), Scale, Color, SpriteEffects.None, Rotation + MathHelper.ToRadians(rotationNumber1), 0, BulletFrames, BulletAnimationTypes.Shoot, BulletSpeed, false));
                        bullets.Add(new Bullet(Texture, Origin, new Vector2(rightBulletPos.X, rightBulletPos.Y), Scale, Color, SpriteEffects.None, Rotation - MathHelper.ToRadians(rotationNumber1), 0, BulletFrames, BulletAnimationTypes.Shoot, BulletSpeed, false));

                        //bullets.Add(new Bullet(Texture, Origin, new Vector2(Position.X, Position.Y + 20), Scale, Color, SpriteEffects.None, Rotation, 0, BulletFrames, BulletAnimationTypes.Shoot, BulletSpeed, false));
                        //bullets.Add(new Bullet(Texture, Origin, new Vector2(Position.X, Position.Y - 20), Scale, Color, SpriteEffects.None, Rotation, 0, BulletFrames, BulletAnimationTypes.Shoot, BulletSpeed, false));
                    }

             
                }

            }

            //if (keyboardState.IsKeyDown(ShootKey))
            //{
            //    currentIndex = 0;
            //    currentAnimation = AnimationTypes.Shoot;
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }
        }
        //protected override void EndOfFrame() { }

        //public override void EndOfAnimation() { }
    }
}
