using Microsoft.Xna.Framework;

namespace FunkyEnfo
{
    public class VecDirection
    {
        public static Vector2 SouthEast = new Vector2(1, 1);
        public static Vector2 NorthWest = -SouthEast;
        public static Vector2 NorthEast = new Vector2(1, -1);
        public static Vector2 SouthWest = -NorthEast;

        public static Vector2 North = new Vector2(0, -1);
        public static Vector2 South = -North;
        public static Vector2 East = new Vector2(1, 0);
        public static Vector2 West = -East;
    }
}
