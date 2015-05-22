using FunkyEnfo.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FunkyEnfo.Screens
{
    public class Enfo : BaseScreen
    {
        private KeyboardState oldState;
        private TileEngine tileEngine;

        public Enfo(AssetsManager assets) : base(assets)
        {
            this.tileEngine = new TileEngine(assets);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: Camera.GetTransform());
            
            tileEngine.Draw(spriteBatch, gameTime);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput();
        }

        private void HandleKeyboardInput()
        {
            KeyboardState newState = Keyboard.GetState();
            const int CameraSpeed = 10;

            if (newState.IsKeyDown(Keys.Left))
            {
                Camera.Move(new Vector2(CameraSpeed, 0));
            }

            if (newState.IsKeyDown(Keys.Right))
            {
                Camera.Move(new Vector2(-CameraSpeed, 0));
            }

            if (newState.IsKeyDown(Keys.Down))
            {
                Camera.Move(new Vector2(0, -CameraSpeed));
            }

            if (newState.IsKeyDown(Keys.Up))
            {
                Camera.Move(new Vector2(0, CameraSpeed));
            }

            oldState = newState;
        }
    }
}
