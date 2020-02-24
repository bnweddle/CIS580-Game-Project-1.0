/* Author: Nathan Bean
 * Modified by Bethany Weddle
 * In class work for Sprite Animation, need to use a state machine to track player movement
 * */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GoalKeeper
{
    /// <summary>
    /// For which direction the player is facing
    /// </summary>
    public enum State
    {
        South = 0,
        North = 1,
        West = 2,
        East = 3,
        Idle = 4
    }

    public class Player 
    {
        // how much the animation moves per frames 
        const int ANIMATION_FRAME_RATE = 124;
        // the speed of the player
        const float PLAYER_SPEED = 200;
        // width of animation frames
        public const int FRAME_WIDTH = 67;
        // height of animation frames
        const int FRAME_HEIGHT = 100;


        Game1 game;
        Texture2D player;
        public State state;
        TimeSpan timer;
        public Vector2 Position;
        public BoundingRectangle Bounds;
        public int score;
        KeyboardState oldState;
        int frame;
        SoundEffect playerHit;


        public Player(Game1 game, Vector2 position)
        {
            this.game = game;
            this.Position = position;          
        }

        public void Initialize()
        {
            score = 0;
            timer = new TimeSpan(0);
            state = State.Idle;
            Bounds.Width = FRAME_WIDTH;
            Bounds.Height = FRAME_HEIGHT;
        }

        public void LoadContent(ContentManager content)
        {
            player = content.Load<Texture2D>("newPlayer");
            playerHit = content.Load<SoundEffect>("paddle_hit");
        }

        public void Update(GameTime gameTime, Keys[] keys, Ball ball)
        {
            //Movement
            KeyboardState newState = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Bounds.X = Position.X;
            Bounds.Y = Position.Y;

            CheckCollisions(ball);

            if (newState.IsKeyDown(keys[0]))
            {
                state = State.North;
                Position.Y -= delta * PLAYER_SPEED;
            }
            else if (newState.IsKeyDown(keys[1]))
            {
                state = State.West;
                Position.X -= delta * PLAYER_SPEED;
            }
            else if (newState.IsKeyDown(keys[2]))
            {
                state = State.East;
                Position.X += delta * PLAYER_SPEED;
            }
            else if (newState.IsKeyDown(keys[3]))
            {
                state = State.South;
                Position.Y += delta * PLAYER_SPEED;
            }
            else state = State.Idle;

            // Making sure player doesn't go off screen
            if (Position.Y < 0)
            {
                Position.Y = 0;
            }
            if (Position.X < 0)
            {
                Position.X = 0;
            }
            if (Position.Y > game.GraphicsDevice.Viewport.Height - FRAME_HEIGHT)
            {
                Position.Y = game.GraphicsDevice.Viewport.Height - FRAME_HEIGHT;
            }
            if (Position.X > game.GraphicsDevice.Viewport.Width - FRAME_WIDTH)
            {
                Position.X = game.GraphicsDevice.Viewport.Width - FRAME_WIDTH;
            }

            // update animation timer when the player is moving
            if (state != State.Idle)
                timer += gameTime.ElapsedGameTime;

            // Check if animation should increase by more than one frame
            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                // increase frame
                frame++;
                // Decrease the timer by one frame duration
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }

            frame %= 4;
            oldState = newState;
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            Rectangle rectSource = new Rectangle(
                frame * FRAME_WIDTH,  // X value
                (int)state % 4 * FRAME_HEIGHT, // Y value
                FRAME_WIDTH,
                FRAME_HEIGHT
                );

            spriteBatch.Draw(player, Position, rectSource, Color.White);
        }

        public void CheckCollisions(Ball ball)
        {
            Vector2 collisionPoint; 
            if(ball.Bounds.CollidesWith(Bounds, out collisionPoint))
            {
                //left side - good
               if(Bounds.X == collisionPoint.X)
                {
                    ball.Velocity.X *= -1;
                    float delta = ball.Bounds.Radius - ball.Bounds.X;
                    Bounds.X += 2 * delta;
                    playerHit.Play();
                }
                //top - good
                if(Bounds.Y == collisionPoint.Y)
                {
                    ball.Velocity.Y *= -1;
                    float delta = ball.Bounds.Radius - ball.Bounds.Y;
                    Bounds.Y += 2 * delta;
                    playerHit.Play();
                }
                //right - good
                if (Bounds.X + Bounds.Width == collisionPoint.X)
                {
                    ball.Velocity.X *= -1;
                    var bounce = (Bounds.X + Bounds.Width) - (ball.Bounds.X - ball.Bounds.Radius);
                    ball.Bounds.X += 2 * bounce;
                    playerHit.Play();
                }
                //bottom - good
                if(Bounds.Y + Bounds.Height == collisionPoint.Y)
                {
                    ball.Velocity.Y *= -1;
                    float delta = (Bounds.Y + Bounds.Height) - (ball.Bounds.Y - ball.Bounds.Radius);
                    ball.Bounds.Y += 2 * delta;
                    playerHit.Play();
                }

            }
        }

    }
}
