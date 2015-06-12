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

        private Vector2 portraitScale;
        private Vector2 portraitPosition;

        private float footerImageRatio;
        private Rectangle footerRectangle;

        private Texture2D mapTexture;
        private Rectangle mapRectangle;
        private Rectangle mapBackgroundRectanlge;

        public GameUi(AssetsManager assets, TileEngine tileEngine, UnitManager unitManager, int width, int height)
        {
            this.assets = assets;
            this.tileEngine = tileEngine;
            this.unitManager = unitManager;
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
            CalculateFooter(screenWidth, screenHeight);
            CalculatePortrait(screenHeight);
            this.CalculateMap(screenHeight);
        }

        private void CalculatePortrait(int screenHeight)
        {
            const float PortraitSpace = 150f;
            var scaledSpace = PortraitSpace * footerImageRatio;
            var scale = scaledSpace / portraitAnimation.SpriteSize;

            this.portraitScale = new Vector2(scale, scale);
            this.portraitPosition = new Vector2(504 * footerImageRatio, screenHeight - 140 * footerImageRatio);
        }

        private void CalculateFooter(int screenWidth, int screenHeight)
        {
            var footerAssetWidth = assets.Textures["Footer"].Width;
            var footerAssetHeight = assets.Textures["Footer"].Height;

            this.footerImageRatio = screenWidth / (float)footerAssetWidth;
            var footerHeight = (int)(footerAssetHeight * this.footerImageRatio);

            this.footerRectangle = new Rectangle(0, screenHeight - footerHeight, screenWidth, footerHeight);
        }

        private void CalculateMap(int screenHeight)
        {
            const float UiMapSize = 276f;
            var mapSize = UiMapSize * footerImageRatio;

            var mapWidth = tileEngine.Tiles.Length;
            var mapHeight = tileEngine.Tiles[0].Length;

            if (mapWidth > mapHeight)
            {
                float ratio = mapSize / mapWidth;
                this.mapRectangle = new Rectangle(0, 0, (int)mapSize, (int)(mapHeight * ratio));
            }
            else
            {
                float ratio = mapSize / mapHeight;
                this.mapRectangle = new Rectangle(0, 0, (int)(mapWidth * ratio), (int)mapSize);
            }

            this.mapRectangle.X = (int)((mapSize / 2f) - 35 * footerImageRatio);
            this.mapRectangle.Y = (int)(screenHeight - mapRectangle.Height - 14 * footerImageRatio);

            mapBackgroundRectanlge = new Rectangle((int)(20 * footerImageRatio), (int)(screenHeight - mapRectangle.Height - 14 * footerImageRatio), (int)mapSize, (int)mapSize);
        }




        private void DrawMap(SpriteBatch spriteBatch)
        {
            mapTexture = mapTexture ?? new Texture2D(spriteBatch.GraphicsDevice, tileEngine.Tiles.Length, tileEngine.Tiles[0].Length);
            var colorData = Enumerable.Repeat(Color.Red, (int)(tileEngine.Tiles.Length * tileEngine.Tiles[0].Length)).ToArray();

            var rows = tileEngine.Tiles.Length;
            var columns = tileEngine.Tiles[0].Length;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
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

                    if(!unitAtPos)
                        colorData[j * rows + i] = tileEngine.Tiles[i][j].Color;
                }
            }
            mapTexture.SetData(colorData);

            spriteBatch.Draw(assets.Textures["1x1Texture"], this.mapBackgroundRectanlge, Color.Black);
            spriteBatch.Draw(mapTexture, this.mapRectangle, Color.White);
        }
    }
}
