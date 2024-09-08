using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace ZombieShooter
{
    public class Sprite 
    {
        public Texture2D Texture { get; set; }
        protected Vector2 origin;
        public virtual Vector2 Origin { get => origin; set => origin = value; }
        public Vector2 Position;// { get; set; }
        public Vector2 Scale { get; set; }
        public Color Color { get; set; }
        public SpriteEffects Effects { get; set; }
        public float Rotation { get; set; }

        public float LayerDepth { get; set; }

        public virtual Rectangle SourceRectangle { get;}

         public virtual Rectangle Hitbox
        {
            get
            {
                if(Effects == SpriteEffects.None)
                {
                    return new Rectangle((int)(Position.X - origin.X), (int)(Position.Y - origin.Y), (int)ScaledSize.X, (int)ScaledSize.Y);
                }
                
                else
                {
                    return new Rectangle((int)(Position.X + origin.X - ScaledSize.X), (int)(Position.Y - origin.Y), (int)ScaledSize.X, (int)ScaledSize.Y);
                }
            }
        }
        public virtual Rectangle BoundingBox
        {
            get
            {
                int x, y, w, h;
                if (Rotation != 0)
                {
                    var cos = Math.Abs(Math.Cos(Rotation));
                    var sin = Math.Abs(Math.Sin(Rotation));
                    var t1_opp = (int)ScaledSize.X * cos;
                    var t1_adj = Math.Sqrt(Math.Pow((int)ScaledSize.X, 2) - Math.Pow(t1_opp, 2));
                    var t2_opp = (int)ScaledSize.Y * sin;
                    var t2_adj = Math.Sqrt(Math.Pow((int)ScaledSize.Y, 2) - Math.Pow(t2_opp, 2));

                    w = (int)(t1_opp + t2_opp);
                    h = (int)(t1_adj + t2_adj);
                    //x = (int)(Position.X - (w/2));
                    //y = (int)(Position.Y - (h/2));
                }
                else
                {  
                    w = (int)ScaledSize.X;
                    h = (int)ScaledSize.Y;
                }

                x = (int)(Position.X - (w / 2)); //- Origin.X / 2);
                y = (int)(Position.Y - (h / 2)); // - Origin.Y / 2);

                return new Rectangle(x, y, w, h);
            }
        }


        public virtual Vector2 ScaledSize
        {
            get
            {
                return new Vector2(SourceRectangle.Width * Scale.X, SourceRectangle.Height * Scale.Y);
            }
        }

        public Sprite(Texture2D texture, Vector2 origin, Vector2 position, Vector2 scale, Color color, SpriteEffects effects, float rotation, float layerDepth)
        {
            Texture = texture;
            Origin = origin;
            Position = position;
            Scale = scale;
            Color = color;
            Effects = effects;
            Rotation = rotation;
            LayerDepth = layerDepth;
        }



        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color, Rotation, Origin, Scale, Effects, LayerDepth);
        }

    }
}
