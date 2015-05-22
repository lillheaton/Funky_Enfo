
using FunkyEnfo.Models;
using FunkyEnfo.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FunkyEnfo
{
    public class UnitManager
    {
        public List<BaseUnit> Units { get; set; }
        private AssetsManager assets;
        private Camera camera;

        public UnitManager(Camera camera, AssetsManager assets)
        {
            this.assets = assets;
            this.camera = camera;
            this.Units = new List<BaseUnit>();

            this.LoadUnits();
        }

        private void LoadUnits()
        {
            var revenantAssets = new Dictionary<string, Spritesheet2D>();
            revenantAssets.Add("Revenant_Move", assets.Texture2Ds["Revenant_Move"]);

            this.Units.Add(new Revenant(camera, revenantAssets));            
        }

        public void Update(GameTime gameTime)
        {
            foreach (var unit in Units)
            {
                unit.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var unit in Units)
            {
                unit.Draw(spriteBatch, gameTime);
            }
        }
    }
}
