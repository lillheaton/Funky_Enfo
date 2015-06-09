using System;

using FunkyEnfo.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FunkyEnfo
{
    public class GameManager : Game
    {
        public BaseScreen CurrentScreen { get; set; }
        public GraphicsDeviceManager Graphics { get; private set; }
        public AssetsManager AssetsManager { get; private set; }
        private SpriteBatch spriteBatch;
        
        public GameManager() : base()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            this.Graphics.PreferredBackBufferWidth = 1200;
            this.Graphics.PreferredBackBufferHeight = 900;
            this.Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.IsMouseVisible = true;
            this.CurrentScreen = new GameScreen(this);
        }

        // This method is called before Initialize
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.AssetsManager = new AssetsManager(this.Content, this.GraphicsDevice);

            //Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //Graphics.IsFullScreen = true;
            //Graphics.ApplyChanges();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            this.CurrentScreen.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);            
            this.CurrentScreen.Draw(spriteBatch, gameTime);
            base.Draw(gameTime);
        }
    }
}
