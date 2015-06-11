using FunkyEnfo.Map;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FunkyEnfo
{
    public class GameUi
    {
        private AssetsManager assets;
        private TileEngine tileEngine;
        private Animation portraitAnimation;

        private Vector2 portraitScale;
        private Vector2 portraitPosition;

        private float footerImageRatio;
        private Rectangle footerRectangle;

        public GameUi(AssetsManager assets, TileEngine tileEngine, int width, int height)
        {
            this.assets = assets;
            this.portraitAnimation = new Animation(assets.Spritesheets["Revenant_Portrait"], TimeSpan.FromMilliseconds(100));

            this.InitialCalculations(width, height);
        }

        public void Update(GameTime gameTime)
        {
            this.portraitAnimation.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(assets.Textures["Footer"], this.footerRectangle, Color.White);
            this.portraitAnimation.Draw(spriteBatch, this.portraitPosition, this.portraitScale);
        }




        private void InitialCalculations(int screenWidth, int screenHeight)
        {
            CalculateFooter(screenWidth, screenHeight);
            CalculatePortrait(screenHeight);
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
    }
}
