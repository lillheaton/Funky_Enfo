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
        public float UnitRadius { get { return this.UnitAnimation.SpriteSize / 2; } }
        public bool DrawHealthBar { get; set; }

        public abstract int MaxHealth { get; set; }
        public abstract int CurrentHealth { get; set; }
        public bool IsDead { get { return CurrentHealth <= 0; } }

        protected GameScreen Screen { get; private set; }
        protected Animation UnitAnimation { get; private set; }
        protected float Rotate { get; set; }

        private Vector2 circlePosition;
        private Vector2 healtBarPosition;        

        protected BaseUnit(Spritesheet2D currentSpritesheet, GameScreen screen)
        {
            Velocity = new Vector3(-1, -2, 0);
            Velocity = Velocity.Truncate(this.GetMaxVelocity());

            this.SteeringBehavior = new SteeringBehavior(this);
            this.Screen = screen;
            this.Position2D = new Vector2();
            this.TargetPosition = new Vector2();
            this.UnitAnimation = new Animation(currentSpritesheet, TimeSpan.FromMilliseconds(70));            
            this.Direction = Direction.South;
            
            // Init spritesheet stuff            
            this.circlePosition = new Vector2(-(this.UnitAnimation.SpriteSize / 5), (this.UnitAnimation.SpriteSize / 4));
            this.healtBarPosition = new Vector2(-25, -(this.UnitAnimation.SpriteSize / 1.8f));
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
            this.UnitAnimation.Update(gameTime, Direction.ToString());
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.Screen.Assets.Textures["Circle_Green_18"], this.Position2D + this.circlePosition, Color.Green);
            this.UnitAnimation.Draw(spriteBatch, this.Position2D);

            if (DrawHealthBar)
                spriteBatch.Draw(this.Screen.Assets.Textures["1x1Texture"], this.Position2D + this.healtBarPosition, null, null, null, 0f, GetHealthBarScaleVectr(), Color.GreenYellow);

            if (DrawForces)
                Forces(spriteBatch);
        }






        protected bool ClearViewTo(IBoid boid)
        {
            return MapHelper.ClearViewFrom(this.Position2D, boid.Position.ToVec2(), this.Screen.TileEngine.Obstacles);
        }

        protected bool InRange(Vector2 vec, float range)
        {
            return Vector2.Distance(this.Position2D, vec) < range;
        }


        private Vector2 GetHealthBarScaleVectr()
        {
            const int HealtBarSize = 50;

            var percentage = this.CurrentHealth / (float)this.MaxHealth;
            return new Vector2(HealtBarSize * percentage, 6);
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