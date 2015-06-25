using FunkyEnfo.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace FunkyEnfo
{
    public class GameUi
    {
        private AssetsManager assets;
        private TileEngine tileEngine;
        private UnitManager unitManager;
        private Animation portraitAnimation;
        private Camera camera;

        private Vector2 portraitScale;
        private Vector2 portraitPosition;

        private float footerImageRatio;
        private Rectangle footerRectangle;

        public Texture2D MapTexture;
        public Rectangle MapRectangle;
        private Rectangle mapBackgroundRectanlge;

        public GameUi(AssetsManager assets, TileEngine tileEngine, UnitManager unitManager, Camera camera, int width, int height)
        {
            this.assets = assets;
            this.tileEngine = tileEngine;
            this.unitManager = unitManager;
            this.camera = camera;
            this.portraitAnimation = new Animation(assets.Spritesheets["Revenant_Portrait"], TimeSpan.FromMilliseconds(100));

            this.InitialCalculations(width, height);
        }

        public void Update(GameTime gameTime)
        {
            this.portraitAnimation.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {   
            this.portraitAnimation.Draw(spriteBatch, this.portraitPosition, this.portraitScale);
            spriteBatch.Draw(assets.Textures["Footer"], this.footerRectangle, Color.White);

            this.DrawMap(spriteBatch);
        }




        private void InitialCalculations(int screenWidth, int screenHeight)
        {
            this.CalculateFooterPosition(screenWidth, screenHeight);
            this.CalculatePortraitPosition(screenHeight);
            this.CalculateMapPosition(screenHeight);
        }

        private void CalculatePortraitPosition(int screenHeight)
        {
            const float PortraitSpace = 150f;
            var scaledSpace = PortraitSpace * footerImageRatio;
            var scale = scaledSpace / portraitAnimation.SpriteSize;

            this.portraitScale = new Vector2(scale, scale);
            this.portraitPosition = new Vector2(504 * footerImageRatio, screenHeight - 140 * footerImageRatio);
        }

        private void CalculateFooterPosition(int screenWidth, int screenHeight)
        {
            var footerAssetWidth = assets.Textures["Footer"].Width;
            var footerAssetHeight = assets.Textures["Footer"].Height;

            this.footerImageRatio = screenWidth / (float)footerAssetWidth;
            var footerHeight = (int)(footerAssetHeight * this.footerImageRatio);

            this.footerRectangle = new Rectangle(0, screenHeight - footerHeight, screenWidth, footerHeight);
        }

        private void CalculateMapPosition(int screenHeight)
        {
            const float UiMapSize = 276f;
            var mapSize = UiMapSize * footerImageRatio;

            var mapWidth = tileEngine.Tiles.Length;
            var mapHeight = tileEngine.Tiles[0].Length;

            if (mapWidth > mapHeight)
            {
                float ratio = mapSize / mapWidth;
                this.MapRectangle = new Rectangle(0, 0, (int)mapSize, (int)(mapHeight * ratio));
            }
            else
            {
                float ratio = mapSize / mapHeight;
                this.MapRectangle = new Rectangle(0, 0, (int)(mapWidth * ratio), (int)mapSize);
            }

            this.MapRectangle.X = (int)((mapSize / 2f) - 35 * footerImageRatio);
            this.MapRectangle.Y = (int)(screenHeight - this.MapRectangle.Height - 14 * footerImageRatio);

            mapBackgroundRectanlge = new Rectangle((int)(20 * footerImageRatio), (int)(screenHeight - this.MapRectangle.Height - 14 * footerImageRatio), (int)mapSize, (int)mapSize);
        }




        private void DrawMap(SpriteBatch spriteBatch)
        {
            var scaleMatric = Matrix.CreateScale(1.0f / tileEngine.TileSize);
            var cameraTopLeftVector = Vector2.Transform(camera.ScreenToWorld(new Vector2(0, 0)), scaleMatric);
            var cameraBottomRightVector = Vector2.Transform(camera.ScreenToWorld(new Vector2(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height)), scaleMatric);

            var cameraTop = Convert.ToInt32(cameraTopLeftVector.Y);
            var cameraLeft = Convert.ToInt32(cameraTopLeftVector.X);
            var cameraRight = Convert.ToInt32(cameraBottomRightVector.X);
            var cameraBottom = Convert.ToInt32(cameraBottomRightVector.Y);


            this.MapTexture = this.MapTexture ?? new Texture2D(spriteBatch.GraphicsDevice, tileEngine.Tiles.Length, tileEngine.Tiles[0].Length);
            var colorData = Enumerable.Repeat(Color.Red, (int)(tileEngine.Tiles.Length * tileEngine.Tiles[0].Length)).ToArray();

            var rows = tileEngine.Tiles.Length;
            var columns = tileEngine.Tiles[0].Length;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    // Draw unit if on position
                    bool unitAtPos = false;
                    for (int k = 0; k < unitManager.Units.Count; k++)
                    {
                        if (tileEngine.Tiles[i][j].Rectangle.Contains(unitManager.Units[k].Position2D))
                        {
                            colorData[j * rows + i] = Color.Red;
                            unitAtPos = true;
                            break;
                        }
                    }

                    // Draw camera rectangle on map
                    if ((i == cameraLeft && j == cameraTop) || 
                        (i == cameraLeft && j >= cameraTop && j <= cameraBottom) ||
                        (i == cameraRight && j >= cameraTop && j <= cameraBottom) ||
                        (i >= cameraLeft && i <= cameraRight && j == cameraTop) || 
                        (i >= cameraLeft && i <= cameraRight && j == cameraBottom))
                    {
                        colorData[j * rows + i] = Color.White;
                        continue;
                    }

                    // If no unit at position draw tile color
                    if (!unitAtPos)
                        colorData[j * rows + i] = tileEngine.Tiles[i][j].Color;
                }
            }
            this.MapTexture.SetData(colorData);

            spriteBatch.Draw(assets.Textures["1x1Texture"], this.mapBackgroundRectanlge, Color.Black);
            spriteBatch.Draw(this.MapTexture, this.MapRectangle, Color.White);
        }
    }
}
