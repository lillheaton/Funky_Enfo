﻿using FunkyEnfo.Map;
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
        public int UnitReachedGoal { get; private set; }

        private const int WaveLenght = 5;
        private GameScreen screen;
        private TimeSpan waveUpdatePerMilliseconds;
        private TimeSpan lastUpdateTime;
        private int currentWaveUnit;
        private bool drawForces;
        private bool drawHealthBar;

        public UnitManager(GameScreen screen)
        {
            this.screen = screen;
            this.Units = new List<BaseUnit>();
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
                var westWisp = new Wisp(new Vector2(5 * screen.TileEngine.TileSize, 2 * screen.TileEngine.TileSize), screen);
                var eastWisp = new Wisp(new Vector2(20 * screen.TileEngine.TileSize, 2 * screen.TileEngine.TileSize), screen);

                // End position
                westWisp.MoveToPosition(MapHelper.GoalPosition);
                eastWisp.MoveToPosition(MapHelper.GoalPosition);

                this.Units.Add(eastWisp);
                this.Units.Add(westWisp);

                currentWaveUnit++;
                if (currentWaveUnit == WaveLenght)
                {
                    this.waveUpdatePerMilliseconds = TimeSpan.FromMilliseconds(10000);
                    currentWaveUnit = 0;
                }
                else
                {
                    this.waveUpdatePerMilliseconds = TimeSpan.FromMilliseconds(2000);
                }
            }
        }




        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Units.Count; i++)
            {
                // Check if unit reached the goal
                if (Vector2.Distance(Units[i].Position2D, MapHelper.GoalPosition) < Units[i].UnitRadius)
                {
                    UnitReachedGoal++;
                    Units.RemoveAt(i);
                    continue;
                }

                // If unit is dead remove from list
                if (Units[i].IsDead)
                {
                    Units.RemoveAt(i);
                    continue;
                }

                Units[i].Update(gameTime);
                Units[i].DrawForces = drawForces;
                Units[i].DrawHealthBar = drawHealthBar;
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

        public void ShowHealthBar(bool show)
        {
            this.drawHealthBar = show;
        }

        public BaseUnit UnitAt(Vector2 position)
        {
            for (int i = 0; i < Units.Count; i++)
            {
                if (Units[i] == Player)
                    continue;

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