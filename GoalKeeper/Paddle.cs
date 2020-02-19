/* Author: Nathan Bean
 * Modified By: Bethany Weddle
 * Date: 2-7-20
 * */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalKeeper
{
    public class Paddle
    {
        public BoundingRectangle Bounds;
        Game1 Game;
        Texture2D texture;
        KeyboardState oldstate;
        Vector2 position;

        /// <summary>
        /// Creates a paddle
        /// </summary>
        /// <param name="game">Reference to the game</param>
        public Paddle(Game1 game, Vector2 position)
        {
            this.Game = game;
            this.position = position;
        }

        public void Initialize()
        {
            Bounds.Width = 50;
            Bounds.Height = 250;
            Bounds.X = position.X;
            Bounds.Y = position.Y;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("onepixel");
        }

        public void Update(GameTime gameTime)
        {
            //Don't call in Game class 
            //Movement
            var newState = Keyboard.GetState();

            // increasing or/and decreasing the speed of the paddle
            if (newState.IsKeyDown(Keys.Up) && !oldstate.IsKeyDown(Keys.Down))
            {
                Bounds.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                //move up
            }
            if (newState.IsKeyDown(Keys.Down) && !oldstate.IsKeyDown(Keys.Up))
            {
                Bounds.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                //move down
            }

            // Making sure paddle doesn't go off screen
            if (Bounds.Y < 0)
            {
                Bounds.Y = 0;
            }
            if (Bounds.Y > Game.GraphicsDevice.Viewport.Height - Bounds.Height)
            {
                Bounds.Y = Game.GraphicsDevice.Viewport.Height - Bounds.Height;
            }

            oldstate = newState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Bounds, Color.White);
        }

        }
}
