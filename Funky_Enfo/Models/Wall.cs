using Lillheaton.Monogame.Steering;
using Microsoft.Xna.Framework;

namespace FunkyEnfo.Models
{
    public class Wall : IRectangleObstacle
    {
        public Vector3 Center { get; set; }
        public Rectangle Rectangle{get; set; }

        public Wall(Rectangle rectangle)
        {
            var size = rectangle.Width / 2;

            this.Rectangle = new Rectangle(rectangle.X - size, rectangle.Y - size, rectangle.Width, rectangle.Height);
            this.Center = new Vector3(this.Rectangle.X + size, this.Rectangle.Y + size, 0);
        }
    }
}