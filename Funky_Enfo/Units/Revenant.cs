using System;

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

            //var direction = this.Position2D - this.TargetPosition;
            //direction.Normalize();

            //var rotation = Math.Atan2(direction.X, direction.Y);

            //var data = MathHelper.PiOver4 * (float)Math.Round(45 / MathHelper.PiOver4);

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
    }
}