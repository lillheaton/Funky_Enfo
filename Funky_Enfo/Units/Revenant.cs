using FunkyEnfo.Extensions;
using FunkyEnfo.Map;
using FunkyEnfo.Screens;
using FunkyEnfo.Units.Attacks;
using Lillheaton.Monogame.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunkyEnfo.Units
{
    public class Revenant : BaseUnit
    {
        private const int AttackDistance = 1000;

        private BaseUnit currentEnemy;
        private Path currentPath;
        private List<RevenantProjectile> projectiles;

        public Revenant(Vector2 position, Enfo enfo) : base(enfo.Assets.Spritesheets["Revenant_Move"], enfo)
        {
            this.projectiles = new List<RevenantProjectile>();
            this.Position2D = position;
            this.TargetPosition = position;

            this.SteeringBehavior.Settings.SeparationRadius = 100;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.EnemyAwareness();
            this.HandleProjectilesUpdate(gameTime);
            this.HandleSteering(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            this.HandleProjectilesDraw(spriteBatch, gameTime);
        }

        public void MoveToPosition(Vector2 pos)
        {
            Task.Factory.StartNew(
                () =>
                    {
                        this.SteeringBehavior.ResetPath();
                        this.currentEnemy = this.Screen.UnitManager.UnitAt(pos);

                        currentPath =
                            new Path(
                                MapHelper.CalculatePath(this.Screen.TileEngine, this.Position2D, pos)
                                    .Select(s => new Vector3(s, 0))
                                    .ToList());

                    });
        }

        private void HandleSteering(GameTime gameTime)
        {
            if (currentPath != null)
            {
                this.SteeringBehavior.FollowPath(currentPath, this.Screen.UnitManager.Units.ToArray());
                this.SteeringBehavior.CollisionAvoidance(this.Screen.TileEngine.Obstacles);
                this.SteeringBehavior.Update(gameTime);
            }
        }

        private void HandleProjectilesUpdate(GameTime gameTime)
        {
            for (int i = 0; i < this.projectiles.Count; i++)
            {
                this.projectiles[i].Update(gameTime);
                if (this.projectiles[i].Done)
                {
                    this.projectiles.RemoveAt(i);
                }
            }
        }

        private void HandleProjectilesDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < this.projectiles.Count; i++)
            {
                this.projectiles[i].Draw(spriteBatch, gameTime);
            }
        }

        private void EnemyAwareness()
        {
            // If enemy is in attacking distance, change spritesheet to attack!
            if (this.currentEnemy != null && Vector2.Distance(this.Position2D, this.currentEnemy.Position.ToVec2()) < AttackDistance)
            {
                this.CurrentSpritesheet = Screen.Assets.Spritesheets["Revenant_Attack"];
            }
            else
            {
                this.CurrentSpritesheet = Screen.Assets.Spritesheets["Revenant_Move"];
            }

            if (this.currentEnemy != null && this.CurrentSpritePosition % this.CurrentSpritesheet.PerAnimation == 0)
            {
                this.projectiles.Add(new RevenantProjectile(this.Position2D, this.currentEnemy, Screen.Assets.Textures["Revenant_Projectile"]));
            }   
        }
    }
}