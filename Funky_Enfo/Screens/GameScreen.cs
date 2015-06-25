using System;

using FunkyEnfo.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FunkyEnfo.Screens
{
    public class GameScreen : BaseScreen
    {
        public TileEngine TileEngine { get; private set; }
        public UnitManager UnitManager { get; private set; }
        public AssetsManager Assets { get { return Game.AssetsManager; } }

        private GameUi ui;
        private KeyboardState oldState;
        private MouseState oldMouseState;

        private bool freeMouse;
        private const int CameraSpeed = 10;

        public GameScreen(GameManager game) : base(game)
        {
            this.TileEngine = new TileEngine(game.AssetsManager);
            this.UnitManager = new UnitManager(this);
            this.ui = new GameUi(
                game.AssetsManager,
                TileEngine,
                UnitManager,
                Camera,
                game.Graphics.PreferredBackBufferWidth,
                game.Graphics.PreferredBackBufferHeight);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: Camera.GetViewMatrix());
            this.TileEngine.Draw(spriteBatch, gameTime);
            this.UnitManager.Draw(spriteBatch, gameTime);
            spriteBatch.End();

            spriteBatch.Begin();
            this.ui.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            this.HandleKeyboardInput();
            this.HandleMouseInput();

            this.UnitManager.Update(gameTime);
            this.ui.Update(gameTime);
        }






        // =========================
        // ======== Inputs =========
        // =========================

        private void HandleKeyboardInput()
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Left))
            {
                Camera.Move(VecDirection.West * CameraSpeed);
            }

            if (newState.IsKeyDown(Keys.Right))
            {
                Camera.Move(VecDirection.East * CameraSpeed);
            }

            if (newState.IsKeyDown(Keys.Down))
            {
                Camera.Move(VecDirection.South * CameraSpeed);
            }

            if (newState.IsKeyDown(Keys.Up))
            {
                Camera.Move(VecDirection.North * CameraSpeed);
            }

            if (newState.IsKeyDown(Keys.F12) && oldState.IsKeyUp(Keys.F12))
            {
                this.freeMouse = !this.freeMouse;
                UnitManager.ToggleShowForces();
                TileEngine.ToggleShowWaypoints();
            }

            this.UnitManager.ShowHealthBar(newState.IsKeyDown(Keys.LeftAlt));

            oldState = newState;
        }

        private void HandleMouseInput()
        {
            var newMouseState = Mouse.GetState();

            if (newMouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed)
            {
                if (this.ui.MapRectangle.Contains(newMouseState.Position.ToVector2()))
                {
                    var diffX = newMouseState.X - this.ui.MapRectangle.X;
                    var diffY = newMouseState.Y - this.ui.MapRectangle.Y;

                    var x = (diffX * TileEngine.TileSize) * (ui.MapTexture.Width / (float)ui.MapRectangle.Width);
                    var y = (diffY * TileEngine.TileSize) * (ui.MapTexture.Height / (float)ui.MapRectangle.Height);

                    // New position for the player
                    this.UnitManager.Player.MoveToPosition(new Vector2(x, y));
                }
                else
                {
                    // New position for the player
                    this.UnitManager.Player.MoveToPosition(this.Camera.ScreenToWorld(newMouseState.Position.ToVector2()));    
                }
            }

            if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                // Move camera
                if (this.ui.MapRectangle.Contains(newMouseState.Position.ToVector2()))
                {
                    var diffX = newMouseState.X - this.ui.MapRectangle.X;
                    var diffY = newMouseState.Y - this.ui.MapRectangle.Y;

                    var x = (diffX * TileEngine.TileSize) * (ui.MapTexture.Width / (float)ui.MapRectangle.Width);
                    var y = (diffY * TileEngine.TileSize) * (ui.MapTexture.Height / (float)ui.MapRectangle.Height);

                    // New position for the player
                    this.Camera.Position = -new Vector2(x, y) + new Vector2(Width / 2f, Height / 2f);
                }
            }

            if (!freeMouse)
            {
                var x = newMouseState.X < 0 ? 0 : (newMouseState.X > this.Width ? this.Width : newMouseState.X);
                var y = newMouseState.Y < 0 ? 0 : (newMouseState.Y > this.Height ? this.Height : newMouseState.Y);
                Mouse.SetPosition(x, y);

                if (y < 10)
                {
                    Camera.Move(VecDirection.North * CameraSpeed);
                }

                if (y > Height - 10)
                {
                    Camera.Move(VecDirection.South * CameraSpeed);
                }

                if (x < 10)
                {
                    Camera.Move(VecDirection.West * CameraSpeed);
                }

                if (x > Width - 10)
                {
                    Camera.Move(VecDirection.East * CameraSpeed);
                }
            }

            if (newMouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue)
            {
                Camera.Zoom += 0.1f;
            }

            if (newMouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue)
            {
                Camera.Zoom -= 0.1f;
            }
            
            oldMouseState = newMouseState;
        }
    }
}
