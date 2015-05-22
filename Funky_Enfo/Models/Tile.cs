using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FunkyEnfo.Models
{
    public class Tile
    {
        private Texture2D texture;
        private float rotation;
        private Rectangle rectangle;
        private Rectangle sourceRectangle;
        private Vector2 origin;

        public Tile(Rectangle position, Rectangle sourceRectangle, Texture2D texture, float rotation)
        {
            var size = position.Width;

            this.texture = texture;
            this.rotation = rotation;
            this.rectangle = position;
            this.sourceRectangle = sourceRectangle;
            this.origin = new Vector2(size / (float)2, size / (float)2);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.texture, null, this.rectangle, this.sourceRectangle, origin, rotation, null, Color.White);
        }
    }
}