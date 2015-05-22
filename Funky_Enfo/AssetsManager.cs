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
        public Dictionary<string, Spritesheet2D> Texture2Ds { get; private set; }
        public Dictionary<string, SpriteFont> Fonts { get; private set; }

        private readonly ContentManager contentManager;

        public AssetsManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            this.Texture2Ds = new Dictionary<string, Spritesheet2D>();
            this.Fonts = new Dictionary<string, SpriteFont>();

            this.LoadTextures();
            this.LoadFonts();
        }

        private void LoadTextures()
        {
            this.Texture2Ds.Add("Tiles", this.LoadSpritesheet("Tiles/tiles_spritesheet"));
            this.Texture2Ds.Add("Revenant_Move", this.LoadSpritesheet("Revenant/Move/revenant_moveSpritesheet"));
            this.Texture2Ds.Add("Whisp_Attack", this.LoadSpritesheet("Whisp/Attack/wisp_attacksheet"));
        }

        private void LoadFonts()
        {
            this.Fonts.Add("MyFont", contentManager.Load<SpriteFont>("Fonts/MyFont"));
        }

        private Spritesheet2D LoadSpritesheet(string path)
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