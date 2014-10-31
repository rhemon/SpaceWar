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
         
        bool explode = false; 
        bool active;
        Texture2D sprite;
        Vector2 velocity;
        int shootTIME = 0;
        Rectangle drawRectangle;
        List<Bullet> bullets = new List<Bullet>();
         
        int health;
        #endregion

        #region Constructors
        public Alien(ContentManager Content, Texture2D sprite, Vector2 location, int WIDTH, int HEIGHT)
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
            set { active = value; }
        }
        public bool Explode
        {
            get { return explode; }
        }
        public int X
        {
            get { return drawRectangle.X; }
        }
        public int Y
        {
            get { return drawRectangle.Y; }
        }
        public int SHOOT_TIME
        {
            get { return shootTIME; }
            set { shootTIME = value; }
        }
        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
        }
        #endregion

        #region Methods

        #region Public Methods
        public void Update(GameTime gameTime, int WINDOW_HEIGHT, List<Bullet> SpaceshipBullets)
        {
            
            drawRectangle.Y += (int)(this.velocity.Y);
            if ((drawRectangle.Y + drawRectangle.Height) > WINDOW_HEIGHT)
            {
                this.active = false;
            }
            
            
            GotHit(SpaceshipBullets);
        }
        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
           
        }
        public void UpdateBullets(GameTime gameTime, Spaceship spaceship, int SPACESHIP_WIDTH)
        {
            foreach (Bullet bullet in bullets)
            {
                if (bullet.Active == 1)
                {
                    bullet.UpdateAlienBullet(gameTime, spaceship);
                }
                
            }
            //cleanupbullets();
        }

        public void DrawUpdatedBullet(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in bullets)
            {
                if (bullet.Active == 1)
                {
                    bullet.Draw(spriteBatch);
                }
            }
            cleanUpBullets(); 
        }

        public void DrawBullets(ContentManager Content, Texture2D bulletSprite, SpriteBatch spriteBatch, Vector2 targetLocation)
        {
            bullets.Add(new Bullet(Content, bulletSprite, this, targetLocation));
            this.DrawUpdatedBullet(spriteBatch);
        }
        
        #endregion

        #region Private Methods

        private void cleanUpBullets()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                if (bullets[i].Active == 0)
                {
                    bullets.RemoveAt(i);
                }
            }

        }

        private void GotHit(List<Bullet> SpaceshipBullets)
        {
            foreach (Bullet bullet in SpaceshipBullets)
            {
                if ((bullet.X >= drawRectangle.X && bullet.X <= drawRectangle.X + drawRectangle.Width) && ((bullet.Y <= drawRectangle.Y + drawRectangle.Height) && (bullet.Y >= drawRectangle.Y)))
                {
                    this.explode = true;
                    bullet.Active = 0;
                }
            }
        }
        #endregion

        #endregion

    }
}
