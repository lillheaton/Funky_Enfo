using FunkyEnfo.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FunkyEnfo.Units
{
    public abstract class BaseUnit
    {
        public Vector2 Position { get; set; }
        public Vector2 TargetPosition { get; set; }

        protected Spritesheet2D CurrentSpritesheet { get; private set; }
        protected Rectangle SourceRectangle { get; set; }
        protected float Rotate { get; set; }

        private Vector2 origin;

        protected BaseUnit(Spritesheet2D currentSpritesheet)
        {
            this.Position = new Vector2();
            this.TargetPosition = new Vector2();
            this.SetCurrentSpritesheet(currentSpritesheet);
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.CurrentSpritesheet.Texture, Position, SourceRectangle, Color.White, Rotate, origin, 1, SpriteEffects.None, 0f);
        }

        public void SetCurrentSpritesheet(Spritesheet2D spritesheet)
        {
            this.CurrentSpritesheet = spritesheet;
            this.SourceRectangle = new Rectangle(0, 0, (int)spritesheet.SpriteSize, (int)spritesheet.SpriteSize);
            this.origin = new Vector2(this.CurrentSpritesheet.SpriteSize / 2f, spritesheet.SpriteSize / 2f);
        }
    }
}