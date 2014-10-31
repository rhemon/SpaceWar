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
namespace SpaceWar
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Initializer
        // field to keep track of game state
        static GameState state;
        int score;
        int secWaitedFor = 0;
        const int SecondsToWaitFor = 1000;
        bool wasEscapeDown = false;
        Menu mainMenu;
        List<Explosion> spaceshipExplode = new List<Explosion>(); 
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Explosion> explosion = new List<Explosion>(); 
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;
        const int ALIEN_WIDTH = 75;
        const int ALIEN_HEIGHT = 50; 
        Spaceship spaceship;
        List<Alien> aliens = new List<Alien>();
        int ALIEN_SPAWN_TIME = 4000;
        int TIME_GONE = 0;
        
        const int timeToShoot = 1000;
        Texture2D expSprite;
        Texture2D bullet;
        Texture2D alien;
        Texture2D alienBullet;
        bool isGameOver = false; 
        Random rand = new Random();
        static SpriteFont font;
        Vector2 SCORE_POSITION = new Vector2(10, 10);
        Vector2 HEALTH_POSITION = new Vector2(10, 30);
        const string SCORE_STRING_PREFIX = "Score: ";
        const string HEALTH_STRING_PREFIX = "Health: ";
        static string scoreString;
        string healthString;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
     
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            IsMouseVisible = true;
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
            score = 0;
            ALIEN_SPAWN_TIME = 4000;
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
            mainMenu = new Menu(Content, WINDOW_WIDTH, WINDOW_HEIGHT);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spaceship = new Spaceship(Content, WINDOW_WIDTH, WINDOW_HEIGHT);
            bullet = Content.Load<Texture2D>("bullet");
            expSprite = Content.Load<Texture2D>("explosion");
            alienBullet = Content.Load<Texture2D>("alienBullet");
            alien = Content.Load<Texture2D>("alien");
            font = Content.Load<SpriteFont>("TextFont");
            scoreString = GetScoreString(score);
            healthString = GetHealthString(spaceship.Health);
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
        #region Properties
        public static string SCORE_STRING
        {
            get { return scoreString; }
        }
        public static SpriteFont Font
        {
            get { return font; }
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

            if (state == GameState.MainMenu)
            {
                // update main menu
                mainMenu.Update(Mouse.GetState());
            
            }
            else if (state == GameState.Play)
            {
                if (isGameOver)
                {
                    Initialize();
                    LoadContent();
                    isGameOver = false;
                }
                if (score == 100 && ALIEN_SPAWN_TIME != 2000)
                {
                    ALIEN_SPAWN_TIME /= 2;
                }
                if (score == 150 && ALIEN_SPAWN_TIME != 1000)
                {
                    ALIEN_SPAWN_TIME /= 2;
                }
                if (score == 200 && ALIEN_SPAWN_TIME != 500)
                {
                    ALIEN_SPAWN_TIME /= 2;
                }
                if (score == 300 && ALIEN_SPAWN_TIME != 369)
                {
                    ALIEN_SPAWN_TIME = 369;
                }
                scoreString = GetScoreString(score);
                KeyboardState keyboard = Keyboard.GetState();
                wasEscapeDown = keyboard.IsKeyDown(Keys.Escape);
                if (wasEscapeDown)
                {
                    wasEscapeDown = false;
                    ChangeState(GameState.MainMenu); 
                }
                // TODO: Add your update logic here
                if (!spaceship.Explode)
                {
                    spaceship.Update(gameTime, keyboard, WINDOW_WIDTH, WINDOW_HEIGHT);
                    spaceship.UpdateBullet(gameTime);
                }
                else if (spaceship.Explode)
                {
                    spaceshipExplode.Add(new Explosion(expSprite, spaceship.X+ 100/2, spaceship.Y + 100/2));
                    spaceshipExplode[0].Update(gameTime);
                }
                
                
                TIME_GONE += gameTime.ElapsedGameTime.Milliseconds;
                if (TIME_GONE > ALIEN_SPAWN_TIME)
                {
                    aliens.Add(new Alien(Content, alien, getRandomLocation(), ALIEN_WIDTH, ALIEN_HEIGHT));
                    TIME_GONE = 0;
                }
                
                foreach (Alien alien in aliens)
                {
                    if (alien.Active)
                    {
                        alien.Update(gameTime, WINDOW_HEIGHT, spaceship.Bullets);
                        alien.UpdateBullets(gameTime, spaceship, 100);
                        if (alien.Explode)
                        {
                            score += 10;
                            explosion.Add(new Explosion(expSprite, alien.DrawRectangle.Center.X, alien.DrawRectangle.Center.Y));
                            alien.Active = false;
                        }
                        if (spaceship.DrawRectangle.Contains(alien.X, alien.Y) || spaceship.DrawRectangle.Intersects(alien.DrawRectangle)) 
                        {
                            explosion.Add(new Explosion(expSprite, alien.DrawRectangle.Center.X, alien.DrawRectangle.Center.Y));
                            alien.Active = false;
                            spaceship.DecrementHealth();
                        }
                    }

                }
                foreach (Explosion exp in explosion)
                {
                    exp.Update(gameTime);
                }
                healthString = GetHealthString(spaceship.Health);
                gameOver(gameTime);
                cleanUpExplosion();
            }
            else
            {
                this.Exit();
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

            
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (state == GameState.MainMenu)
            {
                // draw the main menu
                mainMenu.Draw(spriteBatch);
            }
            else if (state == GameState.Play)
            {
                KeyboardState keyboard = Keyboard.GetState();
                if (!spaceship.Explode)
                {
                    spaceship.Draw(spriteBatch);
                    spaceship.DrawBullet(Content, spriteBatch, keyboard, bullet);
                }
                else
                {
                    if (spaceshipExplode.Count > 0)
                    {
                        spaceshipExplode[0].Draw(spriteBatch);
                    }
                }
                
                foreach (Alien alien in aliens)
                {
                    if (alien.Active)
                    {
                        
                        alien.Draw(spriteBatch);
                        alien.DrawUpdatedBullet(spriteBatch);
                        AlienShoot(gameTime, alien);
                    }
                }
                foreach (Explosion exp in explosion)
                {
                    exp.Draw(spriteBatch);
                }
                // draw score
                spriteBatch.DrawString(font, scoreString, SCORE_POSITION, Color.White);
                spriteBatch.DrawString(font, healthString, HEALTH_POSITION, Color.White);
            }
            else
            {
                this.Exit();
            }
            spriteBatch.End();
            base.Draw(gameTime);

        }

        public static void ChangeState(GameState newState)
        {
            state = newState;
        }
       
        #endregion
        #endregion

        #region Private Mehods 
        private void gameOver(GameTime gameTime)
        {
            if (spaceshipExplode.Count >= 1)
            {
                if (!(spaceshipExplode[0].Active) && spaceship.Explode)
                {
                    secWaitedFor += gameTime.ElapsedGameTime.Milliseconds;
                    if (secWaitedFor > SecondsToWaitFor)
                    {
                        isGameOver = true;
                        clearEverything();
                        ChangeState(GameState.MainMenu);
                    }
                }
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
        private void cleanUpExplosion()
        {
            for (int i = explosion.Count() - 1; i >= 0; i--)
            {
                if (!(explosion[i].Active))
                {
                    explosion.RemoveAt(i);
                }
            }
        }
        // Fix the problem over here, it isn't shooting problem
        private void AlienShoot(GameTime gameTime, Alien alien) 
        {
            alien.SHOOT_TIME += gameTime.ElapsedGameTime.Milliseconds;
            if (alien.SHOOT_TIME > timeToShoot)
            {
                alien.DrawBullets(Content, alienBullet, spriteBatch, new Vector2(spaceship.X, spaceship.Y));
                alien.SHOOT_TIME = 0;
            }
        }
        private void clearEverything()
        {
            TIME_GONE = 0;
            secWaitedFor = 0; 
            aliens.Clear();
            explosion.Clear();
            spaceshipExplode.Clear();
        }
        private string GetScoreString(int score)
        {
            return SCORE_STRING_PREFIX + score;
        }
        private string GetHealthString(int health)
        {
            return HEALTH_STRING_PREFIX + spaceship.Health;
        }
        #endregion
    }
}
