using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

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

        public static Texture2D CreateCirlceTexture(GraphicsDevice graphicsDevice, int radius, Color color)
        {
            // Create texture
            var size = radius * 2 + 2;
            var texture = new Texture2D(graphicsDevice, size, size);

            // Create color array with default transparent color
            var colorData = Enumerable.Repeat(Color.Transparent, size * size).ToArray();

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                colorData[y * size + x + 1] = color;
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
