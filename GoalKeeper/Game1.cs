﻿/* Author: Bethany Weddle
 * CIS580 Project 1
 * */
using Microsoft.Xna.Framework;
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

        //The ball variables
        Texture2D ball;
        Rectangle ballRect;
        Random random = new Random();
        Vector2 ballPosition = Vector2.Zero;
        BoundingCircle ballBound;
        Vector2 ballVelocity;

        //The paddle variables
        Texture2D paddle;
        BoundingRectangle bound;

        KeyboardState oldstate;
        KeyboardState newState;

        //To keep track of lives and levels
        Texture2D heart;
        int lives;
        int levels;

        // Whether the game is over or has started
        bool beginGame;
        bool endGame;

        // For background start and end page
        Texture2D backgroundStart;
        Texture2D backgroundEnd;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            graphics.ApplyChanges();

            ballVelocity = new Vector2(
                (float)random.NextDouble(),
                (float)random.NextDouble());
            //same speed, random direction
            ballVelocity.Normalize();

            bound.Width = 50;
            bound.Height = 250;
            bound.X = 0;
            bound.Y = GraphicsDevice.Viewport.Height / 2 - bound.Height / 2;

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
            ball = Content.Load<Texture2D>("ball");
            paddle = Content.Load<Texture2D>("onepixel");
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
            BeginGame();

            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // increasing or/and decreasing the speed of the paddle
            if (newState.IsKeyDown(Keys.Up) && !oldstate.IsKeyDown(Keys.Down))
            {
                bound.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                //move up
            }
            if (newState.IsKeyDown(Keys.Down) && !oldstate.IsKeyDown(Keys.Up))
            {
                bound.Y += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                //move down
            }

            // Making sure paddle doesn't go off screen
            if (bound.Y < 0)
            {
                bound.Y = 0;
            }
            if (bound.Y > GraphicsDevice.Viewport.Height - bound.Height)
            {
                bound.Y = GraphicsDevice.Viewport.Height - bound.Height;
            }

            // TODO: Add your update logic here
            ballPosition += (float)gameTime.ElapsedGameTime.TotalMilliseconds * ballVelocity;
            ballBound = new BoundingCircle(ballPosition.X, ballPosition.Y, 50);


            //Check for wall collisons, depends on where your wall is
            if (ballPosition.Y < 0) //top of screen
            {
                //invert direction 
                ballVelocity.Y *= -1;
                float delta = 0 - ballPosition.Y;
                ballPosition.Y += 2 * delta;
            }

            if (ballPosition.Y > graphics.PreferredBackBufferHeight - 100) // Bottom of screen
            {
                ballVelocity.Y *= -1;
                float delta = graphics.PreferredBackBufferHeight - 100 - ballPosition.Y;
                ballPosition.Y += 2 * delta;
            }

            if (ballPosition.X < 0) // Side of Screen
            {
                ballVelocity.X *= -1;
                float delta = 0 - ballPosition.X;
                ballPosition.X += 2 * delta;

                if (!CollisionDetected(bound, ballBound))
                {
                    lives--;
                }
                else
                {
                    if(levels == 2)
                    {
                        ballVelocity.X += 0.25f;
                    }
                    if(levels == 3)
                    {
                        ballVelocity.X += 0.25f;
                        bound.Height -= 10;
                    }                   
                }
 
            }

            if (ballPosition.X > graphics.PreferredBackBufferWidth - 100) // Side of screen
            {
                ballVelocity.X *= -1;
                float delta = graphics.PreferredBackBufferWidth - 100 - ballPosition.X;
                ballPosition.X += 2 * delta;
            }

            

            newState = oldstate;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

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
                ballRect = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, 100, 100);
                spriteBatch.Draw(ball, ballRect, Color.White);

                spriteBatch.Draw(paddle, bound, Color.Red);

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
            if (!beginGame)
            {
                Keys[] pressed = keyboardState.GetPressedKeys();
                if (pressed.Length >= 1)
                {
                    if (pressed[0] == Keys.D1 || pressed[0] == Keys.NumPad1)
                    {
                        bound.Height = 250;
                        levels = 1;
                        beginGame = true;
                    }
                    else if (pressed[0] == Keys.D2 || pressed[0] == Keys.NumPad2)
                    {
                        bound.Height = 200;
                        levels = 2;
                        beginGame = true;
                    }
                    else if (pressed[0] == Keys.D3 || pressed[0] == Keys.NumPad3)
                    {
                        bound.Height = 150;
                        levels = 3;
                        beginGame = true;
                    }
                    else
                    {
                        Exit();
                    }

                    ballVelocity.Normalize();
                    endGame = false;

                    return;
                }        
            }

            // Restart the game if Enter is pressed
            if (endGame)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    if(levels == 1)
                    {
                        bound.Height = 250;
                    }
                    else if(levels == 2)
                    {
                        bound.Height = 200;
                    }
                    else
                    {
                        bound.Height = 150;
                    }
                    
                    ballVelocity.Normalize();
                    lives = 5;
                    endGame = false;
                }
            }
        }

        public bool CollisionDetected(BoundingRectangle r, BoundingCircle c)
        {
            // Code for Collison Detected found at 
            // http://www.jeffreythompson.org/collision-detection/circle-rect.php
            // temporary variables to set edges for testing
            float testX = c.X;
            float testY = c.Y;

            // which edge is closest?
            if (c.X < r.X) testX = r.X;      // test left edge
            else if (c.X > r.X + r.Width) testX = r.X + r.Width;   // right edge
            if (c.Y < r.Y) testY = r.Y;      // top edge
            else if (c.Y > r.Y + r.Height) testY = r.Y + r.Height;   // bottom edge

            // get distance from closest edges
            float distX = c.X - testX;
            float distY = c.Y - testY;
            float distance = (distX * distX) + (distY * distY);

            // if the distance is less than the radius, collision!
            if (distance <= c.Radius * c.Radius)
            {
                return true;
            }
            return false;
        }
    }
}
