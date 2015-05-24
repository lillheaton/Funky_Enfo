using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FunkyEnfo
{
    public class PrimitivesHelper
    {
        public static void DrawLine(Texture2D texture, SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int thickness)
        {
            var diff = end - start;
            float angle = (float) Math.Atan2(diff.Y, diff.X);

            var rect = new Rectangle((int) start.X, (int) start.Y, (int) diff.Length(), thickness);

            spriteBatch.Draw(texture, rect, null, color, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
