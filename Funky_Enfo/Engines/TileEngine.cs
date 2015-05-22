using FunkyEnfo.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace FunkyEnfo.Engines
{
    public class TileEngine
    {
        public Tile[,] Tiles { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        private Spritesheet2D spritesheet;

        public TileEngine(AssetsManager assets)
        {
            this.spritesheet = assets.Texture2Ds["Tiles"];
            this.GenerateMap();
        }

        private void GenerateMap()
        {
            var lines = AssetsManager.ReadFile("Content/Map.txt");
            this.Width = lines[0].Length / 2;
            this.Height = lines.Length;
            this.Tiles = new Tile[Width,Height];

            for (int i = 0; i < lines.Length; i++)
            {
                var chunks = Enumerable.Range(0, lines[i].Length / 2).Select(s => lines[i].Substring(s * 2, 2)).ToArray();
                for (int j = 0; j < chunks.Length; j++)
                {
                    var size = (int)spritesheet.SpriteSize;
                    var x = (int)spritesheet.SpriteSize / 2 + (int)(j * spritesheet.SpriteSize);
                    var y = (int)spritesheet.SpriteSize / 2 + (int)(i * spritesheet.SpriteSize);
                    var positionRectangle = new Rectangle(x, y, size, size);

                    var spritePositionX = (int)spritesheet.SpritePosition[this.Translate(chunks[j][0])].X;
                    var spritePositionY = (int)spritesheet.SpritePosition[this.Translate(chunks[j][0])].Y;
                    var spritesheetRectangle = new Rectangle(spritePositionX, spritePositionY, size, size);

                    Tiles[j, i] = new Tile(positionRectangle, spritesheetRectangle, spritesheet.Texture, (float)Math.PI * int.Parse(chunks[j][1].ToString()) / 2);
                }
            }
        }

        private string Translate(char ch)
        {
            switch (ch)
            {
                case 'A':
                    return "Cliff";
                case 'B':
                    return "CliffCornerNE";
                case 'C':
                    return "CliffCornerNW";
                case 'D':
                    return "CliffCornerSE1";
                case 'E':
                    return "CliffCornerSE2";
                case 'F':
                    return "CliffCornerSW1";
                case 'G':
                    return "CliffCornerSW2";
                case 'H':
                    return "CliffE";
                case 'I':
                    return "CliffS";
                case 'J':
                    return "DarkMarble";
                case 'K':
                    return "Dirt";
                case 'L':
                    return "Grass";
                case 'M':
                    return "LightMarble";
                default:
                    return "Cliff";
            }
        }

        
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var tile in Tiles)
            {
                tile.Draw(spriteBatch, gameTime);
            }
        }
    }
}