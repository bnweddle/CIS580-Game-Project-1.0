/* Author: Bethany Weddle
 * CIS580 Project 1
 * */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ParticleSystemStarter;
using System;
using System.Collections.Generic;

namespace GoalKeeper
{
    
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        //Using flyweight pattern to use two instances of player and paddle
        Ball ball;
        Paddle paddle;
        Paddle enemyPaddle;
        Player player;
        Player enemy;

        Texture2D ballText;

        //Used for Zooming/Matrix
        Camera camera = new Camera();
        Viewport view;
        MouseState mouse;

        // For the ball starting point velocity
        public Random Random = new Random();

        // To see if Mute/Spacebar was pressed 
        KeyboardState newState;
        KeyboardState oldState;

        // Whether the game is over or has started
        bool beginGame;
        bool endGame;

        // Sound effects for losing lives, hitting paddle, ending game;
        SoundEffect loseLife;
        SoundEffect gameOver;

        // For background start and end page
        Texture2D backgroundStart;
        Texture2D player1Wins;
        Texture2D player2Wins;

        bool mute = true;
        // Using Spatial Partitioning 
        public Grid grid = new Grid();
        Unit unitP1;
        Unit unitP2;

        /// <summary>
        /// For particle effects
        /// </summary>
        ParticleSystem rainSystem;
        ParticleSystem snowSystem;
        ParticleSystem fireworkSystem;
        ParticleSystem fireSystem;

        Texture2D rainTexture;
        Texture2D snowTexture;
        Texture2D particleTexture;
        Texture2D fireTexture;
        Random random = new Random();

        //This isn't working right
        Song rain;
        SoundEffect crack;

