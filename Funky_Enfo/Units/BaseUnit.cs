using System;

using FunkyEnfo.Models;
using Lillheaton.Monogame.Steering;
using Lillheaton.Monogame.Steering.Behaviours;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FunkyEnfo.Units
{
    public abstract class BaseUnit : IBoid
    {
        public Direction Direction { get; protected set; }

        public SteeringBehavior SteeringBehavior { get; private set; }
        public Vector3 Position { get { return new Vector3(this.Position2D, 0); } set { this.Position2D = new Vector2(value.X, value.Y); } }
        public Vector2 Position2D { get; set; }
        public Vector2 TargetPosition { get; set; }
        public Vector3 Velocity { get; set; }

        protected Spritesheet2D CurrentSpritesheet { get; private set; }
        protected Rectangle SourceRectangle { get; set; }
        protected float Rotate { get; set; }
        protected TimeSpan SpriteUpdatePerMilliseconds { get; set; }

        private Vector2 origin;
        private TimeSpan lastUpdateTime;
        private int currentSpritePosition;

        protected BaseUnit(Spritesheet2D currentSpritesheet)
        {
            this.SteeringBehavior = new SteeringBehavior(this);
            this.Position2D = new Vector2();
            this.TargetPosition = new Vector2();
            this.SetCurrentSpritesheet(currentSpritesheet);
            this.SpriteUpdatePerMilliseconds = TimeSpan.FromMilliseconds(100);
            this.currentSpritePosition = 0;
            this.Direction = Direction.South;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            lastUpdateTime += gameTime.ElapsedGameTime;
            if (lastUpdateTime > SpriteUpdatePerMilliseconds)
            {
                lastUpdateTime -= SpriteUpdatePerMilliseconds;
                this.currentSpritePosition++;
                var spritePosition = CurrentSpritesheet.SpritePosition[Direction.ToString() + currentSpritePosition % this.CurrentSpritesheet.PerAnimation];
                this.SourceRectangle = new Rectangle(spritePosition.ToPoint(), new Point(this.SourceRectangle.Width, this.SourceRectangle.Height));
            }

            spriteBatch.Draw(this.CurrentSpritesheet.Texture, this.Position2D, SourceRectangle, Color.White, Rotate, origin, 1, SpriteEffects.None, 0f);
        }

        public void SetCurrentSpritesheet(Spritesheet2D spritesheet)
        {
            this.CurrentSpritesheet = spritesheet;
            this.SourceRectangle = new Rectangle(0, 0, (int)spritesheet.SpriteSize, (int)spritesheet.SpriteSize);
            this.origin = new Vector2(this.CurrentSpritesheet.SpriteSize / 2f, spritesheet.SpriteSize / 2f);
        }

        public virtual float GetMass()
        {
            return 20f;
        }

        public virtual float GetMaxVelocity()
        {
            return 3f;
        }
    }
}