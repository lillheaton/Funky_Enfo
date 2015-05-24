using System.Collections.Generic;
using FunkyEnfo.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Lillheaton.Monogame.Steering;

namespace FunkyEnfo.Engines
{
    public class TileEngine
    {
        public Tile[,] Tiles { get; private set; }
        public Wall[] Obstacles { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        private Spritesheet2D spritesheet;

        public TileEngine(AssetsManager assets)
        {
            this.spritesheet = assets.Spritesheets["Tiles"];
            this.GenerateMap();
        }

        private void GenerateMap()
        {
            var lines = AssetsManager.ReadFile("Content/Map.txt");
            this.Width = lines[0].Length / 2;
            this.Height = lines.Length;
            this.Tiles = new Tile[Width,Height];
            var obstacles = new List<Wall>();

            for (int i = 0; i < lines.Length; i++)
            {
                var chunks = Enumerable.Range(0, lines[i].Length / 2).Select(s => lines[i].Substring(s * 2, 2)).ToArray();
                for (int j = 0; j < chunks.Length; j++)
                {
                    var size = (int)spritesheet.SpriteSize;
                    var x = (int)(j * spritesheet.SpriteSize);
                    var y = (int)(i * spritesheet.SpriteSize);
                    var positionRectangle = new Rectangle(x, y, size, size);

                    bool obstacle;
                    var spritePositionName = this.Translate(chunks[j][0], out obstacle);
                    var spritePositionX = (int)spritesheet.SpritePosition[spritePositionName].X;
                    var spritePositionY = (int)spritesheet.SpritePosition[spritePositionName].Y;
                    var spritesheetRectangle = new Rectangle(spritePositionX, spritePositionY, size, size);

                    if(obstacle)
                        obstacles.Add(new Wall(positionRectangle));

                    Tiles[j, i] = new Tile(positionRectangle, spritesheetRectangle, spritesheet.Texture, (float)Math.PI * int.Parse(chunks[j][1].ToString()) / 2);
                }
            }

            this.Obstacles = obstacles.ToArray();
        }

        private string Translate(char ch, out bool obstacle)
        {
            obstacle = false;

            switch (ch)
            {
                case 'A':
                    obstacle = true;
                    return "Cliff";
                case 'B':
                    obstacle = true;
                    return "CliffCornerNE";
                case 'C':
                    obstacle = true;
                    return "CliffCornerNW";
                case 'D':
                    obstacle = true;
                    return "CliffCornerSE1";
                case 'E':
                    obstacle = true;
                    return "CliffCornerSE2";
                case 'F':
                    obstacle = true;
                    return "CliffCornerSW1";
                case 'G':
                    obstacle = true;
                    return "CliffCornerSW2";
                case 'H':
                    obstacle = true;
                    return "CliffE";
                case 'I':
                    obstacle = true;
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
                    obstacle = true;
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