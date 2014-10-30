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
    class Bullet
    {
        #region Fields

        Texture2D sprite; 
        int active = 0;
        Vector2 velocity;
        Vector2 targetLocation;
        Rectangle drawRectangle;
        const int damage = 10; 

        #endregion

        #region Constructors 
        /// <summary>
        /// Constructor for Spaceship's Bullets
        /// </summary>
        /// <param name="Content">Content Manager</param>
        /// <param name="bulletSprite">The Texture2D Sprite (Picture) provided for the bullet</param>
        /// <param name="spaceship">The instance of the spaceship object</param>
        public Bullet(ContentManager Content, Texture2D bulletSprite, Spaceship spaceship)
        {
            velocity = new Vector2(0, 10);
            sprite = bulletSprite;
            drawRectangle = new Rectangle((int)(spaceship.X + 50 - 15 / 2), (int)(spaceship.Y - 10 / 2), sprite.Width, sprite.Height);
            active = 1;
        }

        /// <summary>
        /// Constructor for Alien's Bullets
        /// </summary>
        /// <param name="Content">Content Manager</param>
        /// <param name="bulletSprite">The Texture2D Sprite (Picture) provided for the bullet</param>
        /// <param name="alien">The instance of the alien object</param>
        /// <param name="targetLocation">Vector2 object to tell the (X,Y) so that the bullet can have it as its target</param>
        public Bullet(ContentManager Content, Texture2D bulletSprite, Alien alien, Vector2 targetLocation)
        {
            velocity = new Vector2(0, 10);
            sprite = bulletSprite;
            this.targetLocation = targetLocation;
            drawRectangle = new Rectangle((int)(alien.X + 50 - 10 / 2), (int)(alien.Y - 10 / 2), sprite.Width, sprite.Height);
            active = 1;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets and Sets boolean value that indicates if the Bullet is Active or not
        /// </summary>
        public int Active
        {
            get { return active; }
        }

        /// <summary>
        /// Gets an integer value that tells how much the bullet damages, it is a constant 
        /// </summary>
        public int DAMAGE
        {
            get { return damage; }
        }
        
        #endregion

        #region Methods

        public void Update(GameTime gameTime)
        {
            drawRectangle.Y -= (int)(this.velocity.Y);
            if (drawRectangle.Y + drawRectangle.Height < 0)
            {
                active = 0;
            }

        }

        public void UpdateAlienBullet(GameTime gameTime)
        {
            if (targetLocation.Y > drawRectangle.Y) 
            {
                drawRectangle.Y += (int)(this.velocity.Y);
                DecideXMovement();
            }
            else if (targetLocation.Y < drawRectangle.Y)
            {
                drawRectangle.Y -= (int)(this.velocity.Y);
                DecideXMovement();
            }

        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        #region Private Methods
        private void DecideXMovement()
        {
            if (targetLocation.X > drawRectangle.X)
            {
                targetLocation.X += (int)(this.velocity.X);
            }
            else if (targetLocation.X < drawRectangle.X)
            {
                targetLocation.X -= (int)(this.velocity.X);
            }
        }
        #endregion

        #endregion


    }
}
