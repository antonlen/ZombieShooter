using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieShooter
{
    public enum ZombieAnimationTypes
    {
        Idle,
        Walk,
        Attack
    }

    public enum ZombieDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    internal class Zombie : AnimatingSprite
    {
        public Vector2 Speed;
        public int Health;
        public bool attacking;
        public ZombieAnimationTypes currentZombieAnimation;
        public ZombieDirection currentDirection;
        bool moveHoriz;
        bool moveVert;
        TimeSpan timeSpan;
        public Player Player;
        Dictionary<ZombieAnimationTypes, FrameHelper> ZombieFrames;
        public override Rectangle Hitbox => BoundingBox;

        public override FrameHelper currentFrames { get => ZombieFrames[currentZombieAnimation]; }
        public Zombie(Texture2D texture, Vector2 origin, Vector2 position, Vector2 scale, Color color, SpriteEffects spriteEffects, float rotation, float layerDepth,
            Dictionary<ZombieAnimationTypes, FrameHelper> zombieFrames, ZombieAnimationTypes chosenFrames, Vector2 speed, int health, Player player)
            : base(texture, origin, position, scale, color, spriteEffects, rotation, layerDepth)
        {
            Health = health;
            currentZombieAnimation = chosenFrames;
            Speed = speed;
            currentDirection = ZombieDirection.None;
            ZombieFrames = zombieFrames;
            Player = player;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Random ran = new Random();


            //Randomize movement

            if(Hitbox.Intersects(Player.Hitbox))
            {
                if(currentZombieAnimation != ZombieAnimationTypes.Attack)
                {
                    currentIndex = 0;
                }
             
                currentZombieAnimation = ZombieAnimationTypes.Attack;
            }

            if (currentZombieAnimation == ZombieAnimationTypes.Idle || currentZombieAnimation == ZombieAnimationTypes.Attack)
            {
                if (currentDirection != ZombieDirection.None)
                {
                    currentIndex = 0;
                    currentZombieAnimation = ZombieAnimationTypes.Walk;
                }
            }



            if (currentZombieAnimation == ZombieAnimationTypes.Walk)
            {

                if (currentDirection == ZombieDirection.Down)
                {
                    Rotation = MathHelper.ToRadians(0);
                    Position.Y += Speed.Y;
                }

                if (currentDirection == ZombieDirection.Up)
                {
                    Rotation = MathHelper.ToRadians(180);
                    Position.Y -= Speed.Y;
                }





                if (currentDirection == ZombieDirection.Right)
                {
                    Rotation = MathHelper.ToRadians(270);
                    Position.X += Speed.X;
                }

                if (currentDirection == ZombieDirection.Left)
                {
                    Rotation = MathHelper.ToRadians(90);

                    Position.X -= Speed.X;
                }





                if (currentDirection == ZombieDirection.None)
                {
                    currentIndex = 0;
                    //currentZombieAnimation = ZombieAnimationTypes.Attack;
                }
            }
        }

        public void follow(Vector2 playerPosition, GameTime gameTime)
        {
            timeSpan += gameTime.ElapsedGameTime;
            Random ran = new Random();
            moveHoriz = false;
            moveVert = false;

            //if (ran.Next(0, 2) == 0)// && timeSpan.TotalMilliseconds >= 500)
            //{
            int temp = -1 ;
            if (timeSpan.TotalMilliseconds >= 500 && moveHoriz == false && moveVert == false)
            {
                temp = ran.Next(0, 2);
                timeSpan = TimeSpan.Zero;
            }

            if (temp == 0)
            {
                if (Position.Y == playerPosition.Y)
                {
                    moveHoriz = true;
                    temp = 0; 
                }
               
                if (playerPosition.X - 20 > Position.X)
                {
                    currentDirection = ZombieDirection.Right;
                }

                else if (playerPosition.X + 20 < Position.X)
                {
                    currentDirection = ZombieDirection.Left;
                }

                else
                {
                    currentDirection = ZombieDirection.None;
                }
            }

            //}

            //else
            //{ 
            if (temp == 1)
            {
                if(Position.X == playerPosition.X)
                {
                    moveVert = true;
                    temp = 1;
                }
                if (playerPosition.Y + 20 < Position.Y)
                {
                    currentDirection = ZombieDirection.Up;
                }

                else if (playerPosition.Y - 20 > Position.Y)
                {
                    currentDirection = ZombieDirection.Down;
                }

                else
                {
                    currentDirection = ZombieDirection.None;
                }
            }
            //}


        }

    }
}
