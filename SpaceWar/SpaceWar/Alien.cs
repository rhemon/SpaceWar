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
    class Alien
    {
        #region Fields 
        bool active; 
        Texture2D sprite;
        Vector2 velocity;
        Rectangle drawRectangle;
        List<Bullet> bullets = new List<Bullet>();
         
        int health;
        #endregion

        #region Constructors
        public Alien(Texture2D sprite, Vector2 location, int WIDTH, int HEIGHT)
        {
            this.sprite = sprite;
            drawRectangle = new Rectangle((int)location.X, (int)location.Y, WIDTH, HEIGHT);
            velocity = new Vector2(0, 2);
            health = 20;
            active = true; 
        }
        #endregion

        #region Properties
        public int Health
        {
            get { return health; } 
        }
        public bool Active
        {
            get { return active; }
        }
        public int X
        {
            get { return drawRectangle.X; }
        }
        public int Y
        {
            get { return drawRectangle.Y; }
        }
        #endregion

        #region Methods

        #region Public Methods
        public void Update(GameTime gameTime, int WINDOW_HEIGHT)
        {
            drawRectangle.Y += (int)(this.velocity.Y);
            if ((drawRectangle.Y + drawRectangle.Height) > WINDOW_HEIGHT)
            {
                this.active = false; 
            }
        }
        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }
        public void UpdateBullets(GameTime gameTime)
        {
            foreach (Bullet bullet in bullets)
            {
                if (bullet.Active == 1)
                {
                    bullet.UpdateAlienBullet(gameTime);
                }
            }
            //cleanupbullets();
        }
        public void DrawBullets(ContentManager Content, Texture2D bulletSprite, SpriteBatch spriteBatch, Vector2 targetLocation)
        {
            bullets.Add(new Bullet(Content, bulletSprite, this, targetLocation));
            foreach (Bullet bullet in bullets) 
            {
                if (bullet.Active == 1) 
                {
                    bullet.Draw(spriteBatch);
                }
            }
        }  
        #endregion

        #region Private Methods
        #endregion

        #endregion

    }
}
