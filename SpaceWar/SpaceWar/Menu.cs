using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar
{
    /// <summary>
    /// A class for a front end menu
    /// </summary>
    class Menu
    {
        #region Fields

        // fields for buttons
        MenuButton playButton;
        MenuButton quitButton;
        Vector2 STRING_POSION = new Vector2(10, 10);
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentManager">the content manager for loading content</param>
        /// <param name="windowWidth">the window width</param>
        /// <param name="windowHeight">the window height</param>
        public Menu(ContentManager contentManager, int windowWidth, int windowHeight)
        {
            // used for button placement
            int centerX = windowWidth / 2;
            int topCenterY = windowHeight / 4;
            Vector2 buttonCenter = new Vector2(centerX, topCenterY);

            // create buttons
            playButton = new MenuButton(contentManager,
                contentManager.Load<Texture2D>("playbutton"),
                buttonCenter, GameState.Play);
            buttonCenter.Y += windowHeight / 2;
            quitButton = new MenuButton(contentManager,
                contentManager.Load<Texture2D>("quitbutton"),
                buttonCenter, GameState.Quit);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the menu
        /// </summary>
        /// <param name="mouse">the current mouse state</param>
        public void Update(MouseState mouse)
        {
            // update buttons
            playButton.Update(mouse);
            quitButton.Update(mouse);
        }

        /// <summary>
        /// Draws the menu
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw buttons
            spriteBatch.DrawString(Game1.Font, Game1.SCORE_STRING, STRING_POSION, Color.White);
            playButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }

        #endregion
    }
}
