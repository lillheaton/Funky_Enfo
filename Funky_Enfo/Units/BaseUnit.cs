using FunkyEnfo.Extensions;
using FunkyEnfo.Map;
using FunkyEnfo.Models;
using FunkyEnfo.Screens;
using Lillheaton.Monogame.Steering;
using Lillheaton.Monogame.Steering.Behaviours;
using Lillheaton.Monogame.Steering.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
        public bool DrawForces { get; set; }

        protected Enfo Screen { get; private set; }
        protected Spritesheet2D CurrentSpritesheet { get; set; }
        protected Rectangle SourceRectangle { get; set; }
        protected float Rotate { get; set; }
        protected TimeSpan SpriteUpdatePerMilliseconds { get; set; }
        
        private Vector2 origin;
        private TimeSpan lastUpdateTime;
        private int currentSpritePosition;

        protected BaseUnit(Spritesheet2D currentSpritesheet, Enfo screen)
        {
            Velocity = new Vector3(-1, -2, 0);
            Velocity = Velocity.Truncate(this.GetMaxVelocity());

            this.SteeringBehavior = new SteeringBehavior(this);
            this.Screen = screen;
            this.Position2D = new Vector2();
            this.TargetPosition = new Vector2();
            this.SpriteUpdatePerMilliseconds = TimeSpan.FromMilliseconds(100);
            this.currentSpritePosition = 0;
            this.Direction = Direction.South;

            // Init spritesheet stuff
            this.CurrentSpritesheet = currentSpritesheet;
            this.SourceRectangle = new Rectangle(0, 0, (int)currentSpritesheet.SpriteSize, (int)currentSpritesheet.SpriteSize);
            this.origin = new Vector2(this.CurrentSpritesheet.SpriteSize / 2f, currentSpritesheet.SpriteSize / 2f);
        }

        public virtual float GetMass()
        {
            return 20f;
        }

        public virtual float GetMaxVelocity()
        {
            return 3f;
        }

        public virtual void Update(GameTime gameTime)
        {
            this.Direction = CalculateDirection();
            HandleAnimationUpdate(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.CurrentSpritesheet.Texture, this.Position2D, SourceRectangle, Color.White, Rotate, origin, 1, SpriteEffects.None, 0f);
            if (DrawForces)
                Forces(spriteBatch);
        }






        protected bool ClearViewTo(IBoid boid)
        {
            return MapHelper.ClearViewFrom(this.Position2D, boid.Position.ToVec2(), this.Screen.TileEngine.Obstacles);
        }





        private void HandleAnimationUpdate(GameTime gameTime)
        {
            lastUpdateTime += gameTime.ElapsedGameTime;
            if (lastUpdateTime > SpriteUpdatePerMilliseconds)
            {
                lastUpdateTime -= SpriteUpdatePerMilliseconds;
                this.currentSpritePosition++;
                var spritePosition = CurrentSpritesheet.SpritePosition[Direction.ToString() + currentSpritePosition % this.CurrentSpritesheet.PerAnimation];
                this.SourceRectangle = new Rectangle(spritePosition.ToPoint(), new Point(this.SourceRectangle.Width, this.SourceRectangle.Height));
            }
        }

        private void Forces(SpriteBatch spriteBatch)
        {
            const int Scale = 100;
            var drawVec = this.Position2D;
            var velocityForce = Vector2.Normalize(new Vector2(Velocity.X, Velocity.Y));
            var steeringForce = Vector2.Normalize(new Vector2(this.SteeringBehavior.Steering.X, this.SteeringBehavior.Steering.Y));
            var desiredVelocityForce = Vector2.Normalize(new Vector2(this.SteeringBehavior.DesiredVelocity.X, this.SteeringBehavior.DesiredVelocity.Y));

            PrimitivesHelper.DrawLine(this.Screen.Assets.Textures["1x1Texture"], spriteBatch, drawVec,
                drawVec + velocityForce * Scale, Color.Green, 2);

            PrimitivesHelper.DrawLine(this.Screen.Assets.Textures["1x1Texture"], spriteBatch, drawVec,
                drawVec + desiredVelocityForce * Scale, Color.Gray, 2);

            PrimitivesHelper.DrawLine(this.Screen.Assets.Textures["1x1Texture"], spriteBatch, drawVec,
                drawVec + steeringForce * Scale, Color.Red, 2);
        }

        private Direction CalculateDirection()
        {
            float angle = MathHelper.ToDegrees(this.SteeringBehavior.Angle);
            int direction = ((int)((angle + 22.5f) / 45.0f)) & 7;

            return (Direction)direction;
        }
    }
}