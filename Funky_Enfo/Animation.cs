using FunkyEnfo.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FunkyEnfo
{
    public class Animation
    {
        public Spritesheet2D Spritesheet { get; set; }
        public TimeSpan SpriteUpdatePerMilliseconds { get; set; }
        public float SpriteSize { get { return Spritesheet.SpriteSize; } }
        public int CurrentSpritePosition { get; private set; }

        private Rectangle sourceRectangle;
        private Vector2 origin;
        private TimeSpan lastUpdateTime;

        public Animation(Spritesheet2D spritesheet, TimeSpan spriteUpdatePerMilliseconds)
        {
            this.Spritesheet = spritesheet;

            this.SpriteUpdatePerMilliseconds = spriteUpdatePerMilliseconds;
            this.CurrentSpritePosition = 0;

            this.sourceRectangle = new Rectangle(0, 0, (int)Spritesheet.SpriteSize, (int)Spritesheet.SpriteSize);
            this.origin = new Vector2(this.Spritesheet.SpriteSize / 2f, Spritesheet.SpriteSize / 2f);
        }


        public void Update(GameTime gameTime, string spritePositionName = null)
        {
            lastUpdateTime += gameTime.ElapsedGameTime;
            if (lastUpdateTime > SpriteUpdatePerMilliseconds)
            {
                lastUpdateTime -= SpriteUpdatePerMilliseconds;
                this.CurrentSpritePosition++;
                var spritePosition = Spritesheet.SpritePosition[spritePositionName + this.CurrentSpritePosition % this.Spritesheet.PerAnimation];
                this.sourceRectangle = new Rectangle(spritePosition.ToPoint(), new Point(this.sourceRectangle.Width, this.sourceRectangle.Height));
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {            
            spriteBatch.Draw(this.Spritesheet.Texture, position, sourceRectangle, Color.White, 0f, origin, 1, SpriteEffects.None, 0f);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 scale)
        {
            spriteBatch.Draw(this.Spritesheet.Texture, position, sourceRectangle, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 scale, float rotation)
        {
            spriteBatch.Draw(this.Spritesheet.Texture, position, sourceRectangle, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle rectangle, float rotation = 0f)
        {
            spriteBatch.Draw(this.Spritesheet.Texture, null, rectangle, sourceRectangle, null, rotation, null, null);
        }
    }
}
