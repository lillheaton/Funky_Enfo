using FunkyEnfo.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FunkyEnfo.Units.Attacks
{
    public class RevenantProjectile
    {
        public bool Done { get; private set; }
        public BaseUnit Target { get; private set; }

        private Texture2D texture;
        private Vector2 position;

        public RevenantProjectile(Vector2 position, BaseUnit target, Texture2D texture)
        {
            this.texture = texture;
            this.position = position;
            this.Target = target;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Done)
            {
                spriteBatch.Draw(this.texture, position, Color.White);
            }   
        }

        public void Update(GameTime gameTime)
        {
            if (Vector2.Distance(this.Target.Position2D, position) > 5)
            {
                var v = Vector2.Normalize(this.Target.Position.ToVec2() - position) * 3f;
                this.position = this.position + v;
            }
            else
            {
                this.Done = true;
            }
        }
    }
}