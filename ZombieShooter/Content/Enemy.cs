using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieShooter.Content
{

    public enum EnemyAnimationTypes
    {
        Idle,
        Walk,
        Attack
    }
    internal class Enemy : AnimatingSprite
    {
        public EnemyAnimationTypes currentBulletAnimation;
        Dictionary<EnemyAnimationTypes, FrameHelper> BulletFrames;
        public override FrameHelper currentFrames { get => BulletFrames[currentBulletAnimation]; }

        public Enemy(Texture2D texture, Vector2 origin, Vector2 position, Vector2 scale, Color color, SpriteEffects spriteEffects, float rotation, float layerDepth) 
            : base(texture, origin, position, scale, color, spriteEffects, rotation, layerDepth)
        {

        }
    }
}
