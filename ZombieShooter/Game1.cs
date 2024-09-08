using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace ZombieShooter
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D gameSpriteTexture;
        Texture2D mapTexture;
        Texture2D treeTexture;
        Texture2D powerUpTextureTriple;
        Texture2D powerUpTextureNuke;
        Texture2D explosionTexture;
        SpriteFont spriteFont;
        SpriteFont waveFont;
        Dictionary<ZombieAnimationTypes, FrameHelper> zombieFrames;
        //   bool attacking = false;
        Player player;
        //PowerUp powerUp;

        //Zombie zombie; 

        List<Zombie> zombies = new List<Zombie>();
        List<PowerUp> tripleShotPowerUps = new List<PowerUp>();
        List<NukePowerUp> nukePowerUps = new List<NukePowerUp>();

        TimeSpan spawnTimer;
        TimeSpan timeTracker;
        TimeSpan textOnScreenTime;
        TimeSpan powerUpTime;
        int minSpawnTime = 200;
        TimeSpan incHealthSpawnTime;
        TimeSpan explosionTimer;
        TimeSpan fadeOutTimer;
        TimeSpan damageTimer;
        int zombieHealthMax = 0;
        int zombieWave = 1;
        int counter = 0;

        int spawnTime = 5000;

        int powerUpSpawnChanceTriple = 4;
        int powerUpSpawnChanceNuke = 1;
        Color grayScale = Color.White;
        bool hasIntersected = false;
        bool hasAttacked = false;

        //  Bullet bullet;
        KeyboardState keyboardState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Dictionary<AnimationTypes, FrameHelper> frames = new Dictionary<AnimationTypes, FrameHelper>();
            frames.Add(AnimationTypes.Idle, new FrameHelper(new Rectangle[] { new Rectangle(8, 5, 32, 37) }, -1));
            frames.Add(AnimationTypes.Walk, new FrameHelper(new Rectangle[] { new Rectangle(8, 5, 32, 37), new Rectangle(56, 5, 31, 37), new Rectangle(104, 5, 31, 38), new Rectangle(152, 5, 31, 37), new Rectangle(8, 53, 32, 37), new Rectangle(57, 53, 32, 36), new Rectangle(105, 53, 31, 35), new Rectangle(153, 53, 32, 36) }, -1));

            Dictionary<BulletAnimationTypes, FrameHelper> bulletFrames = new Dictionary<BulletAnimationTypes, FrameHelper>();
            bulletFrames.Add(BulletAnimationTypes.Shoot, new FrameHelper(new Rectangle[] { new Rectangle(9, 100, 6, 16) }, -1));
            bulletFrames.Add(BulletAnimationTypes.Shothit, new FrameHelper(new Rectangle[] { new Rectangle(8, 128, 8, 8), new Rectangle(30, 126, 12, 12), new Rectangle(52, 124, 18, 18) }, 0));

            zombieFrames = new Dictionary<ZombieAnimationTypes, FrameHelper>();
            zombieFrames.Add(ZombieAnimationTypes.Idle, new FrameHelper(new Rectangle[] { new Rectangle(3, 343, 42, 32), new Rectangle(51, 342, 42, 31) }, -1));
            zombieFrames.Add(ZombieAnimationTypes.Walk, new FrameHelper(new Rectangle[] { new Rectangle(3, 154, 42, 29), new Rectangle(50, 151, 42, 38), new Rectangle(97, 148, 43, 42), new Rectangle(146, 151, 42, 38), new Rectangle(3, 202, 42, 29), new Rectangle(52, 199, 42, 38), new Rectangle(100, 196, 43, 42), new Rectangle(148, 199, 42, 38) }, -1));
            zombieFrames.Add(ZombieAnimationTypes.Attack, new FrameHelper(new Rectangle[] { new Rectangle(51, 297, 43, 26), new Rectangle(1, 297, 44, 29), new Rectangle(102, 296, 41, 40), new Rectangle(152, 296, 40, 36), new Rectangle(197, 297, 42, 35) }, 2));

            gameSpriteTexture = Content.Load<Texture2D>("zombieShooterSpriteSheet");
            mapTexture = Content.Load<Texture2D>("groundZombieShooter");
            treeTexture = Content.Load<Texture2D>("deadTree2");
            spriteFont = Content.Load<SpriteFont>("font");
            waveFont = Content.Load<SpriteFont>("waveFont");
            powerUpTextureTriple = Content.Load<Texture2D>("tripleShotPowerup");
            powerUpTextureNuke = Content.Load<Texture2D>("nukePowerUp");
            explosionTexture = Content.Load<Texture2D>("explosionZombieShooter");

            incHealthSpawnTime= new TimeSpan(0, 0, 0, 0, 55000);

            player = new Player(gameSpriteTexture, new Vector2(16, 11), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Vector2.One, Color.White, SpriteEffects.None,
                0, 0, frames, AnimationTypes.Idle, new Vector2(3, 3), Keys.A, Keys.D, Keys.W, Keys.S, Keys.J, bulletFrames, 10, 5);
            //powerUp = new PowerUp(powerUpTexture, new Vector2(16, 11), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Vector2.One, Color.White, SpriteEffects.None, 0, 0, 0, 2);

            //zombie = new Zombie(gameSpriteTexture, new Vector2(21, 29 / 2), new Vector2(100, 100), Vector2.One, Color.White, SpriteEffects.None, 0, 0, zombieFrames, ZombieAnimationTypes.Idle, new Vector2((float)1.5, (float)1.5));

            //  bullet = new Bullet(gameSpriteTexture, Vector2.Zero, new Vector2(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2), Vector2.One, Color.White, SpriteEffects.None,
            //0, 0, bulletFrames, BulletAnimationTypes.Shoot, new Vector2(0,0), Keys.J);

            _spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            Random ran = new Random();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime, GraphicsDevice);

            for (int i = 0; i < tripleShotPowerUps.Count; i++)
            {
                if (player.Hitbox.Intersects(tripleShotPowerUps[i].Hitbox))
                {
                    player.tripleShotPowerUpActive = true;
                    powerUpTime = TimeSpan.Zero;
                    tripleShotPowerUps.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < nukePowerUps.Count; i++)
            {
                explosionTimer = TimeSpan.Zero;

                if (player.Hitbox.Intersects(nukePowerUps[i].Hitbox))
                {
                    hasIntersected = true;
                    //for (int j = 0; j < zombies.Count; j++)
                    //{
                    //zombies[j].Health = 0;
                    zombies.Clear();
                    nukePowerUps.RemoveAt(i);
                    i--;
                }
            }


            if (hasIntersected == true)
            {
                explosionTimer += gameTime.ElapsedGameTime;
                fadeOutTimer += gameTime.ElapsedGameTime;
            }

            if (player.tripleShotPowerUpActive == true)
            {
                powerUpTime += gameTime.ElapsedGameTime;
                if (powerUpTime.TotalMilliseconds >= 5000)
                {
                    player.tripleShotPowerUpActive = false;
                    powerUpTime = TimeSpan.Zero;
                }
            }




            for (int i = 0; i < zombies.Count; i++)
            {
                zombies[i].Update(gameTime);
                zombies[i].follow(player.Position, gameTime);
            }

            for (int i = 0; i < player.bullets.Count; i++)
            {
                if (player.bullets[i].Position.X > GraphicsDevice.Viewport.Width || player.bullets[i].Position.X < 0 || player.bullets[i].Position.Y > GraphicsDevice.Viewport.Height || player.bullets[i].Position.Y < 0 || player.bullets[i].currentIndex == 2)
                {
                    player.bullets.RemoveAt(i);
                    i--;
                }
                else
                {
                    for (int j = 0; j < zombies.Count; j++)
                    {


                        if (player.bullets[i].Hitbox.Intersects(zombies[j].Hitbox))
                        {

                            if (player.bullets[i].currentBulletAnimation != BulletAnimationTypes.Shothit)
                            {
                                zombies[j].Health -= 1;
                                player.bullets[i].Speed = new Vector2(0, 0);
                                player.bullets[i].currentIndex = 0;
                            }
                            player.bullets[i].currentBulletAnimation = BulletAnimationTypes.Shothit;

                            if (zombies[j].Health <= 0)
                            {
                                if (powerUpSpawnChanceTriple == ran.Next(0, 45))
                                {
                                    tripleShotPowerUps.Add(new PowerUp(powerUpTextureTriple, Vector2.Zero, new Vector2(zombies[j].Position.X, zombies[j].Position.Y), Vector2.One, Color.White, SpriteEffects.None, 0, 0, 0, 0));

                                }

                                if (powerUpSpawnChanceNuke == ran.Next(0, 100))
                                {
                                    nukePowerUps.Add(new NukePowerUp(powerUpTextureNuke, Vector2.Zero, new Vector2(zombies[j].Position.X, zombies[j].Position.Y), new Vector2(0.1f, 0.1f), Color.White, SpriteEffects.None, 0, 0, 0, 0));
                                }

                                //if (powerUpSpawnChanceNuke == ran.Next(0, 2))
                                //{
                                //    powerUps.Add(new PowerUp(powerUpTextureNuke, Vector2.Zero, new Vector2(zombies[j].Position.X, zombies[j].Position.Y), Vector2.One, Color.White, SpriteEffects.None, 0, 0, 0, 0));
                                //}

                                zombies.RemoveAt(j);
                                j--;


                            }
                            if (i < 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < zombies.Count; i++)
            {
                if (zombies[i].Health == 2)
                {
                    zombies[i].Color = Color.Yellow;
                }

                if (zombies[i].Health == 3)
                {
                    zombies[i].Color = Color.Orange;
                }

                if (zombies[i].Health == 4)
                {
                    zombies[i].Color = Color.Red;
                }

                if (zombies[i].Health == 5)
                {
                    zombies[i].Color = Color.Purple;
                }

                if (zombies[i].Health == 6)
                {
                    zombies[i].Color = Color.Blue;
                }

                if (zombies[i].Health == 7)
                {
                    zombies[i].Color = Color.Black;
                }

                if (zombies[i].currentZombieAnimation == ZombieAnimationTypes.Attack && zombies[i].currentIndex == zombies[i].currentFrames.VIPFrame)
                {
                    if (zombies[i].attacking == false)
                    {
                        player.Health--;

                        zombies[i].attacking = true;
                        hasAttacked = true;
                        damageTimer = gameTime.TotalGameTime;
                    }
                }
                else
                {
                    zombies[i].attacking = false;
                }
            }

            spawnTimer += gameTime.ElapsedGameTime;
            timeTracker += gameTime.ElapsedGameTime;
            incHealthSpawnTime += gameTime.ElapsedGameTime;

            if (timeTracker.TotalMilliseconds >= 5000)
            {
                spawnTime -= 500;
                spawnTime = Math.Max(spawnTime, minSpawnTime);
                timeTracker = TimeSpan.Zero;
            }

            if (incHealthSpawnTime.TotalMilliseconds >= 55000)
            {
                textOnScreenTime += gameTime.ElapsedGameTime;
            }

            if (incHealthSpawnTime.TotalMilliseconds >= 60000 && counter >= 1)
            {
                zombieHealthMax++;
               // zombieWave++;
                textOnScreenTime = TimeSpan.Zero;
                incHealthSpawnTime = TimeSpan.Zero;
            }

            if (spawnTimer.TotalMilliseconds > spawnTime)
            {
                double angle = ran.NextDouble() * (2 * Math.PI);
                double angleX = Math.Cos(angle) * 500 + GraphicsDevice.Viewport.Width / 2;
                double angleY = Math.Sin(angle) * 500 + GraphicsDevice.Viewport.Height / 2;


                zombies.Add(new Zombie(gameSpriteTexture, new Vector2(21, 29 / 2), new Vector2((float)angleX, (float)angleY), Vector2.One, Color.White, SpriteEffects.None, 0, 0, zombieFrames, ZombieAnimationTypes.Idle, new Vector2((float)1.5, (float)1.5), ran.Next(1, zombieHealthMax + 1), player));


                spawnTimer = TimeSpan.Zero;

            }

            if (player.Health <= 0)
            {
                player.Speed = new Vector2(0, 0);
                for (int i = 0; i < zombies.Count; i++)
                {
                    zombies.RemoveAt(i);
                }

                for (int i = 0; i < tripleShotPowerUps.Count; i++)
                {
                    tripleShotPowerUps.RemoveAt(i);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    player.Health = 5;
                    zombieHealthMax = 1;
                    spawnTime = 5000;
                    timeTracker = TimeSpan.Zero;
                    incHealthSpawnTime = TimeSpan.Zero;
                    powerUpTime = TimeSpan.Zero;
                }
            }

            base.Update(gameTime);


        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(mapTexture, new Rectangle(0, 0, mapTexture.Width, mapTexture.Height), Color.White);
            player.Draw(_spriteBatch);
            _spriteBatch.DrawString(spriteFont, $"health : {player.Health}", new Vector2(GraphicsDevice.Viewport.Width - 80, 10), Color.White);
            //_spriteBatch.Draw(mapTexture, player.Hitbox, Color.OrangeRed);

            if (player.Health <= 0)
            {
                _spriteBatch.DrawString(spriteFont, "you lost | press R to restart", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) - spriteFont.MeasureString("you lost") / 2, Color.White);
            }

            Window.Title = fadeOutTimer.TotalMilliseconds.ToString();

            if (incHealthSpawnTime.TotalMilliseconds > 55000)
            {
                if (textOnScreenTime.TotalMilliseconds < 5000)
                {
                    _spriteBatch.DrawString(waveFont, $"WAVE : {zombieHealthMax + 1}", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) - waveFont.MeasureString($"WAVE : {zombieHealthMax + 1}") / 2, Color.White);

                }
                textOnScreenTime = TimeSpan.Zero;
            }

            if(incHealthSpawnTime.TotalMilliseconds > 60000)
            {
                counter++;
            }


            for (int i = 0; i < tripleShotPowerUps.Count; i++)
            {
                tripleShotPowerUps[i].Draw(_spriteBatch);
                //_spriteBatch.Draw(powerUpTexture, powerUps[i].Hitbox, Color.OrangeRed);
            }

            for (int i = 0; i < nukePowerUps.Count; i++)
            {
                nukePowerUps[i].Draw(_spriteBatch);
                //_spriteBatch.Draw(powerUpTextureNuke, nukePowerUps[i].Hitbox, Color.OrangeRed);
            }

            for (int i = 0; i < zombies.Count; i++)
            {
                zombies[i].Draw(_spriteBatch);

                if (zombies[i].Health == 2)
                {
                    zombies[i].Color = Color.Yellow;
                }

                if (zombies[i].Health == 3)
                {
                    zombies[i].Color = Color.Orange;
                }

                if (zombies[i].Health == 4)
                {
                    zombies[i].Color = Color.Red;
                }

                if ((gameTime.TotalGameTime - damageTimer).TotalMilliseconds < 1000)
                {
                    player.Color = Color.Red;
                }

                else
                {
                    hasAttacked = false;
                    player.Color = Color.White;
                    damageTimer = TimeSpan.Zero;
                }
            }


            for (int i = 0; i < nukePowerUps.Count; i++)
            {
                if (player.Hitbox.Intersects(nukePowerUps[i].Hitbox))
                {

                }
            }
            if (hasIntersected == true)
            {
                if (fadeOutTimer.TotalMilliseconds <= 2550)
                {
                    //grayScale.A = (byte)(255 - (fadeOutTimer.TotalMilliseconds / 10));
                    //grayScale.R = (byte)(255 - (fadeOutTimer.TotalMilliseconds / 10));
                    //grayScale.G = (byte)(255 - (fadeOutTimer.TotalMilliseconds / 10));
                    //grayScale.B = (byte)(255 - (fadeOutTimer.TotalMilliseconds / 10));
                    grayScale = Color.Lerp(Color.White, new Color(0, 0, 0, 0), (float)fadeOutTimer.TotalMilliseconds / 2550);
                    _spriteBatch.Draw(explosionTexture, new Rectangle(GraphicsDevice.Viewport.Width / 2 - explosionTexture.Width / 2, GraphicsDevice.Viewport.Height / 2 - explosionTexture.Height / 2, explosionTexture.Width, explosionTexture.Height), grayScale);

                }
                else
                {
                    hasIntersected = false;
                    fadeOutTimer = TimeSpan.Zero;
                    explosionTimer = TimeSpan.Zero;
                }

            }



            for (int i = 0; i < player.bullets.Count; i++)
            {
                //_spriteBatch.Draw(mapTexture, player.bullets[i].Hitbox, Color.OrangeRed);
            }


            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}