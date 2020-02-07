/* Author: Bethany Weddle
 * CIS580 Project 1
 * */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GoalKeeper
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Ball ball;
        Paddle paddle;
        public Random Random = new Random();

        KeyboardState newState;

        //To keep track of lives and levels
        Texture2D heart;
        int lives;
        int levels;

        // Whether the game is over or has started
        bool beginGame;
        bool endGame;

        // Sound effects for losing lives, hitting paddle, ending game;
        SoundEffect loseLife;
        SoundEffect gameOver;
        SoundEffect paddleHit;

        // For background start and end page
        Texture2D backgroundStart;
        Texture2D backgroundEnd;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            paddle = new Paddle(this);
            ball = new Ball(this);
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
            graphics.PreferredBackBufferWidth = 1042;
            graphics.PreferredBackBufferHeight = 768;

            ball.Initialize();
            paddle.Initialize();
            graphics.ApplyChanges();

            lives = 5;

            // Keep track of the game starting and ending
            endGame = false;
            beginGame = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            loseLife = Content.Load<SoundEffect>("lose_life");
            gameOver = Content.Load<SoundEffect>("game_over");
            paddleHit = Content.Load<SoundEffect>("paddle_hit");
            ball.LoadContent(Content);
            paddle.LoadContent(Content);
            heart = Content.Load<Texture2D>("heart");
            // The image before starting the game
            backgroundStart = Content.Load<Texture2D>("levels");
            backgroundEnd = Content.Load<Texture2D>("end");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            newState = Keyboard.GetState();

            BeginGame();
            // Why do I have to hit spacebar twice?
            MuteSound(newState);

            if (newState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            paddle.Update(gameTime);
            ball.Update(gameTime);

            // bounce off the side wall and decrement lives
            if (ball.Bounds.X < ball.Bounds.Radius)
            {
                lives--;
                if(lives > 0)
                    loseLife.Play();
                else
                    gameOver.Play();

                ball.Velocity.X *= -1;
                float delta = ball.Bounds.Radius - ball.Bounds.X;
                ball.Bounds.X += 2 * delta;
            }
            
            
            // Bounce off the board
            if (CollisionDetected(paddle.Bounds, ball.Bounds))
            {
                paddleHit.Play();

                ball.Velocity.X *= -1;
                var bounce = (paddle.Bounds.X + paddle.Bounds.Width) - (ball.Bounds.X - ball.Bounds.Radius);
                ball.Bounds.X += 2 * bounce;
                if (levels == 2)
                {
                    ball.Velocity.X += 0.25f;
                }
                if (levels == 3)
                {
                    ball.Velocity.X += 0.25f;
                    paddle.Bounds.Height -= 10;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SeaGreen);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (!beginGame)
            {
                // Fill the screen with black before the game starts
                spriteBatch.Draw(backgroundStart, new Rectangle(0, 0,
                (int)graphics.PreferredBackBufferWidth, (int)graphics.PreferredBackBufferHeight), Color.White);

            }
            else if (lives <= 0)
            {
                spriteBatch.Draw(backgroundEnd, new Rectangle(0, 0,
                (int)graphics.PreferredBackBufferWidth, (int)graphics.PreferredBackBufferHeight), Color.White);
                endGame = true;
            }
            else
            {

                ball.Draw(spriteBatch);
                paddle.Draw(spriteBatch);

                int start = 50;
                for (int i = 0; i < lives; i++)
                {
                    spriteBatch.Draw(heart, new Rectangle(graphics.PreferredBackBufferWidth - start, graphics.PreferredBackBufferHeight - 50, 50, 50), Color.White);
                    start += 50;
                }
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }

        void BeginGame()
        {
            // Got general idea for outline of code from 
            // https://docs.microsoft.com/en-us/windows/uwp/get-started/get-started-tutorial-game-mg2d
            KeyboardState keyboardState = Keyboard.GetState();

            // Quit the game if Escape is pressed.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // Start the game if Space is pressed.
            // Exit the keyboard handler method early, preventing the dino from jumping on the same keypress.
            if(!beginGame)
            {
                ball.Velocity = Vector2.Zero;
                Keys[] pressed = keyboardState.GetPressedKeys();
                if (pressed.Length >= 1)
                {
                    if (pressed[0] == Keys.D1 || pressed[0] == Keys.NumPad1)
                    {
                        paddle.Bounds.Height = 250;
                        levels = 1;
                        beginGame = true;      
                    }
                    else if (pressed[0] == Keys.D2 || pressed[0] == Keys.NumPad2)
                    {
                        paddle.Bounds.Height = 200;
                        levels = 2;
                        beginGame = true;
                    }
                    else if (pressed[0] == Keys.D3 || pressed[0] == Keys.NumPad3)
                    {
                        paddle.Bounds.Height = 150;
                        levels = 3;
                        beginGame = true;
                    }
                    else
                    {
                        Exit();
                    }

                    ball.Initialize();
                    ball.Velocity.Normalize();
                    endGame = false;
                }
            }

            // Restart the game if Enter is pressed
            if (endGame)
            {
                ball.Velocity = Vector2.Zero;
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    if(levels == 1)
                    {
                        paddle.Bounds.Height = 250;
                    }
                    else if(levels == 2)
                    {
                        paddle.Bounds.Height = 200;
                    }
                    else
                    {
                        paddle.Bounds.Height = 150;
                    }

                    ball.Initialize();
                    ball.Velocity.Normalize();
                    lives = 5;
                    endGame = false;
                }
            }
        }


        public void MuteSound(KeyboardState state)
        {
            // idea of implementation from 
            //https://www.gamefromscratch.com/post/2015/07/25/MonoGame-Tutorial-Audio.aspx
            if (state.IsKeyDown(Keys.Space) )
            {
                if (SoundEffect.MasterVolume == 1.0f)
                {
                    SoundEffect.MasterVolume = 0.0f;
                }
                else
                {
                    SoundEffect.MasterVolume = 1.0f;
                }              
            }
        }

        public bool CollisionDetected(BoundingRectangle r, BoundingCircle c)
        {
            var closestX = Math.Max(r.X, Math.Min(c.X, r.X + r.Width));
            var closestY = Math.Max(r.Y, Math.Min(c.Y, r.Y + r.Height));
            return (Math.Pow(c.Radius, 2) >= Math.Pow(closestX - c.X, 2) + Math.Pow(closestY - c.Y, 2));
        }
    }
}
