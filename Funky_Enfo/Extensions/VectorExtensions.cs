using Microsoft.Xna.Framework;

namespace FunkyEnfo.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 ToVec2(this Vector3 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }
    }
}
