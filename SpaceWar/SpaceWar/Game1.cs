using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ExplodingTeddies;
namespace SpaceWar
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Initializer
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Explosion> aliens_to_explode = new List<Explosion>();
        List<Alien> AliensToExplode = new List<Alien>(); 
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;
        const int ALIEN_WIDTH = 75;
        const int ALIEN_HEIGHT = 50; 
        Spaceship spaceship;
        List<Alien> aliens = new List<Alien>();
        int ALIEN_SPAWN_TIME = 400;
        int TIME_GONE = 0;
        int SHOOT_TIME = 0;
        const int timeToShoot = 3000;
        Alien alienThatWillGetHit;
        Texture2D bullet;
        Texture2D alien;
        Texture2D alienBullet; 
        Random rand = new Random();
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
     
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

        }
        #endregion
        
        #region Content Loader and Unloader
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spaceship = new Spaceship(Content, WINDOW_WIDTH, WINDOW_HEIGHT);
            bullet = Content.Load<Texture2D>("bullet");
            alienBullet = Content.Load<Texture2D>("alienBullet");
            alien = Content.Load<Texture2D>("alien");
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion

        #region Game Loop
        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyboard = Keyboard.GetState();
            // TODO: Add your update logic here
            spaceship.Update(gameTime, keyboard, WINDOW_WIDTH, WINDOW_HEIGHT);
            foreach (Alien alien in aliens)
            {
                if ((spaceship.X + 50 > alien.X) && (spaceship.X < alien.X + ALIEN_WIDTH))
                {
                    alienThatWillGetHit = alien;
                }
            }
            spaceship.UpdateBullet(gameTime, AliensToExplode, alienThatWillGetHit, Content);
            TIME_GONE += gameTime.ElapsedGameTime.Milliseconds;
            if (TIME_GONE > ALIEN_SPAWN_TIME)
            {
                aliens.Add(new Alien(alien, getRandomLocation(), ALIEN_WIDTH, ALIEN_HEIGHT));
                TIME_GONE = 0;
            }

            foreach (Alien alien in aliens)
            {
                if (alien.Active)
                {
                    alien.Update(gameTime, WINDOW_HEIGHT);
                    alien.UpdateBullets(gameTime, spaceship, 100);
                }
            }
            addAliensToExplodeToExplosionList();
            foreach (Explosion explosion in aliens_to_explode) 
            {
                explosion.Play(AliensToExplode[aliens_to_explode.IndexOf(explosion)].X, AliensToExplode[aliens_to_explode.IndexOf(explosion)].Y);
                explosion.Update(gameTime);

            }
            base.Update(gameTime);
        }
        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            KeyboardState keyboard = Keyboard.GetState();
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spaceship.Draw(spriteBatch);
            spaceship.DrawBullet(Content, spriteBatch, keyboard, bullet);
            foreach (Alien alien in aliens)
            {
                if (alien.Active)
                {
                    // Fix the shoot problem
                    alien.Draw(spriteBatch);
                    alien.DrawUpdatedBullet(spriteBatch);
                    AlienShoot(gameTime, alien);
                }   
            }

            foreach (Explosion explosion in aliens_to_explode) 
            {
                explosion.Draw(spriteBatch);
                AliensToExplode[aliens_to_explode.IndexOf(explosion)].Active = false;
            }
            spriteBatch.End();
            base.Draw(gameTime);

        }


        #endregion
        #endregion

        #region Private Mehods 
        private void addAliensToExplodeToExplosionList()
        {
 	        foreach (Alien alien in AliensToExplode)
            {
                aliens_to_explode.Add(new Explosion(Content)); 
            }
        }
        private Vector2 getRandomLocation()
        {
            int X = rand.Next(1, WINDOW_WIDTH-ALIEN_WIDTH);
            int Y = 0 - ALIEN_HEIGHT;
            return (new Vector2(X, Y));
        }
        private void cleanUpAliens()
        {
            for (int i = aliens.Count() - 1; i >= 0; i--)
            {
                if (!(aliens[i].Active))
                {
                    aliens.RemoveAt(i);
                }
            }
        }
        // Fix the problem over here, it isn't shooting problem
        private void AlienShoot(GameTime gameTime, Alien alien) 
        {
            SHOOT_TIME += gameTime.ElapsedGameTime.Milliseconds;
            if (SHOOT_TIME > timeToShoot)
            {
                alien.DrawBullets(Content, alienBullet, spriteBatch, new Vector2(spaceship.X, spaceship.Y));
                SHOOT_TIME = 0;
            }
        }
        #endregion
    }
}
