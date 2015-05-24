using System;
using FunkyEnfo.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FunkyEnfo.Units
{
    public class Revenant : BaseUnit
    {
        private MouseState oldMouseState;
        
        public Revenant(Vector2 position, Enfo enfo) : base(enfo.Assets.Spritesheets["Revenant_Move"], enfo)
        {
            this.Position2D = position;
            this.TargetPosition = position;
            this.DrawForces = true;
        }

        public override void Update(GameTime gameTime)
        {
            this.GetMouseInput();
            this.Direction = CalculateDirection();

            this.SteeringBehavior.Seek(new Vector3(TargetPosition, 0));
            this.SteeringBehavior.CollisionAvoidance(this.Screen.TileEngine.Obstacles);
            this.SteeringBehavior.Update(gameTime);

            //Console.WriteLine("Boid position: {0} ----- First obstacle {1} ", this.Position2D, this.Screen.TileEngine.Obstacles[0].Center);
            Console.WriteLine("Mouse position: {0}", Mouse.GetState().Position);
        }

        private void GetMouseInput()
        {
            var newMouseState = Mouse.GetState();

            if (newMouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed)
            {
                this.TargetPosition = this.Screen.Camera.ScreenToWorld(newMouseState.Position.ToVector2());
            }

            oldMouseState = newMouseState;
        }

        private Direction CalculateDirection()
        {
            float angle = MathHelper.ToDegrees(this.SteeringBehavior.Angle);
            int direction = ((int)((angle + 22.5f) / 45.0f)) & 7;

            return (Direction) direction;
        }
    }
}