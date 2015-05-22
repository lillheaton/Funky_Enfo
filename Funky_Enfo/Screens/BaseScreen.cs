using Microsoft.Xna.Framework;

namespace Funky.Enfo.Screens
{
    public abstract class BaseScreen
    {
        public abstract void Draw(GameTime gameTime, AssetsManager assetsManager);
        public abstract void Update(GameTime gameTime);
    }
}
