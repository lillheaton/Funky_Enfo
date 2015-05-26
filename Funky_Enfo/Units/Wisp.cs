using System;
using System.Collections.Generic;

using FunkyEnfo.Map;
using FunkyEnfo.Screens;
using Lillheaton.Monogame.Steering;
using Lillheaton.Monogame.Steering.Events;

using Microsoft.Xna.Framework;
using System.Linq;

namespace FunkyEnfo.Units
{
    public class Wisp : BaseUnit
    {
        private Path currentPath;
        private List<IBoid> enemies; 

        public Wisp(Vector2 position, Enfo screen): base(screen.Assets.Spritesheets["Whisp_Move"], screen)
        {
            this.Position2D = position;
            this.TargetPosition = position;
            this.enemies = new List<IBoid>();
            this.enemies.Add(screen.UnitManager.Player);

            this.SteeringBehavior.Settings.MaxQueueRadius = 50;
            this.SteeringBehavior.EnemyAhead += SteeringBehaviorOnEnemyAhead;
        }

        private void SteeringBehaviorOnEnemyAhead(object sender, EnemyEventArgs enemyEventArgs)
        {
        }





        public override float GetMaxVelocity()
        {
            return 6f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentPath != null)
            {
                this.SteeringBehavior.FollowPath(currentPath);
                this.SteeringBehavior.CollisionAvoidance(this.Screen.TileEngine.Obstacles);
                this.SteeringBehavior.Queue(this.Screen.UnitManager.Units);
                this.SteeringBehavior.EnemyAwareness(enemies);
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