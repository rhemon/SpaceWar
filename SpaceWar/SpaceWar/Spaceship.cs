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
    class Spaceship
    {
        #region Fields 

        // Spaceship
        // Spaceship Draw 
        Texture2D sprite;
        Rectangle drawRectangle;
        Vector2 location;
        const int SPRITE_WIDTH_AND_HEIGHT = 100;
        //Spaceship Movement 
        Vector2 velocity;
        bool explode = false; 

        // Bullet 
        List<Bullet> bullets;
        ButtonState previouslyPressed = ButtonState.Released;
        // Spaceship Health
        int health;
        


        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Content">Content Manger</param>
        /// <param name="WINDOW_WIDTH">Window's Width</param>
        /// <param name="WINDOW_HEIGHT">Window's Height</param>
        public Spaceship(ContentManager Content, int WINDOW_WIDTH, int WINDOW_HEIGHT)
        {
            bullets = new List<Bullet>();
            // Initializing the sprite, velocity and health of spaceship 
            sprite = Content.Load<Texture2D>("spaceship");
            velocity = new Vector2(5, 5);
            location = new Vector2((WINDOW_WIDTH / 2) - (SPRITE_WIDTH_AND_HEIGHT / 2), WINDOW_HEIGHT - SPRITE_WIDTH_AND_HEIGHT);
            drawRectangle = new Rectangle((int)location.X, (int)location.Y, 100, 100); 
            health = 100;

          
        }

        #endregion

        #region Properties 
        /// <summary>
        /// Gets the health 
        /// </summary>
        public int Health
        {
            get { return health; }
        }

        public int X
        {
            get { return drawRectangle.X; }
        }

        public int Y
        {
            get { return drawRectangle.Y; }
        }
        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
        }
        public List<Bullet> Bullets
        {
            get { return bullets; }
        }
        public bool Explode
        {
            get { return explode; }
        }
        #endregion

        #region Methods 

        #region Public Methods
        public void Update(GameTime gameTime, KeyboardState keyboard, int WINDOW_WIDTH, int WINDOW_HEIGHT)
        {
            if (keyboard.IsKeyDown(Keys.Up))
            {
                drawRectangle.Y -= (int)(this.velocity.Y);
                if (drawRectangle.Y < 0)
                {
                    drawRectangle.Y = 0;
                }
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                drawRectangle.Y += (int)(this.velocity.Y);
                if (drawRectangle.Y + drawRectangle.Height > WINDOW_HEIGHT)
                {
                    drawRectangle.Y = WINDOW_HEIGHT - drawRectangle.Height;
                }
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                drawRectangle.X += (int)(this.velocity.X);
                if (drawRectangle.X + drawRectangle.Width > WINDOW_WIDTH)
                {
                    drawRectangle.X = WINDOW_WIDTH - drawRectangle.Width; 
                }
            }
            if (keyboard.IsKeyDown(Keys.Left))
            {
                drawRectangle.X -= (int)(this.velocity.X);
                if (drawRectangle.X < 0)
                {
                    drawRectangle.X = 0;
                }
            }
            if (health <= 0)
            {
                this.explode = true; 
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        public void UpdateBullet(GameTime gameTime)
        {
            foreach (Bullet bullet in bullets)
            {
                if (bullet.Active == 1)
                {
                    bullet.Update(gameTime);
                    
                }
            }
            this.bulletsCleanUp();
        }

        public void DrawBullet(ContentManager Content, SpriteBatch spriteBatch, KeyboardState keyboard, Texture2D bulletSprite)
        {
            if (keyboard.IsKeyDown(Keys.Space)) 
            {
                previouslyPressed = ButtonState.Pressed; 
            }
            if (keyboard.IsKeyUp(Keys.Space) && previouslyPressed == ButtonState.Pressed)  
            {
                bullets.Add(new Bullet(Content, bulletSprite, this));
                bullets[bullets.Count - 1].Draw(spriteBatch);
                previouslyPressed = ButtonState.Released;
            }
            foreach (Bullet bullet in bullets)
            {
                if (bullet.Active == 1)
                {
                    bullet.Draw(spriteBatch);
                }
            }
        }
        public void DecrementHealth()
        {
            if (health > 0)
            {
                health -= 5;
            }
        }
        #endregion

        #region Private Methods
        private void bulletsCleanUp() 
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                if (bullets[i].Active == 0)
                {
                       bullets.RemoveAt(i);
                }
            }

        }

        #endregion

        #endregion


    }
}