        DateTime startTime = DateTime.UtcNow;
        TimeSpan breakDuration = TimeSpan.FromSeconds(1.0);


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            paddle = new Paddle(this, new Vector2(0, graphics.PreferredBackBufferHeight / 2));
            enemyPaddle = new Paddle(this, new Vector2(992, graphics.PreferredBackBufferHeight / 2));
            ball = new Ball(
                new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), ballText);
            player = new Player(this, new Vector2(200, 200));
            enemy = new Player(this, new Vector2(800, 200));
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
            view.Width = 1042;
            view.Height = 768;

            IsMouseVisible = true;

            player.Initialize();
            enemy.Initialize();
            if(!beginGame)
            {
                ball.Initialize(this);
            }
            paddle.Initialize();
            enemyPaddle.Initialize();

            unitP1 = new Unit(player.Position.X, player.Position.Y, 1, grid);
            unitP2 = new Unit(enemy.Position.X, enemy.Position.Y, 2, grid);

            grid.Add(unitP1);
            grid.Add(unitP2);
            graphics.ApplyChanges();

            // Keep track of the game starting and ending
            endGame = false;
            beginGame = false;
            SoundEffect.MasterVolume = 0.0f;

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
            ball.LoadContent(Content);
            ballText = Content.Load<Texture2D>("ball");
            paddle.LoadContent(Content);
            enemyPaddle.LoadContent(Content);
            player.LoadContent(Content);
            enemy.LoadContent(Content);
            font = Content.Load<SpriteFont>("DefaultFont");
            // The image before starting the game
            backgroundStart = Content.Load<Texture2D>("start");
            player1Wins = Content.Load<Texture2D>("end1");
            player2Wins = Content.Load<Texture2D>("end2");
            // TODO: use this.Content to load your game content here
            LoadParticleEffects();

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
            mouse = Mouse.GetState();
            //startTime = DateTime.UtcNow;

            BeginGame();
            // Why do I have to hit spacebar twice?
            MuteSound(newState, oldState);

            if (newState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //For different keys for each player
            Keys[] keylist1 = { Keys.W, Keys.A, Keys.D, Keys.S };
            Keys[] keylist2 = { Keys.Up, Keys.Left, Keys.Right, Keys.Down };


            ball.Update(gameTime);
            player.Update(gameTime, keylist1, ball);
            enemy.Update(gameTime, keylist2, ball);
            camera.Update(mouse, view);

            ///Particle Systems
            snowSystem.Update(gameTime);
            fireSystem.Update(gameTime);
            rainSystem.Update(gameTime);
            fireworkSystem.Update(gameTime);
                
            unitP1.Update(player.Position.X, player.Position.Y);
            unitP2.Update(enemy.Position.X, enemy.Position.Y);
            grid.CheckCollisions();

            // Bounce off the player 1 board
            if (ball.Bounds.CollidesWith(paddle.Bounds))
            {
                enemy.score++;
                if (enemy.score < 5)
                    loseLife.Play();
                else if (enemy.score == 5)
                {
                    gameOver.Play();
                    endGame = true;
                }

                ball.Velocity.X *= -1;
                var bounce = (paddle.Bounds.X + paddle.Bounds.Width) - (ball.Bounds.X - ball.Bounds.Radius);
                ball.Bounds.X += 2 * bounce;

            }      
            
            if(ball.Bounds.CollidesWith(enemyPaddle.Bounds))
            {
                player.score++;
                if (player.score < 5)
                    loseLife.Play();
                else if (player.score == 5)
                {
                    gameOver.Play();
                    endGame = true;
                }

                ball.Velocity.X *= -1;
                float delta = enemyPaddle.Bounds.X - ball.Bounds.X - ball.Bounds.Radius;
                ball.Bounds.X += 2 * delta;
            }


            oldState = newState;
            base.Update(gameTime);
          
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SeaGreen);

            //Passing in the Camera Matrix to allow zooming capability
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.Matrix);

            if (!beginGame)
            {
                // Fill the screen with black before the game starts
                spriteBatch.Draw(backgroundStart, new Rectangle(0, 0,
                (int)graphics.PreferredBackBufferWidth, (int)graphics.PreferredBackBufferHeight), Color.White);

            }
            else if (endGame)
            {
                if(enemy.score == 5)
                {
                    spriteBatch.Draw(player2Wins, new Rectangle(0, 0,
                 (int)graphics.PreferredBackBufferWidth, (int)graphics.PreferredBackBufferHeight), Color.White);
                }
                else if(player.score == 5)
                {
                    spriteBatch.Draw(player1Wins, new Rectangle(0, 0,
                (int)graphics.PreferredBackBufferWidth, (int)graphics.PreferredBackBufferHeight), Color.White);
                }
                
            }
            else
            {
                ball.Draw(spriteBatch, ballText);
                paddle.Draw(spriteBatch);
                enemyPaddle.Draw(spriteBatch);
                player.Draw(spriteBatch);
                enemy.Draw(spriteBatch);

                // TODO: Add your update logic here 
                



                oldState = newState;

                spriteBatch.DrawString(font, "Player 1 Score: " + Convert.ToString(player.score), new Vector2(20,0), Color.White);
                spriteBatch.DrawString(font, "Player 2 Score: " + Convert.ToString(enemy.score), new Vector2(900, 0), Color.White);
               

                if(camera.Zoom == false)
                    spriteBatch.DrawString(font, "Use left-click to Zoom in", new Vector2(415, view.Height - 20), Color.White);
                if(camera.Zoom == true)
                    spriteBatch.DrawString(font, "Use right-click to Zoom out", new Vector2(415, view.Height - 20), Color.White);



                if (mute == false)
                    spriteBatch.DrawString(font, "Press M to mute", new Vector2(425, 0), Color.White);
                if(mute == true)
                    spriteBatch.DrawString(font, "Press M to unmute", new Vector2(420, 0), Color.White);

                spriteBatch.DrawString(font, "Hold B for Blizzard", new Vector2(0, view.Height - 20), Color.White);
                spriteBatch.DrawString(font, "Hold R for Rain", new Vector2(0, view.Height - 40), Color.White);
                spriteBatch.DrawString(font, "Press F for Fireworks", new Vector2(0, view.Height - 60), Color.White);


            }                
            
            spriteBatch.End();

            if(beginGame && !endGame)
                DrawParticles();

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
            // Exit the keyboard handler method early if not.
            if (!beginGame)
            {
                ball.Velocity = Vector2.Zero;
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    ball.Initialize(this);
                    ball.Velocity.Normalize();
                    endGame = false;
                    beginGame = true;
                }
            }

            // Restart the game if Enter is pressed
            if (endGame)
            {
                ball.Velocity = Vector2.Zero;
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    //Reset the game
                    ball.Initialize(this);
                    ball.Velocity.Normalize();
                    player.score = 0;
                    enemy.score = 0;
                    player.Position = new Vector2(200, 200);
                    enemy.Position = new Vector2(800, 200);
                    player.state = State.Idle;
                    enemy.state = State.Idle;
                    endGame = false;
                }
            }
        }


        public void MuteSound(KeyboardState newS, KeyboardState oldS)
        {
            // idea of implementation from 
            //https://www.gamefromscratch.com/post/2015/07/25/MonoGame-Tutorial-Audio.aspx
            if (!oldState.IsKeyDown(Keys.M) && newS.IsKeyDown(Keys.M))
            {
                if (SoundEffect.MasterVolume == 1.0f)
                {
                    SoundEffect.MasterVolume = 0.0f;
                    mute = true;
                }
                else
                {
                    SoundEffect.MasterVolume = 1.0f;
                    mute = false;
                }              
            }
        }

        public void LoadParticleEffects()
        {
            // TODO: use this.Content to load your game content here
            rainTexture = Content.Load<Texture2D>("drip");
            snowTexture = Content.Load<Texture2D>("snowflake");
            particleTexture = Content.Load<Texture2D>("Particle");
            fireTexture = Content.Load<Texture2D>("Storm");
            rain = Content.Load<Song>("raining");
            crack = Content.Load<SoundEffect>("crackle");
            MediaPlayer.Play(rain);

            MediaPlayer.IsMuted = true;
            MediaPlayer.IsRepeating = true;

            fireSystem = new ParticleSystem(GraphicsDevice, 25, fireTexture);
            fireworkSystem = new ParticleSystem(GraphicsDevice, 1000, particleTexture);
            rainSystem = new ParticleSystem(GraphicsDevice, 1000, rainTexture);
            snowSystem = new ParticleSystem(GraphicsDevice, 1000, snowTexture);

            fireworkSystem.SpawnPerFrame = 4;
            fireSystem.SpawnPerFrame = 4;
            rainSystem.SpawnPerFrame = 4;
            snowSystem.SpawnPerFrame = 4;

            // Set the SpawnParticle method
            snowSystem.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position.X = random.Next(0, 1280);
                particle.Position.Y = random.Next(0, 768);
                particle.Velocity.X = (float)random.NextDouble() - 0.5f;
                particle.Velocity.Y = (float)random.Next(1, 20);

                particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.Gray;
                particle.Scale = 0.015f; //for snow
                particle.Life = 2.0f;
            };

            // Set the SpawnParticle method for fire
            fireSystem.SpawnParticle = (ref Particle particle) =>
            {

                particle.Position = ball.Bounds.Center;
                particle.Velocity.X = MathHelper.Lerp(0, 25, (float)random.NextDouble());
                particle.Velocity.Y = MathHelper.Lerp(0, 100, (float)random.NextDouble());
                particle.Acceleration = Vector2.Zero;
                particle.Color = Color.OrangeRed;
                particle.Scale = 0.05f;
                particle.Life = 3.0f;
            };

            rainSystem.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position.X = random.Next(0, 1280); //for rain 
                particle.Position.Y += random.Next(0, 150); //for rain
                //This is like the wind factor
                particle.Velocity.Y = MathHelper.Lerp(0, 100, (float)random.NextDouble()); // for rain

                particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.Gray;
                particle.Scale = 1f;
                particle.Life = 2.0f;
            };

            fireworkSystem.SpawnParticle = (ref Particle particle) =>
            {
                particle.Position.X = mouse.Position.X;
                particle.Position.Y = mouse.Position.Y;
                float angle = MathHelper.Lerp(0, MathHelper.TwoPi, (float)random.NextDouble());
                Vector2 v = new Vector2(50, 0);
                particle.Velocity = Vector2.Transform(v, Matrix.CreateRotationZ(angle));

                particle.Acceleration = new Vector2(0, -0.1f);
                particle.Color = Color.Purple;
                particle.Scale = 0.015f;
                particle.Life = 1.0f;
            };

            // Set the UpdateParticle method for fire
            fireSystem.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Life -= deltaT;
            };

            snowSystem.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Life -= deltaT;

                if (particle.Position.Y > 700)
                {
                    particle.Position.Y = 0; // for rain and snow
                }
            };

            rainSystem.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT; //for rain
                particle.Life -= deltaT;

                if (particle.Position.Y > 700)
                {
                    particle.Position.Y = 0; // for rain and snow
                }
            };

            fireworkSystem.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };
        }

        public void DrawParticles()
        {
            bool raining = false;

            //Particle Systems
            if (newState.IsKeyDown(Keys.R))
            {
                rainSystem.Draw();
                raining = true;
                MediaPlayer.IsMuted = false;

            }
            else if (!newState.IsKeyDown(Keys.R))
            {
                MediaPlayer.IsMuted = true;
            }

            if (newState.IsKeyDown(Keys.B))
            {
                snowSystem.Draw();
            }
            else if (newState.IsKeyDown(Keys.F))
            {
                fireworkSystem.Draw();
                crack.Play();
            }

            if (raining == false)
            {
                fireSystem.Draw(camera.Matrix);
            }
            else if (raining)
            {
                fireTexture.Dispose();
                
            }
        }


       
    }
}
