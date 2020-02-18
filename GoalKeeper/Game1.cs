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
        SpriteFont font;

        //Using flyweight pattern to use two instances of player and paddle
        Ball ball;
        Paddle paddle;
        Paddle enemyPaddle;
        Player player;
        Player enemy;

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
        SoundEffect paddleHit;

        // For background start and end page
        Texture2D backgroundStart;
        Texture2D backgroundEnd;

        bool mute = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            paddle = new Paddle(this, new Vector2(0, graphics.PreferredBackBufferHeight / 2));
            enemyPaddle = new Paddle(this, new Vector2(992, graphics.PreferredBackBufferHeight / 2));
            ball = new Ball(this);
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

            player.Initialize();
            enemy.Initialize();
            ball.Initialize();
            paddle.Initialize();
            enemyPaddle.Initialize();
            graphics.ApplyChanges();

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
            enemyPaddle.LoadContent(Content);
            player.LoadContent(Content);
            enemy.LoadContent(Content);
            font = Content.Load<SpriteFont>("DefaultFont");
            // The image before starting the game
            backgroundStart = Content.Load<Texture2D>("start");
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
            MuteSound(newState, oldState);

            if (newState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //For different keys for each player
            Keys[] keylist1 = { Keys.W, Keys.A, Keys.D, Keys.S };
            Keys[] keylist2 = { Keys.Up, Keys.Left, Keys.Right, Keys.Down };

            ball.Update(gameTime);
            player.Update(gameTime, keylist1);
            enemy.Update(gameTime, keylist2);

            /*
            // Bounce off the player 1 board
            if (CollisionDetected(paddle.Bounds, ball.Bounds))
            {
                player.lives--;
                enemy.score++;
                if (player.lives > 0)
                    loseLife.Play();
                else if (player.lives == 0)
                {
                    gameOver.Play();
                    endGame = true;
                }

                ball.Velocity.X *= -1;
                var bounce = (paddle.Bounds.X + paddle.Bounds.Width) - (ball.Bounds.X - ball.Bounds.Radius);
                ball.Bounds.X += 2 * bounce;

            }

            
            if(CollisionDetected(enemyPaddle.Bounds, ball.Bounds))
            {
                enemy.lives--;
                player.score++;
                if (enemy.lives > 0)
                    loseLife.Play();
                else if (enemy.lives == 0)
                {
                    gameOver.Play();
                    endGame = true;
                }

                ball.Velocity.X *= -1;
                var bounce = (enemyPaddle.Bounds.X + enemyPaddle.Bounds.Width) - (ball.Bounds.X - ball.Bounds.Radius);
                ball.Bounds.X += 2 * bounce;
            }
            

             if (CollisionDetected(player.Bounds, ball.Bounds))
             {
                 paddleHit.Play();
                 ball.Velocity.X *= -1;
                 var bounce = (player.Bounds.X + player.Bounds.Width) - (ball.Bounds.X - ball.Bounds.Radius);
                 ball.Bounds.X += 2 * bounce;
             }

             if(CollisionDetected(enemy.Bounds, ball.Bounds))
             {
                 paddleHit.Play();
                 ball.Velocity.X *= -1;
                 var bounce = (enemy.Bounds.X + enemy.Bounds.Width) - (ball.Bounds.X - ball.Bounds.Radius);
                 ball.Bounds.X += 2 * bounce;
             } */


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

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (!beginGame)
            {
                // Fill the screen with black before the game starts
                spriteBatch.Draw(backgroundStart, new Rectangle(0, 0,
                (int)graphics.PreferredBackBufferWidth, (int)graphics.PreferredBackBufferHeight), Color.White);

            }
            else if (player.lives <= 0 || enemy.lives <= 0)
            {
                // Why isn't this working??
                if(enemy.score == 5)
                {
                    spriteBatch.DrawString(font, "Player 2 WINS!", new Vector2(425, 0), Color.White);
                }
                else if(player.score == 5)
                {
                    spriteBatch.DrawString(font, "Player 1 WINS!", new Vector2(425, 0), Color.White);
                }
                spriteBatch.Draw(backgroundEnd, new Rectangle(0, 0,
                (int)graphics.PreferredBackBufferWidth, (int)graphics.PreferredBackBufferHeight), Color.White);
            }
            else
            {
                ball.Draw(spriteBatch);
                paddle.Draw(spriteBatch);
                enemyPaddle.Draw(spriteBatch);
                player.Draw(spriteBatch);
                enemy.Draw(spriteBatch);

                spriteBatch.DrawString(font, "Player 1 Score: " + Convert.ToString(player.score), player.position, Color.White);
                spriteBatch.DrawString(font, "Player 2 Score: " + Convert.ToString(enemy.score), enemy.position, Color.White);


                if(mute == false)
                {
                    spriteBatch.DrawString(font, "Press M to mute", new Vector2(425, 0), Color.White);
                }
                if(mute == true)
                {
                    spriteBatch.DrawString(font, "Press M to unmute", new Vector2(420, 0), Color.White);
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
            // Exit the keyboard handler method early if not.
            if (!beginGame)
            {
                ball.Velocity = Vector2.Zero;
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    ball.Initialize();
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
                    ball.Initialize();
                    ball.Velocity.Normalize();
                    player.score = 0;
                    enemy.score = 0;
                    player.lives = 5;
                    enemy.lives = 5;
                    endGame = false;
                    mute = false;
                    SoundEffect.MasterVolume = 1.0f;
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

        public bool CollisionDetected(BoundingRectangle r, BoundingCircle c)
        {
            var closestX = Math.Max(r.X, Math.Min(c.X, r.X + r.Width));
            var closestY = Math.Max(r.Y, Math.Min(c.Y, r.Y + r.Height));
            return (Math.Pow(c.Radius, 2) >= Math.Pow(closestX - c.X, 2) + Math.Pow(closestY - c.Y, 2));
        }
    }
}
