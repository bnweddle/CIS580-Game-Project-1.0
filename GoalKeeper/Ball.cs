/* Author: Nathan Bean
 * Modified By: Bethany Weddle
 * Date: 2-7-20
 * */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace GoalKeeper
{
    public class Ball
    {
        /// <summary>
        /// The game the ball belongs to
        /// </summary>
        Game1 game;

        /// <summary>
        /// The ball's texture
        /// </summary>
        public Texture2D texture;

        /// <summary>
        /// The ball's bounds
        /// </summary>
        public BoundingCircle Bounds;

        public Vector2 Position;

        /// <summary>
        /// The ball's velocity vector
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// Bouncing sound effect when soccer ball hits the walls
        /// </summary>
        SoundEffect bounceX;

        /// <summary>
        /// Creates a new ball
        /// </summary>
        /// <param name="game">The game the ball belongs to</param>
        public Ball(Vector2 position, Texture2D texture2)
        {
            position = Position;
            texture = texture2;
           
        }

        /// <summary>
        /// Initializes the ball, placing it in the center 
        /// of the screen and giving it a random velocity
        /// vector of length 1.
        /// </summary>
        public void Initialize(Game1 g)
        {

            this.game = g;

            Velocity = new Vector2(
                (float)g.Random.NextDouble(),
                (float)g.Random.NextDouble());

            // Set the ball's radius
            // Bounds.Radius = 25;

            // position the ball in the center of the screen
            Bounds.X = Position.X;
            Bounds.Y = Position.Y;       

            Velocity.Normalize();
        }

        /// <summary>
        /// Loads the ball's texture
        /// </summary>
        /// <param name="content">The ContentManager to use</param>
        public void LoadContent(ContentManager content)
        {
            //texture = content.Load<Texture2D>("ball");
            bounceX = content.Load<SoundEffect>("bounce");
        }

        /// <summary>
        /// Updates the ball's position and bounces it off walls
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void Update(GameTime gameTime)
        {
            //var viewport = game.GraphicsDevice.Viewport;

            Bounds.Center += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.50f * Velocity;

            // Check for wall collisions
            if (Bounds.Center.Y < Bounds.Radius)
            {
                Velocity.Y *= -1;
                float delta = Bounds.Radius - Bounds.Y;
                Bounds.Y += 2 * delta;
                if(bounceX != null)
                    bounceX.Play();
            }

            if (Bounds.Center.Y > game.GraphicsDevice.Viewport.Height - Bounds.Radius)
            {
                Velocity.Y *= -1;
                float delta = game.GraphicsDevice.Viewport.Height - Bounds.Radius - Bounds.Y;
                Bounds.Y += 2 * delta;
                if (bounceX != null)
                    bounceX.Play();
            }

            if (Bounds.X < Bounds.Radius)
            {
                Velocity.X *= -1;
                float delta = Bounds.Radius - Bounds.X;
                Bounds.X += 2 * delta;
                if (bounceX != null)
                    bounceX.Play();
            }


            if (Bounds.X > game.GraphicsDevice.Viewport.Width - Bounds.Radius)
            {
                Velocity.X *= -1;
                float delta = game.GraphicsDevice.Viewport.Width - Bounds.Radius - Bounds.X;
                Bounds.X += 2 * delta;
                if (bounceX != null)
                    bounceX.Play();
            }
        }

        /// <summary>
        /// Draws the ball
        /// </summary>
        /// <param name="spriteBatch">
        /// The SpriteBatch to use to draw the ball.  
        /// This method should be invoked between 
        /// SpriteBatch.Begin() and SpriteBatch.End() calls.
        /// </param>
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Bounds, Color.White);
        }
    }
}
