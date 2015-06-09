using FunkyEnfo.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace FunkyEnfo
{
    public class AssetsManager
    {
        public Dictionary<string, Spritesheet2D> Spritesheets { get; private set; }
        public Dictionary<string, SpriteFont> Fonts { get; private set; }
        public Dictionary<string, Texture2D> Textures { get; private set; }

        private readonly ContentManager contentManager;
        private readonly GraphicsDevice graphicsDevice;

        public AssetsManager(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.contentManager = contentManager;
            this.graphicsDevice = graphicsDevice;
            this.Textures = new Dictionary<string, Texture2D>();
            this.Spritesheets = new Dictionary<string, Spritesheet2D>();
            this.Fonts = new Dictionary<string, SpriteFont>();

            this.LoadTextures();
            this.LoadSpritesheets();
            this.LoadFonts();
        }

        private void LoadSpritesheets()
        {
            this.Spritesheets.Add("Tiles", this.LoadSpritesheet("Tiles/tiles_spritesheet", 0));
            this.Spritesheets.Add("Revenant_Move", this.LoadSpritesheet("Revenant/Move/revenant_moveSpritesheet", 8));
            this.Spritesheets.Add("Revenant_Attack", this.LoadSpritesheet("Revenant/Attack/revenant_attacksheet", 8));
            this.Spritesheets.Add("Whisp_Attack", this.LoadSpritesheet("Whisp/Attack/wisp_attacksheet", 8));
            this.Spritesheets.Add("Whisp_Move", this.LoadSpritesheet("Whisp/Move/wisp_movesheet", 8));
        }

        private void LoadTextures()
        {
            var t1 = new Texture2D(graphicsDevice, 1, 1);
            t1.SetData<Color>(new Color[] {Color.White});

            this.Textures.Add("1x1Texture", t1);
            this.Textures.Add("Circle_Green_18", PrimitivesHelper.CreateCirlceTexture(graphicsDevice, 18, new Color(62, 225, 54)));
            this.Textures.Add("Revenant_Projectile", contentManager.Load<Texture2D>("Revenant/Projectile"));
        }

        private void LoadFonts()
        {
            this.Fonts.Add("MyFont", contentManager.Load<SpriteFont>("Fonts/MyFont"));
        }





        private Spritesheet2D LoadSpritesheet(string path, int perAnimation)
        {
            var spritesheet = new Spritesheet2D { Texture = contentManager.Load<Texture2D>(path), SpritePosition = new Dictionary<string, Vector2>() };
            var data = ReadFile(string.Format("Content/{0}.txt", path));

            var names = new List<string>();
            foreach (var s in data)
            {
                var split = s.Split(' ');

                names.Add(split[0]);
                spritesheet.SpritePosition.Add(split[0], new Vector2(float.Parse(split[2]), float.Parse(split[3])));
                spritesheet.SpriteSize = float.Parse(split[4]);
            }

            spritesheet.Names = names.ToArray();
            spritesheet.PerAnimation = perAnimation;
            return spritesheet;
        }

        public static string[] ReadFile(string file)
        {
            var data = new List<string>();

            using (var stream = TitleContainer.OpenStream(file))
            using (var reader = new StreamReader(stream))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    data.Add(line);
                }
            }

            return data.ToArray();
        }
    }
}