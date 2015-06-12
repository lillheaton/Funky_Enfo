using FunkyEnfo.Extensions;
using FunkyEnfo.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunkyEnfo.Map
{
    public class TileEngine
    {
        public Tile[][] Tiles { get; private set; }
        public Wall[] Obstacles { get; private set; }
        public List<Waypoint> Waypoints { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileSize { get; private set; }

        private Spritesheet2D spritesheet;
        private AssetsManager assets;
        private bool drawWaypoints;

        public TileEngine(AssetsManager assets)
        {
            this.assets = assets;
            this.spritesheet = assets.Spritesheets["Tiles"];
            this.LoadMap();
            this.CalculateWaypoints();
        }

        private void LoadMap()
        {
            var lines = AssetsManager.ReadFile("Content/Map.txt");
            this.Width = lines[0].Length / 2;
            this.Height = lines.Length;
            this.Tiles = new Tile[this.Width][];
            this.TileSize = (int)this.spritesheet.SpriteSize;
            var obstacles = new List<Wall>();

            // Init array
            for (int i = 0; i < this.Tiles.Length; i++)
            {
                this.Tiles[i] = new Tile[this.Height];
            }

            for (int i = 0; i < lines.Length; i++)
            {
                var chunks = Enumerable.Range(0, lines[i].Length / 2).Select(s => lines[i].Substring(s * 2, 2)).ToArray();
                for (int j = 0; j < chunks.Length; j++)
                {
                    var size = (int)this.spritesheet.SpriteSize - 2; // negative 2 = graphic tweek
                    var x = j * size;
                    var y = i * size;
                    var positionRectangle = new Rectangle(x, y, size, size);

                    bool obstacle;
                    var spritePositionName = MapHelper.TranslateMapChar(chunks[j][0], out obstacle);
                    var spritePositionX = (int)this.spritesheet.SpritePosition[spritePositionName].X + 1; // add 1 = graphic tweek
                    var spritePositionY = (int)this.spritesheet.SpritePosition[spritePositionName].Y + 1; // add 1 = graphic tweek
                    var spritesheetRectangle = new Rectangle(spritePositionX, spritePositionY, size , size);

                    this.Tiles[j][i] = new Tile(positionRectangle, spritesheetRectangle, this.spritesheet.Texture, (float)Math.PI * int.Parse(chunks[j][1].ToString()) / 2);
                    this.Tiles[j][i].Color = MapHelper.GetTileColor(spritePositionName);

                    if (obstacle)
                    {
                        this.Tiles[j][i].IsWalkable = false;
                        obstacles.Add(new Wall(positionRectangle));
                    }
                }
            }

            this.Obstacles = obstacles.ToArray();
        }

        private void CalculateWaypoints()
        {
            Waypoints = new List<Waypoint>();

            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Height; j++)
                {
                    var arrayPosition = new Vector2(i, j);

                    if (Tiles[i][j].IsWalkable)
                    {
                        continue;
                    }

                    if (Tiles.GetNorthEastNeighbours(arrayPosition).Count(s => s != null && s.IsWalkable) == 3)
                        Waypoints.Add(new Waypoint { Position = Tiles.GetValue((arrayPosition + new Vector2(1, -1)).ToPoint()).Center });

                    if (Tiles.GetNorthWestNeighbours(arrayPosition).Count(s => s != null && s.IsWalkable) == 3)
                        Waypoints.Add(new Waypoint { Position = Tiles.GetValue((arrayPosition + new Vector2(-1, -1)).ToPoint()).Center });

                    if (Tiles.GetSouthEastNeighbours(arrayPosition).Count(s => s != null && s.IsWalkable) == 3)
                        Waypoints.Add(new Waypoint { Position = Tiles.GetValue((arrayPosition + new Vector2(1, 1)).ToPoint()).Center });

                    if (Tiles.GetSouthWestNeighbours(arrayPosition).Count(s => s != null && s.IsWalkable) == 3)
                        Waypoints.Add(new Waypoint { Position = Tiles.GetValue((arrayPosition + new Vector2(-1, 1)).ToPoint()).Center });
                }
            }

            // Loop through every waypoint
            foreach (var waypoint in Waypoints)
            {
                CalculateRelatedWaypoints(waypoint);
            }
        }




        public void ToggleShowWaypoints()
        {
            this.drawWaypoints = !this.drawWaypoints;
        }

        public void CalculateRelatedWaypoints(Waypoint calculatingWaypoint)
        {
            // Loop through all again to see if any waypoint is visible to each other
            foreach (var nestedWaypoint in this.Waypoints.Where(s => s != calculatingWaypoint))
            {
                if (MapHelper.ClearViewFrom(calculatingWaypoint.Position, nestedWaypoint.Position, this.Obstacles))
                {
                    calculatingWaypoint.RelatedPoints.Add(nestedWaypoint);
                }
            }
        }

        public Tile TileAt(Vector2 position)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (Tiles[i][j].Rectangle.Contains(position))
                    {
                        return Tiles[i][j];
                    }
                }
            }

            return null;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < this.Tiles.Length; i++)
            {
                for (int j = 0; j < this.Tiles[i].Length; j++)
                {
                    this.Tiles[i][j].Draw(spriteBatch, gameTime);
                }
            }

            if (drawWaypoints)
            {
                foreach (var waypoint in Waypoints)
                {
                    foreach (var relatedPoint in waypoint.RelatedPoints)
                    {
                        PrimitivesHelper.DrawLine(assets.Textures["1x1Texture"], spriteBatch, waypoint.Position, relatedPoint.Position, Color.Red, 1);
                    }
                }
            }
        }
    }
}