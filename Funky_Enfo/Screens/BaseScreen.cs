using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FunkyEnfo.Screens
{
    public abstract class BaseScreen
    {
        public GameManager Game { get; private set; }
        public Camera Camera { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        protected BaseScreen(GameManager game)
        {
            this.Game = game;
            this.Camera = new Camera();
            this.Width = game.Graphics.PreferredBackBufferWidth;
            this.Height = game.Graphics.PreferredBackBufferHeight;            
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void Update(GameTime gameTime);
    }
}