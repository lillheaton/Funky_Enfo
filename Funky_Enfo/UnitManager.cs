using FunkyEnfo.Map;
using FunkyEnfo.Screens;
using FunkyEnfo.Units;
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
        public List<BaseUnit> Units { get; set; }

        private const int WaveLenght = 5;
        private Enfo screen;
        private TimeSpan waveUpdatePerMilliseconds;
        private TimeSpan lastUpdateTime;
        private int currentWaveUnit;
        private bool drawForces;
        private Random random;

        public UnitManager(Enfo screen)
        {
            this.screen = screen;
            this.Units = new List<BaseUnit>();
            this.random = new Random();
            this.waveUpdatePerMilliseconds = TimeSpan.FromMilliseconds(10000);

            this.LoadUnits();
        }

        private void LoadUnits()
        {
            this.Player = new Revenant(new Vector2(120, 120), this.screen);
            this.Units.Add(Player);
        }

        private void HandleWaves(GameTime gameTime)
        {
            lastUpdateTime += gameTime.ElapsedGameTime;
            if (lastUpdateTime > this.waveUpdatePerMilliseconds)
            {
                lastUpdateTime -= this.waveUpdatePerMilliseconds;
                var revenant = new Wisp(new Vector2(6 * random.Next(58, 64), 2 * 64), screen);

                // End position
                revenant.MoveToPosition(MapHelper.GoalPosition);
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
            for (int i = 0; i < Units.Count; i++)
            {
                Units[i].Update(gameTime);
                Units[i].DrawForces = drawForces;
            }

            this.HandleWaves(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Todo: think of a better way to do this, for performance
            Units = Units.OrderBy(s => s.Position.X).ToList();

            for (int i = 0; i < Units.Count; i++)
            {
                Units[i].Draw(spriteBatch, gameTime);
            }
        }

        public void ToggleShowForces()
        {
            this.drawForces = !this.drawForces;
        }

        public BaseUnit UnitAt(Vector2 position)
        {
            for (int i = 0; i < Units.Count; i++)
            {
                // Get the first unit that are in position
                if (Vector2.Distance(Units[i].Position2D, position) < Units[i].UnitRadius)
                {
                    return Units[i];
                }
            }

            return null;
        }
    }
}