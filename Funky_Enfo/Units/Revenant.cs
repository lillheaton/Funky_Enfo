﻿using FunkyEnfo.Map;
using FunkyEnfo.Screens;
using Lillheaton.Monogame.Steering;
using Microsoft.Xna.Framework;
using System.Linq;

namespace FunkyEnfo.Units
{
    public class Revenant : BaseUnit
    {
        private Path currentPath;
        
        public Revenant(Vector2 position, Enfo enfo) : base(enfo.Assets.Spritesheets["Revenant_Move"], enfo)
        {
            this.Position2D = position;
            this.TargetPosition = position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentPath != null)
            {
                this.SteeringBehavior.FollowPath(currentPath);
                this.SteeringBehavior.CollisionAvoidance(this.Screen.TileEngine.Obstacles);
                this.SteeringBehavior.Queue(this.Screen.UnitManager.Units);
                this.SteeringBehavior.Update(gameTime);    
            }
        }

        public void MoveToPosition(Vector2 pos)
        {
            this.SteeringBehavior.ResetPath();

            currentPath =
                new Path(
                    MapHelper.CalculatePath(this.Screen.TileEngine, this.Position2D, pos)
                        .Select(s => new Vector3(s, 0))
                        .ToList());
        }
    }
}