using FunkyEnfo.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FunkyEnfo.Models
{
    public class Tile
    {
        public bool IsWalkable { get; set; }
        public Rectangle Rectangle { get; private set; }
        public Vector2 Center { get; private set; }
        public Color Color { get; set; }

        private Texture2D texture;
        private float rotation;
        private Rectangle destinationRectangle;
        private Rectangle sourceRectangle;
        private Vector2 origin;

        public Tile(Rectangle position, Rectangle sourceRectangle, Texture2D texture, float rotation)
        {
            this.IsWalkable = true;
            var size = position.Width;
            this.Center = position.ToVector2() + new Vector2(size / 2f, size / 2f);
            this.Rectangle = position;

            this.destinationRectangle = new Rectangle(position.X + size / 2, position.Y + size / 2, size, size);
            this.texture = texture;
            this.rotation = rotation;
            this.sourceRectangle = sourceRectangle;
            this.origin = new Vector2(size / 2f, size / 2f);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.texture, null, this.destinationRectangle, this.sourceRectangle, this.origin, rotation, null, Color.White);
        }
    }
}