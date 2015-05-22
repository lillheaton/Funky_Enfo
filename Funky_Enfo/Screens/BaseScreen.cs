using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FunkyEnfo.Screens
{
    public abstract class BaseScreen
    {
        public Camera Camera { get; private set; }
        public AssetsManager Assets { get; private set; }

        protected BaseScreen(AssetsManager assets)
        {
            this.Camera = new Camera();
            this.Assets = assets;
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}