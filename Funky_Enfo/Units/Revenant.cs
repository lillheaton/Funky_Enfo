using FunkyEnfo.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FunkyEnfo.Units
{
    public class Revenant : BaseUnit
    {
        private Camera camera;
        private Dictionary<string, Spritesheet2D> spritesheets;
        private MouseState oldMouseState;
        
        public Revenant(Camera camera, Dictionary<string, Spritesheet2D> spritesheets) : base(spritesheets["Revenant_Move"])
        {
            this.camera = camera;
            this.spritesheets = spritesheets;
        }

        public override void Update(GameTime gameTime)
        {
            this.GetMouseInput();
            this.Direction = CalculateDirection();

            this.SteeringBehavior.Arrive(new Vector3(TargetPosition, 0));
            this.SteeringBehavior.Update(gameTime);
        }

        private void GetMouseInput()
        {
            var newMouseState = Mouse.GetState();

            if (newMouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed)
            {
                this.TargetPosition = camera.ScreenToWorld(newMouseState.Position.ToVector2());
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