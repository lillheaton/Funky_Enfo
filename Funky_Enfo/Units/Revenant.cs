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
        public override sealed int CurrentHealth { get; set; }
        public override sealed int MaxHealth { get; set; }

        private const int AttackDistance = 500;

        private BaseUnit currentEnemy;
        private Path currentPath;
        private List<RevenantProjectile> projectiles;

        private bool isMoving;
        private bool hasFireProjectile;

        public Revenant(Vector2 position, GameScreen screen) : base(screen.Assets.Spritesheets["Revenant_Move"], screen)
        {
            this.CurrentHealth = 300;
            this.MaxHealth = 300;

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
                    this.currentEnemy = this.Screen.UnitManager.UnitAt(pos);

                    if (currentEnemy == null || !this.InRange(currentEnemy.Position2D, AttackDistance))
                    {
                        this.TargetPosition = pos;

                        isMoving = true;

                        currentPath =
                            new Path(
                                MapHelper.CalculatePath(this.Screen.TileEngine, this.Position2D, pos)
                                    .Select(s => new Vector3(s, 0))
                                    .ToList());

                        // Set current to move spritesheet
                        this.UnitAnimation.Spritesheet = Screen.Assets.Spritesheets["Revenant_Move"];
                    }
                });
        }




        // ==========================
        // ====== Draw logic ======
        // ==========================

        private void HandleProjectilesDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < this.projectiles.Count; i++)
            {
                this.projectiles[i].Draw(spriteBatch, gameTime);
            }
        }



        // ==========================
        // ====== Update logic ======
        // ==========================

        private void HandleSteering(GameTime gameTime)
        {
            if (currentPath != null)
            {
                this.SteeringBehavior.FollowPath(currentPath, this.Screen.UnitManager.Units.ToArray());
                this.SteeringBehavior.CollisionAvoidance(this.Screen.TileEngine.Obstacles);
                this.SteeringBehavior.Update(gameTime);
            }

            // Problem with steeringBehavior. Never arrives to point
            isMoving = Vector2.Distance(this.Position2D, this.TargetPosition) > 20f;
        }

        private void HandleProjectilesUpdate(GameTime gameTime)
        {
            for (int i = 0; i < this.projectiles.Count; i++)
            {
                this.projectiles[i].Update(gameTime);
                if (this.projectiles[i].Done)
                {
                    this.projectiles[i].Target.CurrentHealth -= 10;
                    this.projectiles.RemoveAt(i);
                }
            }
        }

        private void EnemyAwareness()
        {
            // If any enemy is close OR currentEnemy is close
            // Stop moveing
            // CurrentEnemy is any of above
            // Change spritesheet to attack
            // Face enemy

            var enemy = currentEnemy = currentEnemy ?? (!isMoving ? this.EnemyInAttackingRange() : null);

            if (enemy != null && this.InRange(enemy.Position2D, AttackDistance))
            {
                this.SteeringBehavior.Stop();
                this.Attack();
            }

            if (enemy != null && enemy.IsDead)
            {
                currentEnemy = null;
            }
        }





        // ==========================
        // ======== Helpers =========
        // ==========================

        private void Attack()
        {
            // Set current spritesheet to attack
            this.UnitAnimation.Spritesheet = Screen.Assets.Spritesheets["Revenant_Attack"];

            // If user attacks, fire projectile on first spritesheet frame
            if (this.UnitAnimation.CurrentSpritePosition % this.UnitAnimation.Spritesheet.PerAnimation == 0 && !hasFireProjectile)
            {
                // This bool is used for just adding 1 projection every spritesheet cycle
                this.hasFireProjectile = true;

                // Add a projectile
                this.projectiles.Add(new RevenantProjectile(this.Position2D, this.currentEnemy, Screen.Assets.Textures["Revenant_Projectile"]));
                FacePosition(this.currentEnemy.Position2D);
            }
            else if (this.UnitAnimation.CurrentSpritePosition % this.UnitAnimation.Spritesheet.PerAnimation != 0)
            {
                this.hasFireProjectile = false;
            }
        }

        /// <returns>An enemy if it's in attacking range</returns>
        private BaseUnit EnemyInAttackingRange()
        {
            // Check if any enemy is in range
            for (int i = 0; i < this.Screen.UnitManager.Units.Count; i++)
            {
                // This unit is in the list, continue if it finds it
                if (this.Screen.UnitManager.Units[i] == this)
                    continue;

                // Check distance is less than attacking distance
                var d = Vector2.Distance(this.Position2D, this.Screen.UnitManager.Units[i].Position2D);
                if (d < AttackDistance)
                {
                    return this.Screen.UnitManager.Units[i];
                }
            }

            return null;
        }  

        // Does not work, rethink!        
        private void FacePosition(Vector2 position)
        {
            this.Velocity += Vector3.Normalize(new Vector3(position, 0));
        }
    }
}