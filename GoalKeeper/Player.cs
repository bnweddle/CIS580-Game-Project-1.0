/* Author: Nathan Bean
 * In class work for Sprite Animation, need to use a state machine to track player movement
 * */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GoalKeeper
{ 
    enum State
    {
        North,
        South,
        East,
        West,
        Idle
    }

    public class Player
    {
        Game1 game;
        Texture2D texture;
        State state;
        TimeSpan timer;
        Vector2 position;
        //google spritesheets to use

        public Player(Game1 game)
        {
            this.game = game;
            timer = new TimeSpan(0);
            position = new Vector2(200, 200);
            state = State.Idle;
        }

        public void LoadContent()
        {
            game.Content.Load<Texture2D>("player");
        }

        public void Update(GameTime gameTime)
        {
            if(state != State.Idle)
            {
                timer += gameTime.ElapsedGameTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int x, y;
            switch (state)
            {
                case State.South:
                    y = 0;
                    break;
                case State.West:
                    y = 1;
                    break;
                case State.East:
                    y = 2;
                    break;
                case State.North:
                    y = 3;
                    break;
                case State.Idle:
                default:
                    y = 0;
                    break;
            }

            x = (int)timer.TotalMilliseconds % 62;

            var source = new Rectangle(
                x * 45, 
                y * 62, 
                45,
                62
                );

            spriteBatch.Draw(texture, position, source, Color.White);

        }
    }
}
