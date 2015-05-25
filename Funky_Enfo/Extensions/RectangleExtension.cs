using Microsoft.Xna.Framework;

namespace FunkyEnfo.Extensions
{
    public static class RectangleExtension
    {
        public static Vector2 ToVector2(this Rectangle rect)
        {
            return new Vector2(rect.X, rect.Y);
        }
    }
}
