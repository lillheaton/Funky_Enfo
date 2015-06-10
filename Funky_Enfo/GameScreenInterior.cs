using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FunkyEnfo
{
    public class GameScreenInterior
    {
        private AssetsManager assets;

        public GameScreenInterior(AssetsManager assets)
        {
            this.assets = assets;
        }

        private void CreateInterior()
        {

        }


        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var height = spriteBatch.GraphicsDevice.Viewport.Height;
            var width = spriteBatch.GraphicsDevice.Viewport.Width;
            var stoneWidth = assets.Textures["StoneLine"].Width;

            spriteBatch.Draw(assets.Textures["1x1Texture"], new Vector2(0, height - 280), null, null, null, 0f, new Vector2(width, 280), Color.Black);
            spriteBatch.Draw(assets.Textures["StoneLine"], new Vector2(0, height - 300), new Rectangle(0, 0, assets.Textures["StoneLine"].Width, 20), Color.White);

            spriteBatch.Draw(assets.Textures["StoneLine"], new Vector2(stoneWidth, height - 300), null, new Rectangle(0, 0, assets.Textures["StoneLine"].Width, 20), new Vector2(0,0), (float)Math.PI * 2.5f, null, Color.White);

            spriteBatch.Draw(assets.Textures["StoneLine"], new Vector2(stoneWidth, height - 280), null, new Rectangle(0, 0, assets.Textures["StoneLine"].Width, 20), null, 0f, new Vector2(2, 1), Color.White);
        }
    }
}
