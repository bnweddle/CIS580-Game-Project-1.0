using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleSystemStarter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoalKeeper
{
    public class Fireworks
    {

        ParticleSystem fireworkSystem;

        Color Color;

        Game1 Game;

        Stopwatch timer = null;

        TimeSpan duration = TimeSpan.FromSeconds(1.0);

        MouseState mouse = Mouse.GetState();

        SoundEffect crack;

        Random random = new Random();



        public Fireworks(Game1 game, Color color)
        {
            game = Game;
            color = Color;
        }

        public void LoadContent(ContentManager Content, Texture2D texture)
        {

            fireworkSystem = new ParticleSystem(Game.GraphicsDevice, 1000, texture);

            fireworkSystem.SpawnPerFrame = 4;

            fireworkSystem.SpawnParticle = (ref Particle particle) =>
            {
                if(mouse.LeftButton == ButtonState.Pressed)
                {
                    timer = new Stopwatch();
                    timer.Start();
                }
                
                if(timer.Elapsed > duration && timer != null)
                {
                    particle.Life = 0f;
                    timer.Restart();
                    timer.Stop();
                }                                
                else if(timer != null)
                {
                    particle.Position.X = mouse.Position.X;
                    particle.Position.Y = mouse.Position.Y;
                    float angle = MathHelper.Lerp(0, MathHelper.TwoPi, (float)random.NextDouble());
                    Vector2 v = new Vector2(50, 0);
                    particle.Velocity = Vector2.Transform(v, Matrix.CreateRotationZ(angle));

                    particle.Acceleration = new Vector2(0, -0.1f);
                    particle.Color = Color;
                    particle.Scale = 0.015f;
                    particle.Life = 1.0f;
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

        public void Update(GameTime gameTime, SoundEffect crack)
        {
            fireworkSystem.Update(gameTime);
            if(mouse.LeftButton == ButtonState.Pressed)
            {
                if(crack != null)
                    crack.Play();
            }
        }

        public void Draw()
        {
            fireworkSystem.Draw();
        }


        









    }
}
