using FunkyEnfo.Models;
using FunkyEnfo.Screens;
using FunkyEnfo.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace FunkyEnfo
{
    public class UnitManager
    {
        public List<BaseUnit> Units { get; set; }

        private Enfo enfoScreen;
        private KeyboardState oldState;

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
            HandleKeyboardInput();

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

        private void HandleKeyboardInput()
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.F12) && oldState.IsKeyUp(Keys.F12))
            {
                foreach (var unit in Units)
                {
                    unit.DrawForces = !unit.DrawForces;
                }
            }

            oldState = newState;
        }
    }
}
