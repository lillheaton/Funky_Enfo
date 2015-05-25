using FunkyEnfo.Map;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FunkyEnfo.Screens
{
    public class Enfo : BaseScreen
    {
        public TileEngine TileEngine { get; private set; }
        public UnitManager UnitManager { get; private set; }

        private KeyboardState oldState;

        public Enfo(AssetsManager assets) : base(assets)
        {
            this.TileEngine = new TileEngine(assets);
            this.UnitManager = new UnitManager(this);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: Camera.GetViewMatrix());

            this.TileEngine.Draw(spriteBatch, gameTime);
            this.UnitManager.Draw(spriteBatch, gameTime);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput();
            this.UnitManager.Update(gameTime);
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

            if (newState.IsKeyDown(Keys.F12) && oldState.IsKeyUp(Keys.F12))
            {
                UnitManager.ToggleShowForces();
                TileEngine.ToggleShowWaypoints();
            }

            oldState = newState;
        }
    }
}
