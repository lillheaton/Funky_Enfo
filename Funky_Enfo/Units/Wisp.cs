using FunkyEnfo.Map;
using FunkyEnfo.Screens;
using Lillheaton.Monogame.Steering;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace FunkyEnfo.Units
{
    public class Wisp : BaseUnit
    {
        public override sealed int CurrentHealth { get; set; }
        public override sealed int MaxHealth { get; set; }

        private const int EnemyAwarenessDistance = 300;
        private const int AttackDistance = 80;
        private IBoid currentEnemy;
        private Path currentPath;

        public Wisp(Vector2 position, GameScreen screen)
            : base(screen.Assets.Spritesheets["Whisp_Move"], screen)
        {
            this.MaxHealth = 30;
            this.CurrentHealth = 30;

            this.Position2D = position;
            this.TargetPosition = position;

            this.SteeringBehavior.Settings.MaxQueueRadius = 35;
        }

        public override float GetMaxVelocity()
        {
            return 0.8f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.EnemyAwareness();
            this.HandleDeath();

            if (this.currentEnemy != null)
            {
                this.SteeringBehavior.Arrive(this.currentEnemy.Position);
            }
            else if (currentPath != null)
            {
                this.SteeringBehavior.FollowPath(currentPath);
            }

            // Don't know if they need this?
            //this.SteeringBehavior.CollisionAvoidance(this.Screen.TileEngine.Obstacles);
            this.SteeringBehavior.Queue(this.Screen.UnitManager.Units.ToArray());
            this.SteeringBehavior.Update(gameTime);
        }

        public void MoveToPosition(Vector2 pos)
        {
            Task.Factory.StartNew(
                () =>
                {
                    currentPath =
                        new Path(
                            MapHelper.CalculatePath(this.Screen.TileEngine, this.Position2D, pos)
                                .Select(s => new Vector3(s, 0))
                                .ToList());

                    this.SteeringBehavior.ResetPath();
                });
        }


        private void HandleDeath()
        {
            if (CurrentHealth <= 0)
            {
                this.UnitAnimation.Spritesheet = Screen.Assets.Spritesheets["Wisp_Death"];
                if (this.UnitAnimation.CurrentSpritePosition % this.UnitAnimation.Spritesheet.PerAnimation == 7)
                {
                    IsDead = true;
                }
            }
        }

        private void EnemyAwareness()
        {
            //Task.Factory.StartNew(
            //    () =>
            //    {
                    var enemy = Screen.UnitManager.Player;

                    // See if the enemy is in range and if I can see it, then set it to current enemy
                    if (this.InRange(enemy.Position2D, EnemyAwarenessDistance) && this.ClearViewTo(enemy))
                    {
                        currentEnemy = enemy;

                        if (this.InRange(enemy.Position2D, AttackDistance))
                        {
                            this.UnitAnimation.Spritesheet = Screen.Assets.Spritesheets["Whisp_Attack"];
                        }
                        else
                        {
                            this.UnitAnimation.Spritesheet = Screen.Assets.Spritesheets["Whisp_Move"];
                        }
                    }
                    else // Enemy is not in range
                    {
                        // If enemy is not null, which means it was in range but is no longer
                        if (currentEnemy != null)
                        {
                            // Recalculate path to goal
                            this.MoveToPosition(MapHelper.GoalPosition);
                        }
                        this.currentEnemy = null;
                    }
                //});
        }
    }
}