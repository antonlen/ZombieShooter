using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace ZombieShooter
{
    abstract class AnimatingSprite : Sprite
    {
        TimeSpan timeSpan;

        public abstract FrameHelper currentFrames
        {
            get;
        }

        public override Rectangle SourceRectangle { get => currentFrames.Frames[currentIndex]; }
        public int currentIndex { get; set; }

        public AnimatingSprite(Texture2D texture, Vector2 origin, Vector2 position, Vector2 scale, Color color, SpriteEffects spriteEffects, float rotation, float layerDepth)
            : base(texture, origin, position, scale, color, spriteEffects, rotation, layerDepth)
        {

            currentIndex = 0;

            timeSpan = TimeSpan.Zero;
            //CurrentFrames = currentFrames;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, Effects, LayerDepth);
        }

        public virtual void Update(GameTime gameTime)
        {


            timeSpan += gameTime.ElapsedGameTime;


            if (timeSpan.TotalMilliseconds > 100)
            {

                currentIndex++;
                timeSpan = TimeSpan.Zero;
                if (currentIndex == currentFrames.Frames.Length)
                {
                    currentIndex = 0;
                  //  EndOfAnimation();
                }
                //EndOfFrame();
            }


        }

        //protected abstract void EndOfFrame();

        //protected abstract void EndOfAnimation();
    }
}

