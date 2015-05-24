using FunkyEnfo.Models;
using FunkyEnfo.Screens;
using FunkyEnfo.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FunkyEnfo
{
    public class UnitManager
    {
        public List<BaseUnit> Units { get; set; }
        private Enfo enfoScreen;

        public UnitManager(Enfo screen)
        {
            this.enfoScreen = screen;
            this.Units = new List<BaseUnit>();

            this.LoadUnits();
        }

        private void LoadUnits()
        {
            this.Units.Add(new Revenant(new Vector2(120,120), enfoScreen));
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
