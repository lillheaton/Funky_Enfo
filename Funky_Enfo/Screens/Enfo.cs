using FunkyEnfo.Engines;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FunkyEnfo.Screens
{
    public class Enfo : BaseScreen
    {
        private TileEngine tileEngine;

        public Enfo(AssetsManager assets) : base(assets)
        {
            this.tileEngine = new TileEngine(assets);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            tileEngine.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
