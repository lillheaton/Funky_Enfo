using FunkyEnfo.Screens;
using FunkyEnfo.Units;
using Lillheaton.Monogame.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunkyEnfo
{
    public class UnitManager
    {
        public Revenant Player { get; set; }
        public List<IBoid> Units { get; set; }

        private const int WaveLenght = 5;
        private Enfo screen;
        private TimeSpan waveUpdatePerMilliseconds;
        private TimeSpan lastUpdateTime;
        private int currentWaveUnit;
        private bool drawForces;

        public UnitManager(Enfo screen)
        {
            this.screen = screen;
            this.Units = new List<IBoid>();
            this.waveUpdatePerMilliseconds = TimeSpan.FromMilliseconds(10000);

            this.LoadUnits();
        }

        private void LoadUnits()
        {
            this.Player = new Revenant(new Vector2(120, 120), this.screen);
        }

        private void HandleWaves(GameTime gameTime)
        {
            lastUpdateTime += gameTime.ElapsedGameTime;
            if (lastUpdateTime > this.waveUpdatePerMilliseconds)
            {
                lastUpdateTime -= this.waveUpdatePerMilliseconds;
                var revenant = new Wisp(new Vector2(6 * 64, 2 * 64), screen);
                revenant.MoveToPosition(new Vector2(12 * 64, 49 * 64));
                this.Units.Add(revenant);

                currentWaveUnit++;
                if (currentWaveUnit == WaveLenght)
                {
                    this.waveUpdatePerMilliseconds = TimeSpan.FromMilliseconds(10000);
                    currentWaveUnit = 0;
                }
                else
                {
                    this.waveUpdatePerMilliseconds = TimeSpan.FromMilliseconds(1000);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            this.Player.Update(gameTime);

            foreach (var unit in Units.OfType<BaseUnit>())
            {
                unit.DrawForces = drawForces;
                unit.Update(gameTime);
            }

            this.HandleWaves(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Player.Draw(spriteBatch, gameTime);

            foreach (var unit in Units.OfType<BaseUnit>())
            {
                unit.Draw(spriteBatch, gameTime);
            }
        }

        public void ToggleShowForces()
        {
            this.drawForces = !this.drawForces;
        }
    }
}